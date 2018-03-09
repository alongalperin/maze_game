using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;
using ATP2016Project.Presenter;

namespace ATP2016Project.Presenter.Commands
{
    class CommandFinishGame : ACommand
    {
        public CommandFinishGame(IModel _model, IView _view) : base (_model , _view) { }

        public override void DoCommand(params string[] parameters)
        {
            return;
        }

        public override string GetName()
        {
            return "FinishGame";
        }
    }
}
