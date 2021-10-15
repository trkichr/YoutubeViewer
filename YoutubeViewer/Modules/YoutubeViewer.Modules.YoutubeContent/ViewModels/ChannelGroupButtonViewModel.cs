using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using YoutubeViewer.Core;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Core.Mvvm;
using YoutubeViewer.Infrastructure;
using YoutubeViewer.Modules.YoutubeContent.Views;

namespace YoutubeViewer.Modules.YoutubeContent.ViewModels
{
    public class ChannelGroupButtonViewModel : RegionViewModelBase
    {
        public ChannelGroupButtonViewModel(ILogger logger,
                                                                IRegionManager regionManager)
            : this(logger, regionManager, Factories.CreateChannel())
        {
        }

        public ChannelGroupButtonViewModel(ILogger logger,
                                                                IRegionManager regionManager,
                                                                IChannelRepository channelRepository)
            : base(logger, regionManager)
        {
            _channelRepository = channelRepository;

            ChannelGroupCollection = new ObservableCollection<string>();
            ChannelGroupList = ChannelGroupCollection.ToReadOnlyReactiveCollection()
                .AddTo(Disposables);
            SelectedGroup = new ReactivePropertySlim<string>()
                .AddTo(Disposables);
            SelectedIndex = new ReactivePropertySlim<int>()
                .AddTo(Disposables);
            GroupSelectionChangedCommand = new ReactiveCommand<SelectionChangedEventArgs>()
                .WithSubscribe(GroupSelectionChanged)
                .AddTo(Disposables);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ChannelGroupCollection.Clear();
            ChannelGroupCollection.AddRange(_channelRepository.GetAllGroup());
            if (ChannelGroupCollection.Count < 1)
                return;
            var selectedChannelGroup = navigationContext.Parameters.GetValue<string>("SelectedGroupName");
            if (string.IsNullOrEmpty(selectedChannelGroup))
            {
                SelectedIndex.Value = 0;
            }
            else if (ChannelGroupCollection.Contains(selectedChannelGroup))
            {
                SelectedIndex.Value = ChannelGroupCollection.IndexOf(selectedChannelGroup);
                RequestNavigate(selectedChannelGroup);
            }
            else
            {
                SelectedIndex.Value = 0;
                RequestNavigate(ChannelGroupCollection[0]);
            }
        }

        private void RequestNavigate(string group)
        {
            var np = new NavigationParameters();
            np.Add("SelectedGroupName", group);
            RegionManager.RequestNavigate(
                RegionNames.NavigatorRegion, nameof(ChannelNavigator), np);
        }

        public void GroupSelectionChanged(SelectionChangedEventArgs e)
        {
            RequestNavigate(e.AddedItems[0] as string);
        }

        public IChannelRepository _channelRepository;
        public ObservableCollection<string> ChannelGroupCollection { get; private set; }
        public ReadOnlyReactiveCollection<string> ChannelGroupList { get; }
        public ReactivePropertySlim<string> SelectedGroup { get; }
        public ReactivePropertySlim<int> SelectedIndex { get; }
        public ReactiveCommand<SelectionChangedEventArgs> GroupSelectionChangedCommand { get; }
    }
}
