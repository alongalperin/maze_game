using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandDisplayMaze : ACommand
    {
        public CommandDisplayMaze(IModel _model, IView _view) : base (_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            return;
        }

        public override string GetName()
        {
            return "DisplayMaze";
        }
    }
}
