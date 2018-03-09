﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandSolveMaze : ACommand
    {
        public CommandSolveMaze(IModel _model, IView _view) : base (_model, _view) { }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[1];
            m_model.SolveMaze(mazeName);
        }

        public override string GetName()
        {
            return "SolveMaze";
        }
    }
}
