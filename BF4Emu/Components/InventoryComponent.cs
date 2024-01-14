using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class InventoryComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x00:
                    break;
                case 0x01:
                    GetItems(p, pi, ns);
                    break;
                case 0x06:
                    GetTemplate(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.userId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [INVENTORY] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void GetItems(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, new List<Blaze.Tdf>());
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }

        public static void GetTemplate(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<string> t = Helper.ConvertStringList("{aek971_acog} {aek971_eotech}"); //Add Items... Not finish... !!
            Result.Add(Blaze.TdfList.Create("ILST", 1, t.Count, t));
            byte[] buff = Blaze.CreatePacket(p.Component, p.Command, 0, 0x1000, p.ID, Result);
            ns.Write(buff, 0, buff.Length);
            ns.Flush();
        }


    }
}
