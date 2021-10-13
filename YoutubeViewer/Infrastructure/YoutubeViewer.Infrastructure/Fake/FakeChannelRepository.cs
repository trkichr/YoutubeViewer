using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using YoutubeViewer.Core.Models.Channel;

namespace YoutubeViewer.Infrastructure.Fake
{
    public class FakeChannelRepository : IChannelRepository
    {

        public IEnumerable<ChannelEntity> GetAll()
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel_fake.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                    .Select(x => x.GetChannelEntity());
            }
        }

        public IEnumerable<ChannelEntity> GetAll(string group)
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel_fake.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                .Where(x => x.Group == group)
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
            using (StreamReader sr = new StreamReader("channel_fake.json"))
            {
                return JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                .GroupBy(x => x.Group)
                .Select(x => x.Key);
            }
        }

        public void Add(ChannelEntity channel)
        {
        }

        public void Delete(ChannelEntity channel)
        {
        }

        public void Replace(ChannelEntity channel)
        {
        }
    }
}
