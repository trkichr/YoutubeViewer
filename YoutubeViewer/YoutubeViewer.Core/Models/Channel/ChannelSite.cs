namespace YoutubeViewer.Core.Models.Channel
{
    public sealed class ChannelSite : ValueObject<ChannelSite>
    {
        public ChannelSite(string id)
        {
            Id = id;
            Url = "https://www.youtube.com/channel/" + id + "/videos";
        }

        public string Id { get; }
        public string Url { get; }

        protected override bool EqualCore(ChannelSite other)
        {
            return Id == other.Id;
        }
    }
}
