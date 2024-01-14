using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace BF4Emu
{
    public enum LogPriority
    {
        high = 0,
        medium = 1,
        low = 2
    }

    public static class Logger
    {
        public static readonly object _sync = new object();
        public static int PacketCounter = 0;
        public static RichTextBox box = null;
        public static LogPriority LogLevel = LogPriority.low;


        public static string LevelToString(LogPriority level)
        {
            switch (level)
            {
                case LogPriority.high:
                    return " [HIGH]";
                case LogPriority.medium:
                    return " [MED ]";
                case LogPriority.low:
                    return " [LOW ]";
            }
            return "";
        }


        public static void Log(string s, object color = null)
        {
            if (box == null) return;
            try
            {
                box.Invoke(new Action(delegate
                {
                    string stamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " : ";
                    Color c;
                    if (color != null)
                        c = (Color)color;
                    else
                        c = Color.Black;
                    box.SelectionStart = box.TextLength;
                    box.SelectionLength = 0;
                    box.SelectionColor = c;
                    box.AppendText(stamp + s + "\n");
                    BackendLog.Write(stamp + s + "\n");
                    box.SelectionColor = box.ForeColor;
                    box.ScrollToCaret();
                }));
            }
            catch { }
        }

        public static void LogError(string who, Exception e, string cName = "")
        {
            string result = "";
            if (who != "") result = "[" + who + "] " + cName + " ERROR: ";
            result += e.Message;
            if (e.InnerException != null)
                result += " - " + e.InnerException.Message;
            Log(result);
        }

        public static void LogPacket(string source, int handlerID, byte[] data)
        {
            lock (_sync)
            {
                File.WriteAllBytes("logs\\packets\\Packet_" + (PacketCounter++).ToString("d6") + "_Handler" + handlerID + "_" + UnixTimeNow().ToString() + "_" + source + ".bin", data);
            }
        }

        public static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

    }
}
