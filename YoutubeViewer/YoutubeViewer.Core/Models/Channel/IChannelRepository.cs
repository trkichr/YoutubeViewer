using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeViewer.Core.Models.Channel
{
    public interface IChannelRepository
    {
        public IEnumerable<ChannelEntity> GetAll();
        public IEnumerable<ChannelEntity> GetAll(string group);
        public IEnumerable<string> GetAllGroup();
        public void Add(ChannelEntity channel);
        public void Delete(ChannelEntity channel);
        public void Replace(ChannelEntity channel);
    }
}
