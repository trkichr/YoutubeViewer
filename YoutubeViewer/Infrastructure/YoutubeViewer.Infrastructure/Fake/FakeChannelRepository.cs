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
        public FakeChannelRepository()
        {
            var op = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
            };
            using (StreamReader sr = new StreamReader("channel_fake.json"))
            {
                _channelList = JsonSerializer.Deserialize<IEnumerable<ChannelEntityCreator>>(sr.ReadToEnd(), op)
                    .Select(x => x.GetChannelEntity())
                    .ToList();
            }
        }

        public IEnumerable<ChannelEntity> GetAll()
        {
            return _channelList;
        }

        public IEnumerable<ChannelEntity> GetAll(string group)
        {
            return _channelList
                .Where(x => x.Group == group);
        }

        public IEnumerable<string> GetAllGroup()
        {
            return _channelList
                .GroupBy(x => x.Group)
                .Select(x => x.Key);
        }

        public void Add(ChannelEntity selectedChannel)
        {
            if (!_channelList.Contains(selectedChannel))
            {
                _channelList.Add(selectedChannel);
            }
        }

        public void Delete(ChannelEntity selectedChannel)
        {
            if (_channelList.Contains(selectedChannel))
            {
                _channelList.Remove(selectedChannel);
            }
        }

        public void Replace(ChannelEntity selectedChannel)
        {
            var channel = _channelList.FirstOrDefault(x => x.Site.Id == selectedChannel.Site.Id);
            if (channel != null)
            {
                _channelList.Remove(channel);
                _channelList.Add(selectedChannel);
            }
        }

        List<ChannelEntity> _channelList;
    }
}
