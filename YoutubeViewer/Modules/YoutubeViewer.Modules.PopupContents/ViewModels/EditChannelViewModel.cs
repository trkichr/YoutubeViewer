using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YoutubeViewer.Core.Models.Channel;
using YoutubeViewer.Core.Mvvm;

namespace YoutubeViewer.Modules.PopupContents.ViewModels
{
    public class EditChannelViewModel : ViewModelBase, IDialogAware
    {
        public EditChannelViewModel(ILogger logger)
            : base(logger)
        {
            Title = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateAttribute(() => Title)
                .AddTo(Disposables);
            Group = new ReactiveProperty<string>()
                .AddTo(Disposables);
            UrlId = new ReactiveProperty<string>()
                .AddTo(Disposables);
            AvatarId = new ReactiveProperty<string>(string.Empty, ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateAttribute(() => AvatarId)
                .AddTo(Disposables);

            OkCommand = new[]
                {
                    Title.ObserveHasErrors,
                    AvatarId.ObserveHasErrors
                }
                .CombineLatestValuesAreAllFalse()
                .ToReactiveCommand()
                .WithSubscribe(CloseDialog)
                .AddTo(Disposables);
            CancelCommand = new ReactiveCommand()
                .WithSubscribe(Cancel)
                .AddTo(Disposables);
        }

        private void Cancel()
        {
            _dialogResult = new DialogResult(ButtonResult.Cancel);
            this.CloseDialog(_dialogResult);
        }

        private void CloseDialog()
        {
            _dialogParameters = new DialogParameters();
            _dialogParameters.Add("Channel", new ChannelEntity(Title.Value, Group.Value, UrlId.Value, AvatarId.Value));
            _dialogResult = new DialogResult(ButtonResult.OK, _dialogParameters);
            this.CloseDialog(_dialogResult);
        }

        private void CloseDialog(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        string IDialogAware.Title => "チャンネルの編集";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var selectedChannel = parameters.GetValue<ChannelEntity>("SelectedChannel");
            Title.Value = selectedChannel.Title;
            Group.Value = selectedChannel.Group;
            UrlId.Value = selectedChannel.Site.Id;
            AvatarId.Value = selectedChannel.Avatar.Id;
        }

        public IDialogParameters _dialogParameters;
        public IDialogResult _dialogResult;
        [Required()]
        public ReactiveProperty<string> Title { get; }
        public ReactiveProperty<string> Group { get; }
        public ReactiveProperty<string> UrlId { get; }
        [Required()]
        [RegularExpression(@"[-/\w]{32,128}")]
        public ReactiveProperty<string> AvatarId { get; }
        public ReactiveCommand OkCommand { get; }
        public ReactiveCommand CancelCommand { get; }
    }
}
