using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;


namespace BF4Emu
{
    public static class Config
    {
        private static readonly object _sub = new object();
        private static string loc = Path.GetDirectoryName(Application.ExecutablePath) + "\\";
        public static string ConfigFile = loc + "conf\\conf.txt";

        private static readonly object _sync = new object();
        public static List<string> Entries;

        public static string LogLevel;
        public static string MakePacket;

        public static void Credits()
        {
            Logger.Log("-- BF4 Emulator by Eisbaer --");
            Logger.Log("-----------------------------------------------");
            Logger.Log("-- Credits: --");
            Logger.Log("-----------------------------------------------");
            Logger.Log("The1Domo for his Initial Work...");
            Logger.Log("Warranty Voider for his Framework and his Help.");
            Logger.Log("Manu157 for his Help.");
            Logger.Log("BSHTornado for his Help.");
            Logger.Log("Zlofenix for his Help.");
            Logger.Log("Andersson799 for his Help.");
            Logger.Log("-----------------------------------------------");
        }


        public static void Init()
        {
            if (!Directory.Exists("conf"))
            {
                Directory.CreateDirectory("conf");
            }

            if (!File.Exists(ConfigFile))
            {
                Write("LogLevel = Low");
                Write("MakePacket = true");
            }

        }

        public static void InitialConfig()
        {
            try
            {
                if (File.Exists(loc + "conf\\conf.txt"))
                {
                    Entries = new List<string>(File.ReadAllLines(loc + "conf\\conf.txt"));

                    LogLevel = Config.FindEntry("LogLevel");
                    Logger.Log("LogLevel = " + LogLevel);

                    MakePacket = Config.FindEntry("MakePacket");
                    Logger.Log("MakePacket's = " + MakePacket);

                }
                else
                {
                Logger.Log("[CONF]" + loc + " conf\\conf.txt loading failed");
                }
            }
            catch(Exception Ex)
            {
                Logger.Log("InitialConfig Error: ", Ex);
            }
        }

        public static string FindEntry(string name)
        {
            string s = "";
            lock (_sync)
            {
                for (int i = 0; i < Entries.Count; i++)
                {
                    string line = Entries[i];
                    if (line.Trim().StartsWith("#"))
                        continue;
                    string[] parts = line.Split('=');
                    if (parts.Length != 2)
                        continue;
                    if (parts[0].Trim().ToLower() == name.ToLower())
                        return parts[1].Trim();
                }
            }
            return s;
        }

        public static string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }

        public static void Write(string s)
        {
            File.AppendAllText(ConfigFile, s);
        }
    }
}
