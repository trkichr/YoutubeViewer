using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using YoutubeViewer.Modules.YoutubeContent.Views;

namespace YoutubeViewer.Modules.YoutubeContent
{
    public class YoutubeContentModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ChannelGroupButton>();
            containerRegistry.RegisterForNavigation<ChannelNavigator>();
            containerRegistry.RegisterForNavigation<WebContnet>();
        }
    }
}