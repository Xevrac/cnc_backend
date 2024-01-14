using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    class NotifyPlayerJoiningCommand
    {
        public static List<Blaze.Tdf> NotifyPlayerJoining(PlayerInfo pi)
        {
            uint t = Blaze.GetUnixTimeStamp();
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            List<Blaze.Tdf> PDAT = new List<Blaze.Tdf>();
            PDAT.Add(Blaze.TdfInteger.Create("EXID", pi.userId));
            PDAT.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            PDAT.Add(Blaze.TdfInteger.Create("LOC\0", pi.loc));
            PDAT.Add(Blaze.TdfString.Create("NAME", pi.profile.name));
            PDAT.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
            PDAT.Add(BlazeHelper.CreateNETFieldUnion(pi, "PNET"));
            PDAT.Add(Blaze.TdfInteger.Create("SID\0", pi.slot));
            PDAT.Add(Blaze.TdfInteger.Create("STAT", pi.stat));
            PDAT.Add(Blaze.TdfInteger.Create("TIDX", 0xFFFF));
            PDAT.Add(Blaze.TdfInteger.Create("TIME", t));
            PDAT.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            Result.Add(Blaze.TdfStruct.Create("PDAT", PDAT));

            return Result;
        }

    }
}
