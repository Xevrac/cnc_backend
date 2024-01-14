using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazeLibWV;
using System.Net.Sockets;

namespace BF4Emu
{
    public static class AccountsComponent
    {
        public static void HandlePacket(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {
            switch (p.Command)
            {
                case 0x0:
                    //AuthLogin(p, pi, ns);
                    break;
                default:
                    Logger.Log("[CLNT] #" + pi.userId + " Component: [" + p.Component + "] # Command: " + p.Command + " [at] " + " [ACCOUNTS] " + " not found.", System.Drawing.Color.Red);
                    break;
            }
        }

        public static void AuthLogin(Blaze.Packet p, PlayerInfo pi, NetworkStream ns)
        {

        }

    }
}
