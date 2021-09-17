using Microsoft.Extensions.Logging;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Reactive.Disposables;
using System.Windows.Forms;
using YoutubeViewer.Core.Models;

namespace YoutubeViewer.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected CompositeDisposable Disposables { get; private set; }
        protected ILogger _logger;

        protected ViewModelBase(ILogger logger)
        {
            Disposables = new CompositeDisposable();
            _logger = logger;
        }

        public virtual void Destroy()
        {

        }

        protected void ExceptionProc(Exception ex)
        {
            var exceptionBase = ex as ExceptionBase;
            _logger.Log(exceptionBase.Level, ex.InnerException, ex.Message + "  --->   " + exceptionBase.InnerException.Message + "\r\n" + ex.StackTrace + "\r\n", null);

            MessageBoxIcon icon = MessageBoxIcon.Error;
            string caption = "エラー";
            if (exceptionBase != null)
            {
                if (exceptionBase.Level == LogLevel.Information)
                {
                    icon = MessageBoxIcon.Information;
                    caption = "情報";
                }
                else if (exceptionBase.Level == LogLevel.Warning)
                {
                    icon = MessageBoxIcon.Warning;
                    caption = "警告";
                }
                MessageBox.Show(ex.Message, caption, MessageBoxButtons.OK, icon);
            }
        }
    }
}
