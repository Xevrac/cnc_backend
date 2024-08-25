using System.Diagnostics;

namespace CNCEmu
{
    internal static class Utils
    {
        public static void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // This is necessary to use the default browser
            });
        }
    }
}
