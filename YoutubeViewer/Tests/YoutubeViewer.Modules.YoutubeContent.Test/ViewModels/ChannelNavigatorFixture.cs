using Microsoft.Extensions.Logging;
using Moq;
using Prism.Regions;
using Prism.Services.Dialogs;
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

namespace YoutubeViewer.Modules.YoutubeContent.Test.ViewModels
{
    public class ChannelNavigatorFixture
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IDialogService> _dialogMock;
        private static Mock<IRegion> _mockChannelGroupButtonRegion;
        private static Mock<IRegion> _mockChannelNavigatorRegion;
        private static Mock<IRegion> _mockContentRegion;
        private static IRegionManager _rm;

        public ChannelNavigatorFixture()
        {
            _loggerMock = new Mock<ILogger>();
            _dialogMock = new Mock<IDialogService>();
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
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }

        [Fact]
        public void OnNavigatedToシナリオ2()
        {
            var channelRepositoryMock = new Mock<IChannelRepository>();
            channelRepositoryMock.Setup(x => x.GetAllGroup())
                .Returns(new List<string>());
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object, channelRepositoryMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(0);

            var channelRepository = Factories.CreateChannel();
            np = new NavigationParameters();
            np.Add("ChannelUrl", channelRepository.GetAll("クラウドサービス"));
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np),
                                                                            Times.Never);
        }

        [Fact]
        public void ChannelSelectionChangedシナリオ1()
        {
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            var addedItems = new List<ChannelEntity>();
            addedItems.AddRange(vm._channelRepository.GetAll());
            var removedItems = new List<string>();
            var selectionChangedEventArgs = new SelectionChangedEventArgs(Selector.SelectionChangedEvent, removedItems, addedItems);
            vm.ChannelSelectionChanged(selectionChangedEventArgs);

            var np = new NavigationParameters();
            np.Add("ChannelUrl", addedItems[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }

        [Fact]
        public void ChannelSelectionChangedシナリオ2()
        {
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            var addedItems = new List<ChannelEntity>();
            var removedItems = new List<string>();
            var selectionChangedEventArgs = new SelectionChangedEventArgs(Selector.SelectionChangedEvent, removedItems, addedItems);
            vm.ChannelSelectionChanged(selectionChangedEventArgs);

            var channelRepository = Factories.CreateChannel();
            var np = new NavigationParameters();
            np.Add("ChannelUrl", channelRepository.GetAll("クラウドサービス"));
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np),
                                                                            Times.Never);
        }

        [Fact]
        public void DownSelectedChannelシナリオ1()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);
            var channelCol = vm.ChannelCollection.ToList();

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));

            vm.DownSelectedChannel();
            vm.ChannelCollection[0].Is(channelCol[1]);
            vm.ChannelCollection[1].Is(channelCol[0]);
            vm.ChannelCollection[2].Is(channelCol[2]);
        }

        [Fact]
        public void UpSelectedChannelシナリオ1()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var x = nc.Parameters;
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);
            var channelCol = vm.ChannelCollection.ToList();

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));

            vm.SelectedIndex.Value = 1;
            vm.UpSelectedChannel();
            vm.ChannelCollection[0].Is(channelCol[1]);
            vm.ChannelCollection[1].Is(channelCol[0]);
            vm.ChannelCollection[2].Is(channelCol[2]);
        }


        [Fact]
        public void RemoveSelectedChannelシナリオ1()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));

            np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var channel = vm.ChannelCollection[0];
            vm.SelectedChannel.Value = channel;
            vm.RemoveSelectedChannel();
            _mockChannelGroupButtonRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelGroupButton, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Contains(channel).IsFalse();
        }

        [Fact]
        public void EditSelectedChannelシナリオ1()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            DialogParameters dp = new DialogParameters();
            dp.Add("Channel", new ChannelEntity("FakeTitle", "クラウドサービス", "UCJS9pqu9BzkAMNTmzNMNhvg", "FakeId"));
            var dialogResult = new DialogResult(ButtonResult.OK, dp);
            _dialogMock.Setup(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<IDialogParameters>(), It.IsAny<Action<IDialogResult>>()))
                .Callback((string name, IDialogParameters parameters, Action<IDialogResult> callback) =>
                    callback(dialogResult));
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));

            np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var channel = vm.ChannelCollection[0];
            vm.SelectedChannel.Value = channel;
            vm.EditSelectedChannel();
            _mockChannelGroupButtonRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelGroupButton, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
            vm.OnNavigatedTo(nc);
            var editedChannel = vm.ChannelList.First(x => x.Site.Id == channel.Site.Id);
            editedChannel.IsNotNull();
            editedChannel.Title.Is("FakeTitle");
        }

        [Fact]
        public void AddChannelシナリオ1()
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            var channel = new ChannelEntity("FakeTitle", "クラウドサービス", "FakeSiteId", "FakeId");
            DialogParameters dp = new DialogParameters();
            dp.Add("Channel", channel);
            var dialogResult = new DialogResult(ButtonResult.OK, dp);
            _dialogMock.Setup(x => x.ShowDialog(It.IsAny<string>(), It.IsAny<IDialogParameters>(), It.IsAny<Action<IDialogResult>>()))
                .Callback((string name, IDialogParameters parameters, Action<IDialogResult> callback) =>
                    callback(dialogResult));
            var njMock = new Mock<IRegionNavigationJournal>();
            var nsMock = new Mock<IRegionNavigationService>();
            IRegion region = new Region();
            nsMock.SetupGet(n => n.Region).Returns(region);
            nsMock.SetupGet(x => x.Journal).Returns(njMock.Object);
            var nc = new NavigationContext(nsMock.Object, new Uri(ViewNames.ChannelNavigator, UriKind.Relative), np);
            var vm = new ChannelNavigatorViewModel(_loggerMock.Object, _rm, _dialogMock.Object);
            vm.OnNavigatedTo(nc);
            vm.ChannelCollection.Count.Is(3);
            vm.SelectedIndex.Value.Is(0);

            np = new NavigationParameters();
            np.Add("ChannelUrl", vm.ChannelCollection[0]);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.WebContnet, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));

            np = new NavigationParameters();
            np.Add("SelectedGroupName", "クラウドサービス");
            vm.AddChannel();
            _mockChannelGroupButtonRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.ChannelGroupButton, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
            vm.OnNavigatedTo(nc);
            vm.ChannelList.Contains(channel).IsTrue();
        }
    }
}
