using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using YoutubeViewer.Core.Models.Channel;

namespace YoutubeViewer.Infrastructure
{
    internal sealed class ChannelEntityCreator
    {
        public ChannelEntityCreator()
        {
        }

        public ChannelEntityCreator(string title, string group, string urlId, string avatarId)
        {
            Title = title;
            Group = group;
            UrlId = urlId;
            AvatarId = avatarId;
        }

        public ChannelEntity GetChannelEntity()
        {
            return new ChannelEntity(Title, Group, UrlId, AvatarId);
        }

        [JsonPropertyName("Title")]
        public string Title { get; set; }
        [JsonPropertyName("Group")]
        public string Group { get; set; }
        [JsonPropertyName("UrlId")]
        public string UrlId { get; set; }
        [JsonPropertyName("AvatarId")]
        public string AvatarId { get; set; }
    }
}
