using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace CNCEmu
{
    public static class AuthenticationComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x28:
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


            // Overall Result
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();

            // Session is collection of data related to user session
            List<Blaze.Tdf> SESS = new List<Blaze.Tdf>();

            // Player Details contains information specific to player
            List<Blaze.Tdf> PDTL = new List<Blaze.Tdf>();

            //Known Packets from other Solutions
            //Result.Add(Blaze.TdfInteger.Create("AGUP", 0)); // Can Age Up
            //Result.Add(Blaze.TdfInteger.Create("ANON", 0)); // Is Anonymous
            //Result.Add(Blaze.TdfInteger.Create("NTOS", 0)); // Needs Legal Docs (TOS)
            //Result.Add(Blaze.TdfString.Create("PCTK", "PlayerTicket_1337")); // PCLogin Token

            //List<Blaze.Tdf> SESS = new List<Blaze.Tdf>();
            //SESS.Add(Blaze.TdfInteger.Create("BUID", pi.userId)); //BlazeUserID
            //SESS.Add(Blaze.TdfString.Create("KEY", "SessionKey_1337"));
            //SESS.Add(Blaze.TdfString.Create("SKEY", "SessionKey_1337"));
            //SESS.Add(Blaze.TdfString.Create("PRIV", ""));
            //SESS.Add(Blaze.TdfString.Create("MAIL", "bf4.server.pc@ea.com"));
            //SESS.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            //SESS.Add(Blaze.TdfInteger.Create("FRSC", 0));
            //SESS.Add(Blaze.TdfInteger.Create("FRST", 0));
            //SESS.Add(Blaze.TdfInteger.Create("LLOG", 1403663841));

            //// Add the new TdfString and TdfInteger fields from BFP4FBlazeServer
            //SESS.Add(Blaze.TdfString.Create("THST", ""));
            //SESS.Add(Blaze.TdfString.Create("LDHT", ""));
            //SESS.Add(Blaze.TdfString.Create("TSUI", ""));
            //SESS.Add(Blaze.TdfString.Create("TURI", ""));
            //SESS.Add(Blaze.TdfInteger.Create("XTYP", 0));

            //Result.Add(Blaze.TdfStruct.Create("SESS", SESS));

            //List<Blaze.Tdf> PDTL = new List<Blaze.Tdf>();
            //PDTL.Add(Blaze.TdfString.Create("DSNM", pi.profile.name));
            //PDTL.Add(Blaze.TdfInteger.Create("LAST", t));
            //PDTL.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
            //PDTL.Add(Blaze.TdfInteger.Create("PLAT", 4)); //#1 XBL2 #2 PS3 #3 WII #4 PC
            //PDTL.Add(Blaze.TdfInteger.Create("STAS", 2));
            //PDTL.Add(Blaze.TdfInteger.Create("XREF", 0));
            //Result.Add(Blaze.TdfStruct.Create("PDTL", PDTL));
            //Result.Add(Blaze.TdfInteger.Create("SPAM", 0));
            //Result.Add(Blaze.TdfInteger.Create("UNDR", 0));

            //List<Blaze.TdfStruct> playerEntries = new List<Blaze.TdfStruct>();
            //List<Blaze.Tdf> playerEntry = new List<Blaze.Tdf>();
            //playerEntries.Add(Blaze.TdfStruct.Create("0", playerEntry));
            //Result.Add(Blaze.TdfList.Create("PLST", 3, 1, playerEntries));

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

//Working example to Loginpersona 
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BlazeLibWV;
//using System.Net.Sockets;
//using CNCEmu;

//namespace CNCEmu
//{
//    public static class AuthenticationComponent
//    {

//        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
//        {
//            switch (p.Command)
//            {
//                case 0x28:
//                    Login(p, pi, ns);
//                    break;
//                case 0x64:
//                    ListPersonas(p, pi, ns);
//                    break;
//                case 0x6E:
//                    LoginPersona(p, pi, ns);
//                    break;
//                case 0x78:
//                    LogoutPersona(p, pi, ns);
//                    break;
//            }
//        }

//        public static void Login(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
//        {
//            if (!pi.isServer)
//            {
//                List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);

//                // Check if the third element is a TdfString before casting
//                Blaze.TdfString TOKN = input.OfType<Blaze.TdfString>().FirstOrDefault();

//                if (TOKN != null)
//                {
//                    long id;

//                    // Attempt to convert the token value to a long
//                    if (long.TryParse(TOKN.Value, out id))
//                    {
//                        // Search for the profile with the matching id
//                        foreach (Profile profile in Profiles.profiles)
//                        {
//                            if (profile.id == id)
//                            {
//                                pi.profile = profile;
//                                break;
//                            }
//                        }

//                        if (pi.profile == null)
//                        {
//                            Logger.Log("[CLNT] #" + pi.userId + " Could not find player profile for token 0x" + id.ToString("X") + "!");
//                            pi.userId = 0;
//                            return;
//                        }
//                        else
//                        {
//                            for (int i = 0; i < BlazeServer.allClients.Count; i++)
//                            {
//                                if (BlazeServer.allClients[i].userId == id)
//                                {
//                                    BlazeServer.allClients.RemoveAt(i);
//                                    i--;
//                                }
//                            }

//                            pi.userId = id;
//                            Logger.Log("[CLNT] New ID #" + pi.userId + " Client Playername = \"" + pi.profile.name + "\"");
//                        }
//                    }
//                    else
//                    {
//                        // Log a warning with the token value if conversion fails
//                        Logger.Log("[CLNT] #" + pi.userId + " Warning: Unable to convert token value to a valid ID. Token value: " + TOKN.Value);

//                        // Assign the token value as the username for further handling
//                        pi.profile = new Profile { name = TOKN.Value };
//                        pi.userId = -1; // Use a placeholder value or handle as needed
//                    }
//                }
//                else
//                {
//                    // Handle the case where a TdfString is not found
//                    Logger.Log("[CLNT] #" + pi.userId + " Unexpected format for token in login packet.");
//                    pi.userId = 0;
//                    return;
//                }
//            }


//            uint t = Blaze.GetUnixTimeStamp();
//            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
//            Result.Add(Blaze.TdfString.Create("LDHT", ""));
//            Result.Add(Blaze.TdfInteger.Create("NTOS", 0));
//            Result.Add(Blaze.TdfString.Create("PCTK", ""));
//            List<Blaze.TdfStruct> playerentries = new List<Blaze.TdfStruct>();
//            List<Blaze.Tdf> PlayerEntry = new List<Blaze.Tdf>();
//            PlayerEntry.Add(Blaze.TdfString.Create("DSNM", pi.profile.name));
//            PlayerEntry.Add(Blaze.TdfInteger.Create("LAST", t));
//            PlayerEntry.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
//            PlayerEntry.Add(Blaze.TdfInteger.Create("STAS", 2));
//            PlayerEntry.Add(Blaze.TdfInteger.Create("XREF", 0));
//            PlayerEntry.Add(Blaze.TdfInteger.Create("XTYP", 0));
//            playerentries.Add(Blaze.TdfStruct.Create("0", PlayerEntry));
//            Result.Add(Blaze.TdfList.Create("PLST", 3, 1, playerentries));
//            Result.Add(Blaze.TdfString.Create("PRIV", ""));
//            Result.Add(Blaze.TdfString.Create("SKEY", "some_client_key"));
//            Result.Add(Blaze.TdfInteger.Create("SPAM", 1));
//            Result.Add(Blaze.TdfString.Create("THST", ""));
//            Result.Add(Blaze.TdfString.Create("TSUI", ""));
//            Result.Add(Blaze.TdfString.Create("TURI", ""));
//            Result.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
//            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
//            ns.Write(buff, 0, buff.Length);
//            ns.Flush();
//        }

//        public static void LoginPersona(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
//        {
//            uint t = Blaze.GetUnixTimeStamp();
//            List<Blaze.Tdf> SESS = new List<Blaze.Tdf>();
//            SESS.Add(Blaze.TdfInteger.Create("BUID", pi.userId));
//            SESS.Add(Blaze.TdfInteger.Create("FRST", 0));
//            SESS.Add(Blaze.TdfString.Create("KEY\0", "some_client_key"));
//            SESS.Add(Blaze.TdfInteger.Create("LLOG", t));
//            SESS.Add(Blaze.TdfString.Create("MAIL", ""));
//            List<Blaze.Tdf> PDTL = new List<Blaze.Tdf>();
//            PDTL.Add(Blaze.TdfString.Create("DSNM", pi.profile.name));
//            PDTL.Add(Blaze.TdfInteger.Create("LAST", t));
//            PDTL.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
//            PDTL.Add(Blaze.TdfInteger.Create("STAS", 0));
//            PDTL.Add(Blaze.TdfInteger.Create("XREF", 0));
//            PDTL.Add(Blaze.TdfInteger.Create("XTYP", 0));
//            SESS.Add(Blaze.TdfStruct.Create("PDTL", PDTL));
//            SESS.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
//            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, SESS);
//            ns.Write(buff, 0, buff.Length);
//            ns.Flush();

//            AsyncUserSessions.NotifyUserAdded(pi, p, pi, ns);
//            AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);
//        }

//        public static void LogoutPersona(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
//        {
//            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
//            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
//            ns.Write(buff, 0, buff.Length);
//            ns.Flush();

//            AsyncUserSessions.NotifyUserRemoved(pi, p, pi.userId, ns);
//            AsyncUserSessions.NotifyUserStatus(pi, p, pi, ns);
//        }

//        public static void ListPersonas(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
//        {
//            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
//            List<Blaze.TdfStruct> entries = new List<Blaze.TdfStruct>();
//            List<Blaze.Tdf> e = new List<Blaze.Tdf>();
//            e.Add(Blaze.TdfString.Create("DSNM", pi.profile.name));
//            e.Add(Blaze.TdfInteger.Create("LAST", Blaze.GetUnixTimeStamp()));
//            e.Add(Blaze.TdfInteger.Create("PID\0", pi.profile.id));
//            e.Add(Blaze.TdfInteger.Create("STAS", 2));
//            e.Add(Blaze.TdfInteger.Create("XREF", 0));
//            e.Add(Blaze.TdfInteger.Create("XTYP", 0));
//            entries.Add(Blaze.TdfStruct.Create("0", e));
//            result.Add(Blaze.TdfList.Create("PINF", 3, 1, entries));
//            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, result);
//            ns.Write(buff, 0, buff.Length);
//            ns.Flush();
//        }
//    }
//}