using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class NotifyGameStateChangeCommand
    {
        public static List<Blaze.Tdf> NotifyGameStateChange(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            Result.Add(Blaze.TdfInteger.Create("GSTA", pi.game.GSTA));
            return Result;
        }


    }
}
