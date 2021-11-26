using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace YoutubeViewer.Core.Mvvm
{
    public static class Shared
    {
        public static bool IsActualFiles { get; } =
            ConfigurationManager.AppSettings["IsActualFiles"] == "1";
        public static string FakePath { get; } =
            ConfigurationManager.AppSettings["FakePath"];
    }
}
