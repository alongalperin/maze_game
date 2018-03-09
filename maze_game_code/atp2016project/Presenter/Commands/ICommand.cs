using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Presenter.Commands
{
    /// <summary>
    /// represents Command in the poroject.
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// run the command. excute the code of the command
        /// </summary>
        /// <param name="parameters"></param>
        void DoCommand(params string[] parameters);

        /// <summary>
        /// return the name of the command, each command should have a name
        /// </summary>
        string GetName();
    }
}
