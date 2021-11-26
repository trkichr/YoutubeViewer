using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace YoutubeViewer.Modules.PopupContents
{
    public class PopupContentsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.AddChannel>();
            containerRegistry.RegisterForNavigation<Views.EditChannel>();
        }
    }
}