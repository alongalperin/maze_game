using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandLoadMaze : ACommand
    {
        public CommandLoadMaze(IModel _model, IView _view) : base(_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            string path = parameters[1];
            string mazeName = parameters[2];

            m_model.LoadMaze(mazeName, path);
        }

        public override string GetName()
        {
            return "LoadMaze";
        }
    }
}
