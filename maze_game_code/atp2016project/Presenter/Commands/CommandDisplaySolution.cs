using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;


namespace ATP2016Project.Presenter.Commands
{
    class CommandDisplaySolution : ACommand
    {
        public CommandDisplaySolution(IModel _model, IView _view) : base(_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[1];
            if (m_model.isSolutionExists(mazeName) == true)
            {
                bool flag = true;
                m_view.setDisplaySoutionMode(flag);
                int currentFloor = m_view.GetCurrentFloor();
                byte[] mazeInBytes = m_model.getMazeFloorInByte(mazeName, currentFloor);

                if (m_view.isSetStepsConfigured() == false)
                {
                    m_view.setSolutionSteps( m_model.getSolutionList(mazeName) );
                }

                m_view.displayFloor(mazeInBytes);
            }
            else
            {
                m_view.DisplayMessage("please generate solution first");
            }
        }

        public override string GetName()
        {
            return "DisplaySolution";
        }
    }
}
