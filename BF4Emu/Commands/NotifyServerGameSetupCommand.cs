using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class NotifyServerGameSetupCommand
    {
        public static List<Blaze.Tdf> NotifyServerGameSetup(Blaze.Packet p, PlayerInfo pi)
        {
            List<Blaze.Tdf> input = Blaze.ReadPacketContent(p);

            uint t = Blaze.GetUnixTimeStamp();
            pi.game.GNAM = ((Blaze.TdfString)input[5]).Value;
            pi.game.GSET = ((Blaze.TdfInteger)input[6]).Value;
            pi.game.VOIP = ((Blaze.TdfInteger)input[24]).Value;
            pi.game.VSTR = ((Blaze.TdfString)input[25]).Value;
            List<Blaze.Tdf> result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> GAME = new List<Blaze.Tdf>();
            GAME.Add(Blaze.TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { pi.userId })));
            GAME.Add(Blaze.TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            GAME.Add(Blaze.TdfString.Create("GNAM", pi.game.GNAM));
            GAME.Add(Blaze.TdfInteger.Create("GPVH", 666));
            GAME.Add(Blaze.TdfInteger.Create("GSET", pi.game.GSET));
            GAME.Add(Blaze.TdfInteger.Create("GSID", 1));
            GAME.Add(Blaze.TdfInteger.Create("GSTA", pi.game.GSTA));
            GAME.Add(Blaze.TdfString.Create("GTYP", "AssaultStandard"));
            GAME.Add(BlazeHelper.CreateNETField(pi, "HNET"));
            GAME.Add(Blaze.TdfInteger.Create("HSES", 13666));
            GAME.Add(Blaze.TdfInteger.Create("IGNO", 0));
            GAME.Add(Blaze.TdfInteger.Create("MCAP", 0x20));
            GAME.Add(BlazeHelper.CreateNQOSField(pi, "NQOS"));
            GAME.Add(Blaze.TdfInteger.Create("NRES", 0));
            GAME.Add(Blaze.TdfInteger.Create("NTOP", 1));
            GAME.Add(Blaze.TdfString.Create("PGID", ""));
            List<Blaze.Tdf> PHST = new List<Blaze.Tdf>();
            PHST.Add(Blaze.TdfInteger.Create("HPID", pi.userId));
            GAME.Add(Blaze.TdfStruct.Create("PHST", PHST));
            GAME.Add(Blaze.TdfInteger.Create("PRES", 1));
            GAME.Add(Blaze.TdfString.Create("PSAS", "wv"));
            GAME.Add(Blaze.TdfInteger.Create("QCAP", 0x10));
            GAME.Add(Blaze.TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(Blaze.TdfInteger.Create("TCAP", 0x10));
            List<Blaze.Tdf> THST = new List<Blaze.Tdf>();
            THST.Add(Blaze.TdfInteger.Create("HPID", pi.userId));
            GAME.Add(Blaze.TdfStruct.Create("THST", THST));
            GAME.Add(Blaze.TdfList.Create("TIDS", 0, 2, new List<long>(new long[] { 1, 2 })));
            GAME.Add(Blaze.TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(Blaze.TdfInteger.Create("VOIP", pi.game.VOIP));
            GAME.Add(Blaze.TdfString.Create("VSTR", pi.game.VSTR));
            result.Add(Blaze.TdfStruct.Create("GAME", GAME));
            List<Blaze.TdfStruct> PROS = new List<Blaze.TdfStruct>();
            List<Blaze.Tdf> ee0 = new List<Blaze.Tdf>();
            ee0.Add(Blaze.TdfInteger.Create("EXID", pi.userId));
            ee0.Add(Blaze.TdfInteger.Create("GID\0", pi.game.id));
            ee0.Add(Blaze.TdfInteger.Create("LOC\0", pi.loc));
            ee0.Add(Blaze.TdfString.Create("NAME", pi.profile.name));
            ee0.Add(Blaze.TdfInteger.Create("PID\0", pi.userId));
            ee0.Add(BlazeHelper.CreateNETFieldUnion(pi, "PNET"));
            ee0.Add(Blaze.TdfInteger.Create("SID\0", pi.slot));
            ee0.Add(Blaze.TdfInteger.Create("SLOT", 0));
            ee0.Add(Blaze.TdfInteger.Create("STAT", 2));
            ee0.Add(Blaze.TdfInteger.Create("TIDX", 0xFFFF));
            ee0.Add(Blaze.TdfInteger.Create("TIME", t));
            ee0.Add(Blaze.TdfInteger.Create("UID\0", pi.userId));
            PROS.Add(Blaze.TdfStruct.Create("0", ee0));
            result.Add(Blaze.TdfList.Create("PROS", 3, 1, PROS));
            List<Blaze.Tdf> VALU = new List<Blaze.Tdf>();
            VALU.Add(Blaze.TdfInteger.Create("DCTX", 0));
            result.Add(Blaze.TdfUnion.Create("REAS", 0, Blaze.TdfStruct.Create("VALU", VALU)));

            return result;
        }


    }
}
