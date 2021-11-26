using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using YoutubeViewer.Core;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Core.Mvvm;
using YoutubeViewer.Infrastructure;
using YoutubeViewer.Modules.YoutubeContent.Views;

namespace YoutubeViewer.Modules.YoutubeContent.ViewModels
{
    public class ChannelNavigatorViewModel : RegionViewModelBase
    {
        public ChannelNavigatorViewModel(ILogger logger,
                                                            IRegionManager regionManager,
                                                            IDialogService ds)
            :this(logger, regionManager, ds, Factories.CreateChannel())
        {
        }

        public ChannelNavigatorViewModel(ILogger logger,
                                                            IRegionManager regionManager,
                                                            IDialogService ds,
                                                            IChannelRepository channelRepository)
            : base(logger, regionManager)
        {
            _ds = ds;
            _channelRepository = channelRepository;

            ChannelCollection = new ObservableCollection<ChannelEntity>();
            ChannelList = ChannelCollection.ToReadOnlyReactiveCollection()
                .AddTo(Disposables);
            SelectedChannel = new ReactivePropertySlim<ChannelEntity>()
                .AddTo(Disposables);
            SelectedIndex = new ReactivePropertySlim<int>()
                .AddTo(Disposables);
            ChannelSelectionChangedCommand = new ReactiveCommand<SelectionChangedEventArgs>()
                .WithSubscribe(ChannelSelectionChanged)
                .AddTo(Disposables);
            AddChannelCommand = new ReactiveCommand()
                .WithSubscribe(AddChannel)
                .AddTo(Disposables);
            RemoveChannelCommand = SelectedChannel
                .Select(x => x != null)
                .ToReactiveCommand()
                .WithSubscribe(RemoveSelectedChannel)
                .AddTo(Disposables);
            EditChannelCommand = SelectedChannel
                .Select(x => x != null)
                .ToReactiveCommand()
                .WithSubscribe(EditSelectedChannel)
                .AddTo(Disposables);
            UpChannelCommand = SelectedIndex
                .Select(x => x > 0)
                .ToReactiveCommand()
                .WithSubscribe(UpSelectedChannel)
                .AddTo(Disposables);
            DownChannelCommand = SelectedIndex
                .Select(x => x < ChannelList.Count - 1)
                .ToReactiveCommand()
                .WithSubscribe(DownSelectedChannel)
                .AddTo(Disposables);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _selectedChannelGroup =
                navigationContext.Parameters.GetValue<string>("SelectedGroupName");

            ChannelCollection.Clear();
            ChannelCollection.AddRange(_channelRepository.GetAll(_selectedChannelGroup));
            if (ChannelCollection.Count > 0)
            {
                SelectedIndex.Value = 0;
                NavigateContentRegion(ChannelCollection[0]);
            }
        }

        private void NavigateContentRegion(ChannelEntity channel)
        {
            var np = new NavigationParameters();
            np.Add("ChannelUrl", channel);
            RegionManager.RequestNavigate(
                RegionNames.ContentRegion, nameof(WebContnet), np);
        }

        public void ChannelSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e?.AddedItems.Count > 0)
            {
                NavigateContentRegion((ChannelEntity)e.AddedItems[0]);
            }
        }

        public void DownSelectedChannel()
        {
            ChannelCollection.Move(SelectedIndex.Value, SelectedIndex.Value + 1);
        }

        public void UpSelectedChannel()
        {
            ChannelCollection.Move(SelectedIndex.Value, SelectedIndex.Value - 1);
        }

        public void RemoveSelectedChannel()
        {
            _channelRepository.Delete(SelectedChannel.Value);
            NavigationParameters np = new NavigationParameters();
            np.Add("SelectedGroupName", SelectedChannel.Value.Group);
            RegionManager.RequestNavigate(
                RegionNames.GroupButtonRegion, nameof(ChannelGroupButton), np);
        }

        public void EditSelectedChannel()
        {
            IDialogParameters dp = new DialogParameters();
            dp.Add(nameof(SelectedChannel), SelectedChannel.Value);

            _ds.ShowDialog(nameof(ViewNames.EditChannel), dp, x =>
            {
                ChannelEntity channel = GetDialogResult(x);
                if (channel != null
                    && _channelRepository.GetAll().Contains(SelectedChannel.Value))
                {
                    _channelRepository.Replace(channel);
                    NavigationParameters np = new NavigationParameters();
                    np.Add("SelectedGroupName", channel.Group);
                    RegionManager.RequestNavigate(
                        RegionNames.GroupButtonRegion, nameof(ChannelGroupButton), np);
                }
            });
        }

        public void AddChannel()
        {
            _ds.ShowDialog(nameof(ViewNames.AddChannel), null, x =>
            {
                ChannelEntity channel = GetDialogResult(x);
                if (channel != null
                    && !_channelRepository.GetAll().Contains(channel))
                {
                    _channelRepository.Add(channel);
                    NavigationParameters np = new NavigationParameters();
                    np.Add("SelectedGroupName", channel.Group);
                    RegionManager.RequestNavigate(
                        RegionNames.GroupButtonRegion, nameof(ChannelGroupButton), np);
                }
            });
        }

        private ChannelEntity GetDialogResult(IDialogResult dialogResult)
        {
            if (dialogResult.Result != ButtonResult.OK)
                return null;
            if (dialogResult.Parameters == null)
                return null;
            dialogResult.Parameters.TryGetValue("Channel", out ChannelEntity channel);
            return channel;
        }

        private IDialogService _ds;
        public IChannelRepository _channelRepository;
        private string _selectedChannelGroup;
        public ObservableCollection<ChannelEntity> ChannelCollection { get; private set; }
        public ReadOnlyReactiveCollection<ChannelEntity> ChannelList { get; }
        public ReactivePropertySlim<ChannelEntity> SelectedChannel { get; }
        public ReactivePropertySlim<int> SelectedIndex { get; }
        public ReactiveCommand<SelectionChangedEventArgs> ChannelSelectionChangedCommand { get; }
        public ReactiveCommand AddChannelCommand { get; }
        public ReactiveCommand RemoveChannelCommand { get; }
        public ReactiveCommand EditChannelCommand { get; }
        public ReactiveCommand UpChannelCommand { get; }
        public ReactiveCommand DownChannelCommand { get; }
    }
}
