using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using YoutubeViewer.Core;
using YoutubeViewer.Modules.YoutubeContent.Views;

namespace YoutubeViewer.Modules.YoutubeContent
{
    public class YoutubeContentModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public YoutubeContentModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.GroupButtonRegion, nameof(ChannelGroupButton));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ChannelGroupButton>();
            containerRegistry.RegisterForNavigation<ChannelNavigator>();
            containerRegistry.RegisterForNavigation<WebContnet>();
        }
    }
}