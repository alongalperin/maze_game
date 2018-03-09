using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    /// <summary>
    /// abstract class for represent the command in the system. all command should inherite from this class
    /// </summary>
    abstract class ACommand : ICommand
    {
        protected IView m_view;
        protected IModel m_model;
        
            /// <summary>
            /// basic constructor for abstract command
            /// </summary>
            /// <param name="_model"></param>
            /// <param name="_view"></param>
            public ACommand (IModel _model, IView _view)
            {
                m_view = _view;
                m_model = _model;
            }

        /// <summary>
        /// run the command. excute the code of the command
        /// </summary>
        /// <param name="parameters"></param>
        public abstract void DoCommand(params string[] parameters);

        /// <summary>
        /// return the name of the command, each command should have a name
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();
    }
}
