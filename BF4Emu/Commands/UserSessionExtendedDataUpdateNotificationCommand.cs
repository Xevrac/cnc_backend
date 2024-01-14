using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class UserSessionExtendedDataUpdateNotificationCommand
    {
        public static List<Blaze.Tdf> UserSessionExtendedDataUpdateNotification(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(BlazeHelper.CreateUserDataStruct(pi));
            Result.Add(Blaze.TdfInteger.Create("USID", pi.userId));
            return Result;
        }
    }
}
