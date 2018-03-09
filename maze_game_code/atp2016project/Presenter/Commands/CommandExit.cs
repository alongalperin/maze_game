using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;

namespace ATP2016Project.Presenter.Commands
{
    class CommandExit : ACommand
    {
    public CommandExit(IModel _model, IView _view) : base (_model, _view) { }
        
    public override void DoCommand(params string[] parameters)
    {
        m_model.ZipGeneratedMazes();
        m_model.ZipSolutions();
        //m_view.ClosingGame();
    }

    public override string GetName()
    {
        return "exit";
    }
    }
}
