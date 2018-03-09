using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandGenerateMaze : ACommand
    {
        
        public CommandGenerateMaze(IModel _model, IView _view) : base(_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[1];
            int height = Int16.Parse(parameters[2]);
            int width = Int16.Parse(parameters[3]);
            int floors = Int16.Parse(parameters[4]);

            m_model.GenerateMaze(mazeName, height, width, floors);
        }

        public override string GetName()
        {
            return "GenerateMaze";
        }
    }
}
