using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CNCEmu
{
    public static class GenFiles
    {
        public static void CreatePackets()
        {
            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");
            if (!Directory.Exists("logs\\packets"))
                Directory.CreateDirectory("logs\\packets");
        }
    }
}
