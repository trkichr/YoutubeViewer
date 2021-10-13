namespace YoutubeViewer.Core.Models.Channel
{
    public sealed class Avatar : ValueObject<Avatar>
    {
        public Avatar(string id)
        {
            Id = id;
            Url = "https://yt3.ggpht.com/" + id + "=s48-c-k-c0x00ffffff-no-rj";
        }

        public string Id { get; }
        public string Url { get; }

        protected override bool EqualCore(Avatar other)
        {
            return Id == other.Id;
        }
    }
}
