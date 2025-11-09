using Crazy_zoo;
using Crazy_zoo.Data;
using Crazy_zoo.Logging;
using Crazy_zoo.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Windows;

namespace Crazy_zoo
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, ev) =>
            {
                MessageBox.Show(ev.ExceptionObject.ToString(), "Unhandled Exception");
            };

            DispatcherUnhandledException += (s, ev) =>
            {
                MessageBox.Show(ev.Exception.ToString(), "Dispatcher Exception");
                ev.Handled = true;
            };

            try
            {
                var services = new ServiceCollection();

                services.AddSingleton<ILogger, XMLLogger>();

                string connectionString = ConfigurationManager
                    .ConnectionStrings["ZooDB"]
                    .ConnectionString;

                services.AddSingleton<IRepository<Animal>>(
                    sp => new SqlRepository<Animal>(connectionString));

                services.AddSingleton<ZooViewModel>();
                services.AddSingleton<MainWindow>();

                ServiceProvider = services.BuildServiceProvider();

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Startup Exception");
            }

            base.OnStartup(e);
        }
    }
}
