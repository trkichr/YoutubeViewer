using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Core.Mvvm;
using YoutubeViewer.Infrastructure.Fake;
using YoutubeViewer.Infrastructure.Json;

namespace YoutubeViewer.Infrastructure
{
    public static class Factories
    {
        public static IChannelRepository CreateChannel()
        {
#if DEBUG
            if (Shared.IsActualFiles)
            {
                return new JsonChannelRepository();
            }
            else
            {
                return new FakeChannelRepository();
            }
#endif
            return new JsonChannelRepository();
        }
    }
}
