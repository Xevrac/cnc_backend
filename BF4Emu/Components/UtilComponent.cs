using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class UtilComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x02:
                    Ping(p, pi, ns);
                    break;
                case 0x05:
                    getTelemetryServer(p, pi,ns);
                    break;
                case 0x07:
                    PreAuth(p, pi, ns);
                    break;
                case 0x08:
                    PostAuth(p,pi,ns);
                    break;
                case 0x16:
                    SetClientMetrics(p, pi,ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.userId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [UTIL] " +  " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void Ping(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            pi.timeout.Restart();
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfInteger.Create("STIM", Blaze.GetUnixTimeStamp()));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void getTelemetryServer(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> TELE = new List<Blaze.Tdf>();

            TELE.Add(Blaze.TdfString.Create("ADRS", "http://river.data.ea.com"));
            TELE.Add(Blaze.TdfInteger.Create("ANON", 0));
            TELE.Add(Blaze.TdfString.Create("DISA", "AD,AF,AG,AI,AL,AM,AN,AO,AQ,AR,AS,AW,AX,AZ,BA,BB,BD,BF,BH,BI,BJ,BM,BN,BO,BR,BS,BT,BV,BW,BY,BZ,CC,CD,CF,CG,CI,CK,CL,CM,CN,CO,CR,CU,CV,CX,DJ,DM,DO,DZ,EC,EG,EH,ER,ET,FJ,FK,FM,FO,GA,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GS,GT,GU,GW,GY,HM,HN,HT,ID,IL,IM,IN,IO,IQ,IR,IS,JE,JM,JO,KE,KG,KH,KI,KM,KN,KP,KR,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LY,MA,MC,MD,ME,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MY,MZ,NA,NC,NE,NF,NG,NI,NP,NR,NU,OM,PA,PE,PF,PG,PH,PK,PM,PN,PS,PW,PY,QA,RE,RS,RW,SA,SB,SC,SD,SG,SH,SJ,SL,SM,SN,SO,SR,ST,SV,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TT,TV,TZ,UA,UG,UM,UY,UZ,VA,VC,VE,VG,VN,VU,WF,WS,YE,YT,ZM,ZW,ZZ"));
            TELE.Add(Blaze.TdfInteger.Create("EDCT", 1));
            TELE.Add(Blaze.TdfString.Create("FILT", "-GAME/COMM/EXPD"));
            TELE.Add(Blaze.TdfInteger.Create("LOC", 1701729619));
            TELE.Add(Blaze.TdfInteger.Create("MINR", 0));
            TELE.Add(Blaze.TdfString.Create("NOOK", "US, CA, MX"));
            TELE.Add(Blaze.TdfInteger.Create("PORT", 0x1BB));
            TELE.Add(Blaze.TdfInteger.Create("SDLY", 15000));
            TELE.Add(Blaze.TdfString.Create("SESS", "session_key_telemetry"));
            TELE.Add(Blaze.TdfString.Create("SKEY", ""));
            TELE.Add(Blaze.TdfInteger.Create("SPCT", 0));
            TELE.Add(Blaze.TdfString.Create("STIM", "Default"));
            TELE.Add(Blaze.TdfString.Create("SVNM", "telemetry - 3 - common"));

            Result.Add(Blaze.TdfStruct.Create("TELE", TELE));

            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void PreAuth(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            uint utime = Blaze.GetUnixTimeStamp();

            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);
            Blaze.TdfStruct CDAT = (Blaze.TdfStruct)input[0];
            Blaze.TdfInteger TYPE = (Blaze.TdfInteger)CDAT.Values[3];
            pi.isServer = TYPE.Value != 0;

            if (pi.isServer)  //Make as a Server !
            {
                pi.game = new GameInfo();
                pi.profile = Profiles.Create("bf4.server.pc@ea.com", 999);
                pi.userId = 999;
            }
            Blaze.TdfStruct CINF = (Blaze.TdfStruct)input[1];
            Blaze.TdfString CVER = (Blaze.TdfString)CINF.Values[5];
            Blaze.TdfInteger LOC = (Blaze.TdfInteger)CINF.Values[8];
            pi.loc = LOC.Value;
            pi.version = CVER.Value;
            BlazeServer.Log("[CLNT] #" + pi.userId + " is a " + (pi.isServer ? "server" : "client"), System.Drawing.Color.Blue);

            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            Result.Add(Blaze.TdfString.Create("ASRC", "302123")); //Authentication Source 300294
            List<string> t = Helper.ConvertStringList("{30728} {1} {30729} {25} {30730} {27} {4} {28} {6} {7} {9} {10} {63490} {35} {15} {30720} {30722} {30723} {30724} {30726} {2000} {30727}"); // Component ID's
            List<long> t2 = new List<long>();
            foreach (string v in t)
                t2.Add(Convert.ToInt64(v));
            Result.Add(Blaze.TdfList.Create("CIDS", 0, t2.Count, t2));
            t = new List<string>();
            List<string> t3 = new List<string>();
            Helper.ConvertDoubleStringList("{associationListSkipInitialSet ; 1} {blazeServerClientId ; GOS-BlazeServer-BF4-PC} {bytevaultHostname ; bytevault.gameservices.ea.com} {bytevaultPort ; 42210} {bytevaultSecure ; true} {capsStringValidationUri ; client-strings.xboxlive.com} {connIdleTimeout ; 90s} {defaultRequestTimeout ; 60s} {identityDisplayUri ; console2/welcome} {identityRedirectUri ; http://127.0.0.1/success} {nucleusConnect ; https://accounts.ea.com} {nucleusProxy ; https://gateway.ea.com} {pingPeriod ; 30s} {userManagerMaxCachedUsers ; 0} {voipHeadsetUpdateRate ; 1000} {xblTokenUrn ; accounts.ea.com} {xlspConnectionIdleTimeout ; 300}", out t, out t3);
            Blaze.TdfDoubleList conf2 = Blaze.TdfDoubleList.Create("CONF", 1, 1, t, t3, t.Count);
            List<Blaze.Tdf> t4 = new List<Blaze.Tdf>();
            t4.Add(conf2);
            Result.Add(Blaze.TdfStruct.Create("CONF", t4));
            Result.Add(Blaze.TdfString.Create("ESRC", "302123"));
            Result.Add(Blaze.TdfString.Create("INST", "battlefield-4-pc"));
            Result.Add(Blaze.TdfInteger.Create("MINR", 0));
            Result.Add(Blaze.TdfString.Create("NASP", "cem_ea_id"));
            Result.Add(Blaze.TdfString.Create("PILD", ""));
            Result.Add(Blaze.TdfString.Create("PLAT", "pc"));

            List<Blaze.Tdf> QOSS = new List<Blaze.Tdf>();
            List<Blaze.Tdf> BWPS = new List<Blaze.Tdf>();
            BWPS.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.ams_psa));
            BWPS.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.ams_psp));
            BWPS.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.ams_sna));
            QOSS.Add(Blaze.TdfStruct.Create("BWPS", BWPS));
            QOSS.Add(Blaze.TdfInteger.Create("LNP\0", 0xA));

            List<Blaze.TdfStruct> LTPS = new List<Blaze.TdfStruct>();

            List<Blaze.Tdf> LTPS1 = new List<Blaze.Tdf>();
            LTPS1.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.ams_psa));
            LTPS1.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.ams_psp));
            LTPS1.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.ams_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS1));

            List<Blaze.Tdf> LTPS2 = new List<Blaze.Tdf>();
            LTPS2.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.gru_psa));
            LTPS2.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.gru_psp));
            LTPS2.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.gru_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS2));

            List<Blaze.Tdf> LTPS3 = new List<Blaze.Tdf>();
            LTPS3.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.iad_psa));
            LTPS3.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.iad_psp));
            LTPS3.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.iad_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS3));

            List<Blaze.Tdf> LTPS4 = new List<Blaze.Tdf>();
            LTPS4.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.lax_psa));
            LTPS4.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.lax_psp));
            LTPS4.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.lax_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS4));

            List<Blaze.Tdf> LTPS5 = new List<Blaze.Tdf>();
            LTPS5.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.nrt_psa));
            LTPS5.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.nrt_psp));
            LTPS5.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.nrt_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS5));

            List<Blaze.Tdf> LTPS6 = new List<Blaze.Tdf>();
            LTPS6.Add(Blaze.TdfString.Create("PSA\0", ProviderInfo.syd_psa));
            LTPS6.Add(Blaze.TdfInteger.Create("PSP\0", ProviderInfo.syd_psp));
            LTPS6.Add(Blaze.TdfString.Create("SNA\0", ProviderInfo.syd_sna));
            LTPS.Add(Blaze.CreateStructStub(LTPS6));

            t = Helper.ConvertStringList("{" + ProviderInfo.ams + "}" + "{" + ProviderInfo.gru + "}" + "{" + ProviderInfo.iad + "}" + "{" + ProviderInfo.lax + "}" + "{" + ProviderInfo.nrt + "}" + "{" + ProviderInfo.syd + "}");
            QOSS.Add(Blaze.TdfDoubleList.Create("LTPS", 1, 3, t, LTPS, LTPS.Count));

            QOSS.Add(Blaze.TdfInteger.Create("SVID", 0x45410805)); // ServerID
            QOSS.Add(Blaze.TdfInteger.Create("TIME", utime));

            Result.Add(Blaze.TdfStruct.Create("QOSS", QOSS));
            Result.Add(Blaze.TdfString.Create("RSRC", "302123")); 
            Result.Add(Blaze.TdfString.Create("SVER", "Blaze 13.3.1.8.0 (CL# 1148269)")); // Blaze Server Version 13.15.08.0 (CL# 9442625)
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);

            Logger.LogPacket("PreAuth", Convert.ToInt32(pi.userId), buff); //Test
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void PostAuth(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> PSSList = new List<Blaze.Tdf>();
            PSSList.Add(Blaze.TdfString.Create("ADRS", "127.0.0.1")); //playersyncservice.ea.com
            PSSList.Add(Blaze.TdfString.Create("PJID", "123071"));
            PSSList.Add(Blaze.TdfInteger.Create("PORT", 80));
            PSSList.Add(Blaze.TdfInteger.Create("RPRT", 9));
            Result.Add(Blaze.TdfStruct.Create("PSS\0", PSSList));
            List<Blaze.Tdf> TELEList = new List<Blaze.Tdf>();
            TELEList.Add(Blaze.TdfString.Create("ADRS", "127.0.0.1")); //river.data.ea.com
            TELEList.Add(Blaze.TdfInteger.Create("ANON", 0));
            TELEList.Add(Blaze.TdfString.Create("DISA", "AD,AF,AG,AI,AL,AM,AN,AO,AQ,AR,AS,AW,AX,AZ,BA,BB,BD,BF,BH,BI,BJ,BM,BN,BO,BR,BS,BT,BV,BW,BY,BZ,CC,CD,CF,CG,CI,CK,CL,CM,CN,CO,CR,CU,CV,CX,DJ,DM,DO,DZ,EC,EG,EH,ER,ET,FJ,FK,FM,FO,GA,GD,GE,GF,GG,GH,GI,GL,GM,GN,GP,GQ,GS,GT,GU,GW,GY,HM,HN,HT,ID,IL,IM,IN,IO,IQ,IR,IS,JE,JM,JO,KE,KG,KH,KI,KM,KN,KP,KR,KW,KY,KZ,LA,LB,LC,LI,LK,LR,LS,LY,MA,MC,MD,ME,MG,MH,ML,MM,MN,MO,MP,MQ,MR,MS,MU,MV,MW,MY,MZ,NA,NC,NE,NF,NG,NI,NP,NR,NU,OM,PA,PE,PF,PG,PH,PK,PM,PN,PS,PW,PY,QA,RE,RS,RW,SA,SB,SC,SD,SG,SH,SJ,SL,SM,SN,SO,SR,ST,SV,SY,SZ,TC,TD,TF,TG,TH,TJ,TK,TL,TM,TN,TO,TT,TV,TZ,UA,UG,UM,UY,UZ,VA,VC,VE,VG,VN,VU,WF,WS,YE,YT,ZM,ZW,ZZ"));
            TELEList.Add(Blaze.TdfString.Create("FILT", "-GAME/COMM/EXPD"));
            TELEList.Add(Blaze.TdfInteger.Create("LOC\0", pi.loc));
            TELEList.Add(Blaze.TdfString.Create("NOOK", "US, CA, MX"));
            TELEList.Add(Blaze.TdfInteger.Create("PORT", 80));
            TELEList.Add(Blaze.TdfInteger.Create("SDLY", 0x3A98));
            TELEList.Add(Blaze.TdfString.Create("SESS", "tele_sess"));
            TELEList.Add(Blaze.TdfString.Create("SKEY", "some_tele_key"));
            TELEList.Add(Blaze.TdfInteger.Create("SPCT", 0x4B));
            TELEList.Add(Blaze.TdfString.Create("STIM", "Default"));
            Result.Add(Blaze.TdfStruct.Create("TELE", TELEList));
            List<Blaze.Tdf> TICKList = new List<Blaze.Tdf>();
            TICKList.Add(Blaze.TdfString.Create("ADRS", "127.0.0.1")); //ticker.ea.com
            TICKList.Add(Blaze.TdfInteger.Create("PORT", 8999));
            TICKList.Add(Blaze.TdfString.Create("SKEY", pi.userId + ",127.0.0.1:80,battlefield-assault-pc,10,50,50,50,50,0,0"));
            Result.Add(Blaze.TdfStruct.Create("TICK", TICKList));
            List<Blaze.Tdf> UROPList = new List<Blaze.Tdf>();
            UROPList.Add(Blaze.TdfInteger.Create("TMOP", 1));
            UROPList.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            Result.Add(Blaze.TdfStruct.Create("UROP", UROPList));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void SetClientMetrics(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

    }
}
