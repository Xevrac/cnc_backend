using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;


namespace BF4Emu
{
    class NotifyUserStatusCommand
    {
        public static List<Blaze.Tdf> NotifyUserStatus(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("FLGS", 3));
            Result.Add(Blaze.TdfInteger.Create("ID\0\0", pi.userId));
            return Result;
        }
    }
}
