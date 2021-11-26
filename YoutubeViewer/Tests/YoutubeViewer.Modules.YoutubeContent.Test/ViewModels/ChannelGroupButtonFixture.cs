using Microsoft.Extensions.Logging;
using Moq;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xunit;
using YoutubeViewer.Core;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Infrastructure;
using YoutubeViewer.Modules.YoutubeContent.ViewModels;
using YoutubeViewer.Modules.YoutubeContent.Views;
using YoutubeViewer.Services.Interfaces;

namespace YoutubeViewer.Modules.YoutubeContent.Test.ViewModels
{
    public class ChannelGroupButtonFixture
    {
        private Mock<ILogger> _loggerMock;
        private static Mock<IRegion> _mockChannelGroupButtonRegion;
        private static Mock<IRegion> _mockChannelNavigatorRegion;
        private static IRegionManager _rm;

        public ChannelGroupButtonFixture()
        {
            _loggerMock = new Mock<ILogger>();
            _mockChannelGroupButtonRegion = new Mock<IRegion>();
            _mockChannelNavigatorRegion = new Mock<IRegion>();
            _mockChannelGroupButtonRegion.SetupGet((r) => r.Name).Returns(RegionNames.GroupButtonRegion);
            _mockChannelNavigatorRegion.SetupGet((r) => r.Name).Returns(RegionNames.NavigatorRegion);
            _rm = new RegionManager();
            _rm.Regions.Add(_mockChannelGroupButtonRegion.Object);
            _rm.Regions.Add(_mockChannelNavigatorRegion.Object);
        }

        [Fact]
        public void OnNavigatedToシナリオ１()
        {
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative));
            var vm = new ChannelGroupButtonViewModel(_loggerMock.Object, _rm);
            vm.OnNavigatedTo(nc);
            vm.ChannelGroupCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);
        }

        [Fact]
        public void OnNavigatedToシナリオ2()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x =nc.Parameters;
            var vm = new ChannelGroupButtonViewModel(_loggerMock.Object, _rm);
            vm.OnNavigatedTo(nc);
            vm.ChannelGroupCollection.Count.Is(3);
            int index = vm.ChannelGroupCollection.IndexOf(np.GetValue<string>("SelectedGroupName"));
            vm.SelectedIndex.Value.Is(index);

            _mockChannelNavigatorRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelNavigator, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }

        [Fact]
        public void OnNavigatedToシナリオ3()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "Fake");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelGroupButtonViewModel(_loggerMock.Object, _rm);
            vm.OnNavigatedTo(nc);
            vm.ChannelGroupCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);

            np = new NavigationParameters();
            np.Add("SelectedGroupName", vm.ChannelGroupCollection[0]);
            _mockChannelNavigatorRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelNavigator, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }

        [Fact]
        public void OnNavigatedToシナリオ4()
        {
            var channelRepositoryMock = new Mock<IChannelRepository>();
            channelRepositoryMock.Setup(x => x.GetAllGroup())
                .Returns(new List<string>());
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "Fake");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelGroupButtonViewModel(_loggerMock.Object, _rm, channelRepositoryMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelGroupCollection.Count.Is(0);
            vm.SelectedIndex.Value.Is(0);
            vm.SelectedGroup.Value.Is(null);

            var channelRepository = Factories.CreateChannel();
            np = new NavigationParameters();
            np.Add("ChannelUrl", channelRepository.GetAllGroup().First());
            _mockChannelNavigatorRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelNavigator, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np),
                                                                            Times.Never);
        }

        [Fact]
        public void GroupSelectionChanged()
        {
            var addedItems = new List<string>();
            addedItems.Add("testGroup");
            var removedItems = new List<string>();
            var selectionChangedEventArgs = new SelectionChangedEventArgs(Selector.SelectionChangedEvent, removedItems, addedItems);
            var vm = new ChannelGroupButtonViewModel(_loggerMock.Object, _rm);
            vm.GroupSelectionChanged(selectionChangedEventArgs);

            var np = new NavigationParameters();
            np.Add("SelectedGroupName", addedItems[0]);
            _mockChannelNavigatorRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelNavigator, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }
    }
}
