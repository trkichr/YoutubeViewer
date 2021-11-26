using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Core.Mvvm;
using YoutubeViewer.Services.Interfaces;

namespace YoutubeViewer.Modules.YoutubeContent.ViewModels
{
    public class WebContnetViewModel : RegionViewModelBase
    {
        public WebContnetViewModel(ILogger logger,
                                                    IRegionManager regionManager,
                                                    ISystemService systemService)
            : base(logger, regionManager)
        {
            _systemService = systemService;
            WebViewLoadedCommand = new ReactiveCommand<RoutedEventArgs>()
                .WithSubscribe(e => _webView = (WebView2)e.Source)
                .AddTo(Disposables);
            WebNavigationCompletedCommand = new ReactiveCommand<CoreWebView2NavigationCompletedEventArgs>()
                .WithSubscribe(e => WebNavigationCompleted(e))
                .AddTo(Disposables);
            WebSourceChangedCommand = new ReactiveCommand<CoreWebView2SourceChangedEventArgs>()
                .AddTo(Disposables);

            ChannelUrl = new ReactivePropertySlim<string>()
                .AddTo(Disposables);
            BrowserBackCommand = WebSourceChangedCommand
                .Select(x => _webView.CanGoBack)
                .ToReactiveCommand()
                .WithSubscribe(BrowserBack)
                .AddTo(Disposables);
            BrowserForwardCommand = WebSourceChangedCommand
                .Select(x => _webView.CanGoForward)
                .ToReactiveCommand()
                .WithSubscribe(BrowserForward)
                .AddTo(Disposables);
            RefreshCommand = new ReactiveCommand()
                .WithSubscribe(Refresh)
                .AddTo(Disposables);
            OpenInBrowserCommand = new ReactiveCommand()
                .WithSubscribe(OpenInBrowser)
                .AddTo(Disposables);
            LoadingVisibility = new ReactivePropertySlim<Visibility>(Visibility.Visible)
                .AddTo(Disposables);
            FailedMesageVisibility = new ReactivePropertySlim<Visibility>(Visibility.Collapsed)
                .AddTo(Disposables);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            ChannelEntity channel = navigationContext.Parameters.GetValue<ChannelEntity>("ChannelUrl");
            ChannelUrl.Value = channel.Site.Url;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void WebNavigationCompleted(CoreWebView2NavigationCompletedEventArgs e)
        {
            LoadingVisibility.Value = Visibility.Collapsed;
            if (e != null && !e.IsSuccess)
            {
                FailedMesageVisibility.Value = Visibility.Visible;
            }
        }

        private void OpenInBrowser()
        {
            _systemService.OpenInWebBrowser(ChannelUrl.Value);
        }

        private void Refresh()
        {
            FailedMesageVisibility.Value = Visibility.Collapsed;
            LoadingVisibility.Value = Visibility.Visible;
            _webView.Reload();
        }

        private void BrowserForward()
        {
            _webView.GoForward();
        }

        private void BrowserBack()
        {
            _webView.GoBack();
        }

        private readonly ISystemService _systemService;
        public WebView2 _webView;
        public ReactiveCommand<RoutedEventArgs> WebViewLoadedCommand { get; }
        public ReactiveCommand<CoreWebView2NavigationCompletedEventArgs> WebNavigationCompletedCommand { get; }
        public ReactiveCommand<CoreWebView2SourceChangedEventArgs> WebSourceChangedCommand { get; }
        public ReactivePropertySlim<string> ChannelUrl { get; set; }
        public ReactiveCommand BrowserBackCommand { get; }
        public ReactiveCommand BrowserForwardCommand { get; }
        public ReactiveCommand RefreshCommand { get; }
        public ReactiveCommand OpenInBrowserCommand { get; }
        public ReactivePropertySlim<Visibility> LoadingVisibility { get; }
        public ReactivePropertySlim<Visibility> FailedMesageVisibility { get; }
    }
}
