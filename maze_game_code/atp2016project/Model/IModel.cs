using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016.MazeGenerators;
using System.Collections;

namespace ATP2016Project.Model
{
    public delegate void modelEventDelegate(string problemCode);

    interface IModel
    {
        event modelEventDelegate ModelChanged;

        void GenerateMaze(string name, int height, int width, int floors);

        byte[] getMazeInByte(string name, int floor);

        byte[] getMazeFloorInByte(string name, int floor);

        Position getStrartPoint(string name);

        Position getGoalPoint(string name);

        int[] getDimentions(string name);

        void SaveMaze(string path, string mazeName);

        void LoadMaze(string path, string mazeName);

        void isMazeExists(string name);

        List<string> getGeneratedMazes();

        void SolveMaze(string mazeName);

        ArrayList getSolutionList(string currentDisplayed);

        bool isSolutionExists(string mazeName);

        AMaze getMaze(string name);

        string isMovePosible(string mazeName, int row, int col, int floor, string direction);

        void ZipGeneratedMazes();

        void ZipSolutions();
    }
}
