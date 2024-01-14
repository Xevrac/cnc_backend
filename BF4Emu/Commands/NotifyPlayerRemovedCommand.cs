using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class NotifyPlayerRemovedCommand
    {
        public static List<Blaze.Tdf> NotifyPlayerRemoved(PlayerInfo pi, long pid, long cntx, long reas)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("CNTX", cntx));
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            Result.Add(Blaze.TdfInteger.Create("PID\0", pid));
            Result.Add(Blaze.TdfInteger.Create("REAS", reas));
            return Result;
        }

    }
}
