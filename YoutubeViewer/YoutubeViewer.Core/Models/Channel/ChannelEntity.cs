using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public ImageSource AvatarImage
        {
            get
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource =
                    new Uri(Avatar.Url);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }
    }
}
