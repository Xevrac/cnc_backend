using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class NotifyUserRemovedCommand
    {
        public static List<Blaze.Tdf> NotifyUserRemoved(PlayerInfo pi, long pid)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("BUID", pid));
            return Result;
        }

    }
}
