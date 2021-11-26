using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Moq;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Xunit;
using YoutubeViewer.Core;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Modules.YoutubeContent.ViewModels;
using YoutubeViewer.Services.Interfaces;

namespace YoutubeViewer.Modules.YoutubeContent.Test.ViewModels
{
    public class WebContnetFixture
    {
        private Mock<ILogger> _loggerMock;
        private Mock<ISystemService> _systemServiceMock;
        private static Mock<IRegion> _mockChannelGroupButtonRegion;
        private static Mock<IRegion> _mockChannelNavigatorRegion;
        private static Mock<IRegion> _mockContentRegion;
        private static IRegionManager _rm;

        public WebContnetFixture()
        {
            _loggerMock = new Mock<ILogger>();
            _systemServiceMock = new Mock<ISystemService>();
            _mockChannelGroupButtonRegion = new Mock<IRegion>();
            _mockChannelNavigatorRegion = new Mock<IRegion>();
            _mockContentRegion = new Mock<IRegion>();
            _mockChannelGroupButtonRegion.SetupGet((r) => r.Name).Returns(RegionNames.GroupButtonRegion);
            _mockChannelNavigatorRegion.SetupGet((r) => r.Name).Returns(RegionNames.NavigatorRegion);
            _mockContentRegion.SetupGet((r) => r.Name).Returns(RegionNames.ContentRegion);

            _rm = new RegionManager();
            _rm.Regions.Add(_mockChannelGroupButtonRegion.Object);
            _rm.Regions.Add(_mockChannelNavigatorRegion.Object);
            _rm.Regions.Add(_mockContentRegion.Object);
        }

        [Fact]
        public void OnNavigatedToシナリオ1()
        {
            var np = new NavigationParameters();
            var channel = new ChannelEntity("FakeTitle", "クラウドサービス", "FakeSiteId", "FakeAvatarId");
            np.Add("ChannelUrl", channel);
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new WebContnetViewModel(_loggerMock.Object, _rm, _systemServiceMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelUrl.Value.Is(channel.Site.Url);
        }

        [Fact]
        public void IsNavigationTargetシナリオ1()
        {
            var np = new NavigationParameters();
            var channel = new ChannelEntity("FakeTitle", "クラウドサービス", "FakeSiteId", "FakeAvatarId");
            np.Add("ChannelUrl", channel);
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new WebContnetViewModel(_loggerMock.Object, _rm, _systemServiceMock.Object);
            bool isNavigationTarget = true;
            isNavigationTarget = vm.IsNavigationTarget(nc);
            isNavigationTarget.IsFalse();
        }
    }
}
