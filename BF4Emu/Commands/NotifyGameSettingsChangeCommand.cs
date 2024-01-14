using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class NotifyGameSettingsChangeCommand
    {
        public static List<Blaze.Tdf> NotifyGameSettingsChange(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("ATTR", pi.game.GSET));
            Result.Add(Blaze.TdfInteger.Create("GID", pi.game.id));
            return Result;
        }

    }
}
