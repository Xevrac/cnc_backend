using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF4Emu
{
    public class Profile
    {
        public string _raw;
        public string name;
        public long id;

        public static Profile Load(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            Profile p = new Profile();
            p._raw = File.ReadAllText(filename);
            foreach (string line in lines)
            {
                string[] parts = line.Split('=');
                if (parts.Length != 2) continue;
                string what = parts[0].Trim().ToLower();
                switch (what)
                {
                    case "name":
                        p.name = parts[1].Trim();
                        break;
                    case "id":
                        p.id = Convert.ToInt32(parts[1].Trim());
                        break;
                }
            }
            if (p.name == null || p.id == 0)
                return null;
            return p;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ID = " + id + ", ");
            sb.Append("Name = " + name);
            return sb.ToString();
        }
    }

    public static class Profiles
    {
        public static List<Profile> profiles = new List<Profile>();
        public static void Refresh()
        {
            profiles = new List<Profile>();
            if (!Directory.Exists("backend"))
                Directory.CreateDirectory("backend");
            if (!Directory.Exists("backend\\profiles"))
                Directory.CreateDirectory("backend\\profiles");
            string[] files = Directory.GetFiles("backend\\profiles\\", "*.txt");
            foreach (string file in files)
            {
                Profile p = Profile.Load(file);
                if (p != null)
                    profiles.Add(p);
            }
            BlazeServer.Log("[MAIN] Loaded " + profiles.Count + " player profiles");
        }

        public static string getProfilePath(long id)
        {
            return "backend\\profiles\\" + id.ToString("X8") + "_profile.txt";
        }

        public static Profile Create(string name)
        {
            long id = 1000;
            while (File.Exists(getProfilePath(id)))
                id++;
            return Create(name, id);
        }

        public static Profile Create(string name, long id)
        {
            string profileContent = "name=" + name + "\nid=" + id;
            string filename = getProfilePath(id);
            File.WriteAllText(filename, profileContent, Encoding.Unicode);
            return Profile.Load(filename);
        }
    }
}
