using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;


namespace BF4Emu
{
    class NotifyClientGameSetupCommand
    {
        public static List<Blaze.Tdf> NotifyClientGameSetup(PlayerInfo pi, PlayerInfo srv)
        {
            long reas = 1;
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> GAME = new List<Blaze.Tdf>();
            GAME.Add(Blaze.TdfList.Create("ADMN", 0, 1, new List<long>(new long[] { srv.userId })));
            if (srv.game.ATTR != null)
                GAME.Add(srv.game.ATTR);
            GAME.Add(Blaze.TdfList.Create("CAP\0", 0, 2, new List<long>(new long[] { 0x20, 0 })));
            GAME.Add(Blaze.TdfInteger.Create("GID\0", srv.game.id));
            GAME.Add(Blaze.TdfString.Create("GNAM", srv.game.GNAM));
            GAME.Add(Blaze.TdfInteger.Create("GPVH", 666));
            GAME.Add(Blaze.TdfInteger.Create("GSET", srv.game.GSET));
            GAME.Add(Blaze.TdfInteger.Create("GSID", 1));
            GAME.Add(Blaze.TdfInteger.Create("GSTA", srv.game.GSTA));
            GAME.Add(Blaze.TdfString.Create("GTYP", "AssaultStandard"));
            GAME.Add(BlazeHelper.CreateNETField(srv, "HNET"));
            GAME.Add(Blaze.TdfInteger.Create("HSES", 13666));
            GAME.Add(Blaze.TdfInteger.Create("IGNO", 0));
            GAME.Add(Blaze.TdfInteger.Create("MCAP", 0x20));
            GAME.Add(BlazeHelper.CreateNQOSField(srv, "NQOS"));
            GAME.Add(Blaze.TdfInteger.Create("NRES", 0));
            GAME.Add(Blaze.TdfInteger.Create("NTOP", 1));
            GAME.Add(Blaze.TdfString.Create("PGID", ""));
            List<Blaze.Tdf> PHST = new List<Blaze.Tdf>();
            PHST.Add(Blaze.TdfInteger.Create("HPID", srv.userId));
            PHST.Add(Blaze.TdfInteger.Create("HSLT", srv.slot));
            GAME.Add(Blaze.TdfStruct.Create("PHST", PHST));
            GAME.Add(Blaze.TdfInteger.Create("PRES", 1));
            GAME.Add(Blaze.TdfString.Create("PSAS", "wv"));
            GAME.Add(Blaze.TdfInteger.Create("QCAP", 0x10));
            GAME.Add(Blaze.TdfInteger.Create("SEED", 0x2CF2048F));
            GAME.Add(Blaze.TdfInteger.Create("TCAP", 0x10));
            List<Blaze.Tdf> THST = new List<Blaze.Tdf>();
            THST.Add(Blaze.TdfInteger.Create("HPID", srv.userId));
            THST.Add(Blaze.TdfInteger.Create("HSLT", srv.slot));
            GAME.Add(Blaze.TdfStruct.Create("THST", THST));
            List<long> playerIdList = new List<long>();
            for (int i = 0; i < 32; i++)
                if (srv.game.slotUse[i] != -1)
                    playerIdList.Add(srv.game.slotUse[i]);
            GAME.Add(Blaze.TdfList.Create("TIDS", 0, 2, playerIdList));
            GAME.Add(Blaze.TdfString.Create("UUID", "f5193367-c991-4429-aee4-8d5f3adab938"));
            GAME.Add(Blaze.TdfInteger.Create("VOIP", srv.game.VOIP));
            GAME.Add(Blaze.TdfString.Create("VSTR", srv.game.VSTR));
            Result.Add(Blaze.TdfStruct.Create("GAME", GAME));
            List<Blaze.TdfStruct> PROS = new List<Blaze.TdfStruct>();
            for (int i = 0; i < 32; i++)
                if (srv.game.players[i] != null)
                    PROS.Add(BlazeHelper.MakePROSEntry(i, srv.game.players[i]));
            Result.Add(Blaze.TdfList.Create("PROS", 3, PROS.Count, PROS));
            List<Blaze.Tdf> VALU = new List<Blaze.Tdf>();
            VALU.Add(Blaze.TdfInteger.Create("DCTX", reas));
            Result.Add(Blaze.TdfUnion.Create("REAS", 0, Blaze.TdfStruct.Create("VALU", VALU)));


            return Result;
        }
    }
}
