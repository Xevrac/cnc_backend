using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCEmu
{
    public static class CleanPackets
    {
        public static void Clean()
        {
            if (Directory.Exists("logs\\packets"))
                Directory.Delete("logs\\packets",true);
            if (Directory.Exists("logs"))
                Directory.Delete("logs",true);
        }
    }
}
