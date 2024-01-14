using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class NotifyGamePlayerStateChangeCommand
    {
        public static List<Blaze.Tdf> NotifyGamePlayerStateChange(PlayerInfo pi, long stat)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            Result.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
            Result.Add(Blaze.TdfInteger.Create("STAT", stat));
            return Result;
        }


    }
}
