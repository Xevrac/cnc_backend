using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class UserUpdatedCommand
    {
        public static List<Blaze.Tdf> UserUpdated(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("FLGS", 3));
            Result.Add(Blaze.TdfInteger.Create("ID", pi.userId));

            return Result;
        }

    }
}
