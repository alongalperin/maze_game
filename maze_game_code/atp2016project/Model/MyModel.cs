using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ATP2016.MazeGenerators;
using ATP2016.Search;
using System.IO;
using ATP2016.Compression;
using ATP2016.MazeGenerators.MazeSearchDomain;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace ATP2016Project.Model
{
    class MyModel : IModel
    {
        public event modelEventDelegate ModelChanged;
        private Dictionary<string, AMaze> m_generatedMazes;
        private Dictionary<string, Solution> m_solutionsToMazes; // maze name, solution


        public MyModel(){
            m_generatedMazes = new Dictionary<string, AMaze>();
            m_solutionsToMazes = new Dictionary<string, Solution>();
            UnZipMazes();
            UnZipSolutions();
        }

        private void UnZipMazes()
        {
            if (File.Exists("APT2016_GeneratedMazes.zip"))
            {
                using (Stream stream = File.Open("APT2016_GeneratedMazes.zip", FileMode.Open))
                {
                    using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        m_generatedMazes = (Dictionary<string, AMaze>)binaryFormatter.Deserialize(zipStream);
                    }
                }
            }
        }   

        private void UnZipSolutions()
        {
            if (File.Exists("APT2016_GeneratedMazes.zip"))
            {
                using (Stream stream = File.Open("APT2016_Solutions.zip", FileMode.Open))
                {
                    using (var zipStream = new GZipStream(stream, CompressionMode.Decompress))
                    {
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        m_solutionsToMazes = (Dictionary<string, Solution>)binaryFormatter.Deserialize(zipStream);
                    }
                }
            }
        }

        public void GenerateMaze(string name, int height, int width, int floors)
        {
            bool isOverride = false;
            isOverride = m_generatedMazes.ContainsKey(name);
            MyMaze3dGenerator mazeGanerator = new MyMaze3dGenerator();
            Maze3d maze3d = (Maze3d)mazeGanerator.generate(height, width, floors);
            if (isOverride == false)
            {
                addToGeneratedDict(name, maze3d);
                ModelChanged("generatemaze " + name);
            }
            else // there is maze with this name already, need to delete old maze
            {
                removeGeneratedMazeFromDict(name);
                addToGeneratedDict(name, maze3d);
                ModelChanged("generatemaze " + name + " maze generated succsesfuly with override of old maze with same name");
                ModelChanged("deletedMaze " + name + " old version was overrided");
            }
        }



        #region Set / Get dictionaries

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void addToGeneratedDict(string name, Maze3d maze)
        {
            m_generatedMazes.Add(name, maze);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void addToSolutionDict(string name, Solution solution)
        {
            m_solutionsToMazes.Add(name, solution);
        }

        private void removeGeneratedMazeFromDict(string name)
        {
            m_generatedMazes.Remove(name);
        }

        private void removeSolutionFromDict(string name)
        {
            m_solutionsToMazes.Remove(name);
        }

        public byte[] getMazeInByte(string name, int floor)
        {
            Maze3d maze = (Maze3d)m_generatedMazes[name];
            return maze.toByteArray();
        }

        public byte[] getMazeFloorInByte(string name, int floor)
        {
            Maze3d maze = (Maze3d)m_generatedMazes[name];
            Maze2d mazeFloor = maze.m_lFloors[floor];
            return mazeFloor.toByteArray();
        }

        #endregion

        #region Get details about maze
            
        public Position getStrartPoint(string name)
        {
            return m_generatedMazes[name].startPoint;
        }

        public Position getGoalPoint(string name)
        {
            return m_generatedMazes[name].goalPoint;
        }

        public int[] getDimentions(string name)
        {
            Maze3d maze = (Maze3d)m_generatedMazes[name];
            int[] ans = new int[3];
            ans[0] = maze.Height;
            ans[1] = maze.Width;
            ans[2] = maze.Floors;
            return ans;
        }

        #endregion


        public void SaveMaze(string path, string name)
        {
            bool isOverRide = false; // flag that will tell us if we overeride existing file

            try
            {
                if (File.Exists(path) == true) { isOverRide = true; } // check if we override an existing file

                Maze3d maze3d = (Maze3d)m_generatedMazes[name]; // get the maze from the dictoinary
                byte[] byteMaze = maze3d.toByteArray();
                int mazeLengthInByte = byteMaze.Length;
                using (FileStream fileOutStream = new FileStream(path, FileMode.Create))
                {
                    using (Stream outStream = new MyCompressorStream(fileOutStream, MyCompressorStream.Compress))
                    {
                        outStream.Write(byteMaze, 0, mazeLengthInByte);
                        outStream.Flush();
                    }
                }

            }
            catch (Exception e)
            {

                ModelChanged("savemaze error uccored during " + name + "saving. error: " +e.Message);
            }

            if (isOverRide)
            {
                ModelChanged("savemaze " + name +" saved, override the previous file");
            }
            else
            {
                ModelChanged("savemaze " + name + " saved.");
            }
        }


        public void LoadMaze(string path, string mazeName)
        {
            bool isOverride = m_generatedMazes.ContainsKey(mazeName); // check if we override existing name of maze
            int bytesReadFromDecompress = 0; // how much decompressed info we read
            byte[] readIntoArr = new byte[50]; // we will read into this array
            List<byte> list = new List<byte>();

            try
            {
                using (FileStream decompressedFileStream = new FileStream(path, FileMode.Open))
                {
                    using (MyCompressorStream myCompressorStream = new MyCompressorStream(decompressedFileStream, MyCompressorStream.Decompress))
                    {
                        while ((bytesReadFromDecompress = myCompressorStream.Read(readIntoArr, 0, readIntoArr.Length)) != 0)
                        {
                            for (int i = 0; i < bytesReadFromDecompress; i++)
                            {
                                list.Add(readIntoArr[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelChanged("loadmaze error occured during load. Error: " + e.Message);
                return;
            }

            byte[] mazeByteArray = list.ToArray();
            Maze3d fromMazeFromLoad = new Maze3d(mazeByteArray);

            if (isOverride == true) // if we override existsing maze, we need to delete the old maze with same maze
            {
                removeGeneratedMazeFromDict(mazeName);
                addToGeneratedDict(mazeName, fromMazeFromLoad);
                ModelChanged("mazeload maze " + mazeName + " loaded succsesfuly with override of old maze with same name");
                ModelChanged("deletedMaze " + mazeName);
            }
            else
            {
                addToGeneratedDict(mazeName, fromMazeFromLoad);
                ModelChanged("mazeload maze " + mazeName + " loaded succsesfuly");
            }
        }

        public void isMazeExists(string name)
        {
            bool ans =m_generatedMazes.ContainsKey(name);
            ModelChanged("isMazeExists " + name + " " + ans.ToString());
        }


        public List<string> getGeneratedMazes()
        {
            List<string> list = new List<string>();

            foreach (KeyValuePair<string, AMaze> item in m_generatedMazes)
            {
                list.Add(item.Key);
            }

            return list;
        }


        public void SolveMaze(string mazeName)
        {
            Maze3d maze = (Maze3d)m_generatedMazes[mazeName];
            ISearchable maze3dSearchable = new SearchableMaze(maze);
            ABlindSearch bfs = new BFS();
            Solution bfsSolution = bfs.Search(maze3dSearchable);

            if (m_solutionsToMazes.ContainsKey(mazeName) == true)
            {
                removeSolutionFromDict(mazeName);
                addToSolutionDict(mazeName, bfsSolution);
                ModelChanged("solvemaze " + "maze " + mazeName + " solving done");
            }
            else
            {
                addToSolutionDict(mazeName, bfsSolution);
                ModelChanged("solvemaze " + "maze " + mazeName + " solving is done");
            }
        }


        public ArrayList getSolutionList(string currentDisplayed)
        {
            return m_solutionsToMazes[currentDisplayed].GetSolutionPath();
        }


        public bool isSolutionExists(string mazeName)
        {
            return m_solutionsToMazes.ContainsKey(mazeName);
        }


        public AMaze getMaze(string name)
        {
            return m_generatedMazes[name];
        }


        public string isMovePosible(string mazeName, int row, int col, int floor, string direction)
        {
            Maze3d maze = (Maze3d)m_generatedMazes[mazeName];  

            // check in case of step not to the goal
            int mazeHeight = maze.Height;
            int mazeWidth = maze.Width;
            int mazeFloors = maze.Floors;
            int deltaCol = 0;
            int deltaRow = 0;
            int deltaFloors = 0;
            switch (direction)
            {
                case "left":
                    deltaCol = -1;
                    break;
                case "up":
                    deltaRow = -1;
                    break;
                case "right":
                    deltaCol = 1;
                    break;
                case "down":
                    deltaRow = 1;
                    break;
                case "pageup":
                    deltaFloors = 1;
                    break;
                case "next":
                    deltaFloors = -1;
                    break;
            }
            int finalCol = col + deltaCol;
            int finalRow = row + deltaRow;
            int finalFloor = floor + deltaFloors;
            // check if the next step is the goal
            Position nextStep = new Position(finalCol, finalRow, floor);
            if (nextStep.Equals(maze.goalPoint) == true)
            {
                return "goal " + direction;
            }

            // check if the new move exists the maze borders
            if (finalCol < 0 || finalRow < 0 || finalFloor < 0)
            {
                return "false";
            }

            // check if the new move exists the maze borders
            if (finalRow >= mazeHeight)
            {
                return "false";
            }
            if (finalCol >= mazeWidth)
            {
                return "false";
            }
            if (finalFloor >= mazeFloors)
            {
                return "false";
            }
            if (maze.m_lFloors[finalFloor].getContentCell2d(finalCol, finalRow) == 1) // check if we have wall in the cell
            {
                return "false";
            }

            // check if can move up or down floor
            string UpDown = "";

            // if we have 1 floor we cant go up or down
            if (mazeFloors == 1)
            {
                return direction + "";
            }

            // if we are not in the last floor in many floors maze
            if (finalFloor == mazeFloors-1)  
            {
                if (maze.m_lFloors[finalFloor-1].getContentCell2d(finalCol, finalRow) == 0)
                {
                    return direction += " down";
                }
                return direction;
            }

            // check first floor in many floors maze
            if (finalFloor == 0 && mazeFloors > 0)
            {
                if (maze.m_lFloors[finalFloor+1].getContentCell2d(finalCol, finalRow) == 0)
                {
                    return direction += " up";
                }
                return direction;
            }

            if (maze.m_lFloors[finalFloor+1].getContentCell2d(finalCol, finalRow) == 0) // we are in middle floor
            {
                UpDown += " up";
            }

            if (maze.m_lFloors[finalFloor-1].getContentCell2d(finalFloor, finalRow) == 0)
            {
                UpDown += " down";
            }

            if (UpDown == " up down")
            {
                UpDown = " both";
            }

            return direction + UpDown;
        }


        #region Zipping
        public void ZipGeneratedMazes()
        {
            using (Stream stream = File.Open("APT2016_GeneratedMazes.zip", FileMode.Create))
            {
                using (var zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(zipStream, m_generatedMazes);
                }
            }
        }

        public void ZipSolutions()
        {
            using (Stream stream = File.Open("APT2016_Solutions.zip", FileMode.Create))
            {
                using (var zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(zipStream, m_solutionsToMazes);
                }
            }
        }
        #endregion
    }
}
