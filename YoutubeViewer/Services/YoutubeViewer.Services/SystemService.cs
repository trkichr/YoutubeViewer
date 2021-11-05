using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YoutubeViewer.Services.Interfaces;

namespace YoutubeViewer.Services
{
    public class SystemService : ISystemService
    {
        public void OpenInWebBrowser(string url)
        {
            // For more info see https://github.com/dotnet/corefx/issues/10361
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
