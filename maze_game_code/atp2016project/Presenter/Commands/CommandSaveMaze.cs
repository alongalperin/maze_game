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
    /// <summary>
    /// command for saving maze
    /// </summary>
    class CommandSaveMaze : ACommand
    {
        public CommandSaveMaze(IModel _model, IView _view) : base(_model, _view) { }


        /// <summary>
        /// we save the current maze that displayed
        /// </summary>
        /// <param name="parameters"></param>
        public override void DoCommand(params string[] parameters)
        {
            string curretntMaze = MyPresenter.getCurrentDisplayed();
            string path = parameters[1];
            m_model.SaveMaze(path, curretntMaze);
        }

        public override string GetName()
        {
            return "SaveMaze";
        }
    }
}
