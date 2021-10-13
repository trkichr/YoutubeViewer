using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeViewer.Core.Models.Channel
{
    public sealed class ChannelEntity
    {
        public ChannelEntity(string title, string group, string siteId, string avatarId)
        {
            Title = title;
            Group = group;
            Site = new ChannelSite(siteId);
            Avatar = new Avatar(avatarId);
        }

        public string Title { get; }
        public string Group { get; }
        public ChannelSite Site { get; }
        public Avatar Avatar { get; }
    }
}
