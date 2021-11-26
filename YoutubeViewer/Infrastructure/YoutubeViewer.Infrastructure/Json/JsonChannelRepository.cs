using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YoutubeViewer.Core.Models.Channel;

namespace YoutubeViewer.Infrastructure.Json
{
    public sealed class JsonChannelRepository : IChannelRepository
    {
        public IEnumerable<ChannelEntity> GetAll(string group)
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                .Where(x => x.Group == group)
                .Select(x => x.GetChannelEntity());
            }
        }

        public IEnumerable<ChannelEntity> GetAll()
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                    .Select(x => x.GetChannelEntity());
            }
        }

        public IEnumerable<string> GetAllGroup()
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                .GroupBy(x => x.Group)
                .Select(x => x.Key);
            }
        }

        public void Add(ChannelEntity channel)
        {
            var savedChannels = GetAll().ToList();
            var savedChannel = savedChannels.FirstOrDefault(x => x.Equals(channel));
            if (savedChannel == null)
            {
                savedChannels.Add(channel);
                WriteJsonFile(savedChannels);
            }
        }

        public void Delete(ChannelEntity channel)
        {
            var savedChannels = GetAll().ToList();
            var savedChannel = savedChannels.FirstOrDefault(x => x.Equals(channel));
            if (savedChannel != null)
            {
                savedChannels.Remove(channel);
                WriteJsonFile(savedChannels);
            }
        }

        public void Replace(ChannelEntity channel)
        {
            var savedChannels = GetAll().ToList();
            var savedChannel = savedChannels.FirstOrDefault(x => x.Site.Id == channel.Site.Id);
            if (savedChannel != null)
            {
                savedChannels.Remove(savedChannel);
                savedChannels.Add(channel);
                WriteJsonFile(savedChannels);
            }
        }

        private void WriteJsonFile(List<ChannelEntity> channelList)
        {
            var op = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                IgnoreNullValues = false,
                IgnoreReadOnlyProperties = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string jsonStr = JsonSerializer
                .Serialize(channelList.Select(x => new ChannelEntityCreator(x.Title, x.Group, x.Site.Id, x.Avatar.Id)), op);
            File.WriteAllText("channel.json", jsonStr);
        }
    }
}
