using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using YoutubeViewer.Core.Mvvm;

namespace YoutubeViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(ILogger logger, IRegionManager rm)
            : base(logger)
        {
            _rm = rm;
            WindowClosingCommand = new ReactiveCommand()
                .WithSubscribe(WindowClosing)
                .AddTo(Disposables);
        }

        private void WindowClosing()
        {
            foreach (var region in _rm.Regions)
            {
                region.RemoveAll();
            }
        }

        public ReactiveCommand WindowClosingCommand { get; }
        private IRegionManager _rm;
    }
}
