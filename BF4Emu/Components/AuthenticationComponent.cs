using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class AuthenticationComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x3C:
                    ExpressLogin(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.userId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [AUTHENTICATIONCOMP] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void ExpressLogin(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            uint t = Blaze.GetUnixTimeStamp();

            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("AGUP", 0)); // Can Age Up
            Result.Add(Blaze.TdfInteger.Create("ANON", 0)); // Is Anonymous
            Result.Add(Blaze.TdfInteger.Create("NTOS", 0)); // Needs Legal Docs (TOS)
            Result.Add(Blaze.TdfString.Create("PCTK", "PlayerTicket_1337")); // PCLogin Token

            List<Blaze.Tdf> SESS = new List<Blaze.Tdf>();

            SESS.Add(Blaze.TdfInteger.Create("BUID", pi.userId)); //BlazeUserID
            SESS.Add(Blaze.TdfString.Create("KEY", "SessionKey_1337"));
            SESS.Add(Blaze.TdfString.Create("MAIL", "bf4.server.pc@ea.com"));
            SESS.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            SESS.Add(Blaze.TdfInteger.Create("FRSC", 0));
            SESS.Add(Blaze.TdfInteger.Create("FRST", 0));
            SESS.Add(Blaze.TdfInteger.Create("LLOG", 1403663841));

            Result.Add(Blaze.TdfStruct.Create("SESS", SESS));

            List<Blaze.Tdf> PDTL = new List<Blaze.Tdf>();
            PDTL.Add(Blaze.TdfString.Create("DSNM", pi.profile.name));
            PDTL.Add(Blaze.TdfInteger.Create("LAST", t));
            PDTL.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
            PDTL.Add(Blaze.TdfInteger.Create("PLAT", 4)); //#1 XBL2 #2 PS3 #3 WII #4 PC
            PDTL.Add(Blaze.TdfInteger.Create("STAS", 2));
            PDTL.Add(Blaze.TdfInteger.Create("XREF", 0));
            Result.Add(Blaze.TdfStruct.Create("PDTL", PDTL));
            Result.Add(Blaze.TdfInteger.Create("SPAM", 0));
            Result.Add(Blaze.TdfInteger.Create("UNDR", 0));

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            Logger.LogPacket("ExpressLog", Convert.ToInt32(pi.userId), buff); //TestLog
            ns.Write(buff, 0, buff.Length);

            // Send UserAuthenticated Packet
            List<Blaze.Tdf> Result2 = UserAuthenticatedCommand.UserAuthenticated(pi);
            byte[] buff2 = Blaze.CreatePacket(0x7802, 8, 0, 0x2000, p.ID, Result2);
            Logger.LogPacket("UserAuthenticated", Convert.ToInt32(pi.userId), buff2); //TestLog
            ns.Write(buff2, 0, buff2.Length);

            //Send UserUpdated Packet
            List<Blaze.Tdf> Result3 = UserUpdatedCommand.UserUpdated(pi);
            byte[] buff3 = Blaze.CreatePacket(0x7802, 5, 0, 0x2000, p.ID, Result3);
            Logger.LogPacket("UserUpdated", Convert.ToInt32(pi.userId), buff3); //TestLog
            ns.Write(buff3, 0, buff3.Length);

            //Send UserAdded Packet
            List<Blaze.Tdf> Result4 = UserAddedCommand.UserAdded(pi);
            byte[] buff4 = Blaze.CreatePacket(0x7802, 2, 0, 0x2000, p.ID, Result4);
            Logger.LogPacket("UserAdded", Convert.ToInt32(pi.userId), buff4); //TestLog
            ns.Write(buff4, 0, buff4.Length);



            ns.Flush();
        }
    }
}
