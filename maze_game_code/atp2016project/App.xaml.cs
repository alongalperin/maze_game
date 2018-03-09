using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ATP2016Project.Model;
using ATP2016Project.View;
using ATP2016Project.Presenter;

namespace ATP2016Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IModel model = new MyModel();
            IView view = new MainWindow();
            MyPresenter presenter = new MyPresenter(model, view);
            view.Start();
        }
    }
}
