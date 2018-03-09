using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;
using ATP2016Project.Presenter.Commands;
using ATP2016.MazeGenerators;

namespace ATP2016Project.Presenter
{
    class MyPresenter
    {
        private IModel m_model;
        private IView m_view;
        private Dictionary<string, ACommand> m_commands;
        private static string currentDisplayed;
        
        public MyPresenter(IModel _model, IView _view)
        {
            m_model = _model;
            m_view = _view;
            m_commands = new Dictionary<string, ACommand>();
            SetCommandsDict();
            SetEvents();
            currentDisplayed = "";
        }

        private void SetEvents()
        {
            m_view.ViewChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "GetGeneratedMazes")
                {
                    m_view.setGeneratedMazes(m_model.getGeneratedMazes());
                }
            };

            m_view.ViewChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "displaymaze")
                {
                    string mazeName = splittedCommand[1];
                    if (currentDisplayed == mazeName)
                    {
                        m_view.DisplayMessage("maze already displayed");
                    }
                    else // need to change maze in displayer
                    {
                        currentDisplayed = mazeName;
                        m_view.displayMaze(mazeName, (Maze3d)m_model.getMaze(mazeName));
                    }
                }
            };

            m_view.ViewChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                string commandName = splittedCommand[0].Trim().ToLower();
                ACommand command = m_commands[commandName];
                command.DoCommand(splittedCommand);
            };

            m_model.ModelChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "generatemaze")
                {
                    string mazeName = splittedCommand[1];
                    if (currentDisplayed == "") // if we dont have displayed maze yet, we will print the new maze
                    {
                        currentDisplayed = mazeName;
                        m_view.setMaze((Maze3d)m_model.getMaze(mazeName));
                        m_view.displayMaze(mazeName, (Maze3d)m_model.getMaze(mazeName));
                    }
                    else if (currentDisplayed == mazeName)
                    {
                        m_view.ToggleMazeDisplay("new maze with same name created\n you can choose display maze to display it");
                    }
                }
                else if (splittedCommand[0] == "notgeneratemaze")
                {
                    m_view.DisplayMessage("Maze " + splittedCommand[1] + " not created because of " + splittedCommand[2]);   
                }
            };

            m_view.ViewChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "finishgame")
                {
                    currentDisplayed = "";
                    m_view.FinishGame();
                }
            };

            m_model.ModelChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "savemaze" && splittedCommand[1] != "error")
                {
                    m_view.DisplayMessage(skipStringArr(splittedCommand, 1));
                }
                else if (splittedCommand[0] == "savemaze" && splittedCommand[1] == "error")
                {
                    m_view.DisplayMessage(info); // we print the all message in case of error savemaze error uccored during " + name + "saving. error: " +e.Message
                }
            };

            m_model.ModelChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if ( splittedCommand[0] =="deletedMaze")
                {
                    string mazeName = splittedCommand[1];
                    if (currentDisplayed == mazeName)
                    {
                        currentDisplayed = "";
                        m_view.ToggleMazeDisplay("this maze is not valid any more, new maze with same name");
                    }
                    else
                    {
                        m_view.ToggleMazeDisplay(skipStringArr(splittedCommand, 1));
                    }
                }
            };

            m_model.ModelChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "mazeload")
                {
                    string mazeName = splittedCommand[2];
                    if (currentDisplayed == "")
                    {
                        byte[] floorInByte = m_model.getMazeFloorInByte(mazeName, 0);
                        currentDisplayed = mazeName;
                        m_view.setStartAndGoalPoints(m_model.getStrartPoint(mazeName), m_model.getGoalPoint(mazeName));
                        m_view.setDimentions(m_model.getDimentions(mazeName));
                        m_view.displayFloor(floorInByte);
                    }
                    else if (currentDisplayed != "")
                    {
                        m_view.DisplayMessage(skipStringArr(splittedCommand, 1));
                    }
                }
            };

            m_model.ModelChanged += delegate(string info)
            {
                string[] splittedCommand = info.Trim().Split();
                if (splittedCommand[0] == "solvemaze")
                {
                    m_view.setSolutionSteps( m_model.getSolutionList(currentDisplayed) );
                    m_view.DisplayMessage(skipStringArr(splittedCommand, 1));
                }
            };

        }

        private void SetCommandsDict()
        {
            ACommand generateMaze = new CommandGenerateMaze(m_model, m_view);
            m_commands.Add(generateMaze.GetName().ToLower(), generateMaze);

            ACommand saveMaze = new CommandSaveMaze(m_model, m_view);
            m_commands.Add(saveMaze.GetName().ToLower(), saveMaze);

            ACommand loadMaze = new CommandLoadMaze(m_model, m_view);
            m_commands.Add(loadMaze.GetName().ToLower(), loadMaze);

            ACommand getGeneratedMazes = new CommandGetGeneratedMazes(m_model, m_view);
            m_commands.Add(getGeneratedMazes.GetName().ToLower(), getGeneratedMazes);

            ACommand displayMaze = new CommandDisplayMaze(m_model, m_view);
            m_commands.Add(displayMaze.GetName().ToLower(), displayMaze);

            ACommand solveMaze = new CommandSolveMaze(m_model, m_view);
            m_commands.Add(solveMaze.GetName().ToLower(), solveMaze);

            ACommand displaySolution = new CommandDisplaySolution(m_model, m_view);
            m_commands.Add(displaySolution.GetName().ToLower(), displaySolution);

            ACommand keyPressed = new CommandKeyPressed(m_model, m_view);
            m_commands.Add(keyPressed.GetName().ToLower(), keyPressed);

            ACommand finishGame = new CommandFinishGame(m_model, m_view);
            m_commands.Add(finishGame.GetName().ToLower(), finishGame);

            ACommand exitCommand = new CommandExit(m_model, m_view);
            m_commands.Add(exitCommand.GetName().ToLower(), exitCommand);
        }

        public static string getCurrentDisplayed()
        {
            return currentDisplayed;
        }

        /// <summary>
        /// combine string from array of strings, from specific index
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private string skipStringArr(string[] arr, int position)
        {
            string ans = "";

            for (int i = position; i < arr.Length; i++)
            {
                ans += arr[i] + " ";
            }

            return ans;
        }
    }
}
