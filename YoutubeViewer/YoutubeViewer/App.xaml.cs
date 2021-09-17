using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using YoutubeViewer.Services;
using YoutubeViewer.Services.Interfaces;
using YoutubeViewer.Views;

namespace YoutubeViewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log/file.txt" };
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;
            var factory = new NLog.Extensions.Logging.NLogLoggerFactory();
            ILogger logger = factory.CreateLogger("");
            containerRegistry.RegisterInstance(logger);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
