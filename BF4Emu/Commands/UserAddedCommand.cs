using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class UserAddedCommand
    {
        public static List<Blaze.Tdf> UserAdded(PlayerInfo pi)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> DATA = new List<Blaze.Tdf>();
            List<Blaze.Tdf> USER = new List<Blaze.Tdf>();
            List<Blaze.Tdf> QDAT = new List<Blaze.Tdf>();
            DATA.Add(Blaze.TdfString.Create("BPS", "ams")); //Best PingSite
            DATA.Add(Blaze.TdfString.Create("CTY", "")); //Country
            DATA.Add(Blaze.TdfInteger.Create("HWFG", 0)); //Hardware Flags
            List<string> t = Helper.ConvertStringList("{354} {376} {241} {177} {206} {37}");
            List<long> t2 = new List<long>();
            foreach (string v in t)
                t2.Add(Convert.ToInt64(v));
            DATA.Add(Blaze.TdfList.Create("PSLM", 0, t2.Count, t2)); //PingSite list # in ms
            DATA.Add(Blaze.TdfStruct.Create("QDAT", QDAT)); //Quality of Service Data
            DATA.Add(Blaze.TdfInteger.Create("UATT", 0)); //UserInfoAttribute
            Result.Add(Blaze.TdfStruct.Create("DATA", DATA));

            USER.Add(Blaze.TdfInteger.Create("AID", pi.userId));
            USER.Add(Blaze.TdfInteger.Create("ALOC", 1701729619));
            USER.Add(Blaze.TdfInteger.Create("ID", pi.userId));
            USER.Add(Blaze.TdfString.Create("NAME", pi.profile.name));
            USER.Add(Blaze.TdfInteger.Create("ORIG", pi.userId));
            USER.Add(Blaze.TdfInteger.Create("PIDI", 0));
            Result.Add(Blaze.TdfStruct.Create("USER", USER));

            return Result;
        }
    }
}
