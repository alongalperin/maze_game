using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandKeyPressed : ACommand
    {
        public CommandKeyPressed(IModel _model, IView _view) : base (_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[1];
            int playerRow = Int32.Parse(parameters[2]);
            int playerCol = Int32.Parse( parameters[3] );
            int floor = Int32.Parse( parameters[4] );
            string direction = parameters[5];
           
            m_view.Move(m_model.isMovePosible(mazeName, playerRow, playerCol, floor, direction));
        }

        public override string GetName()
        {
            return "KeyPressed";
        }
    }
}
