using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlazeLibWV;

namespace BF4Emu
{
    public static class BlazeServer
    {
        public static readonly object _sync = new object();
        public static bool _exit;
        public static bool _isRunning = false;
        public static RichTextBox box = null;
        public static TcpListener lBlaze = null;
        public static int idCounter;
        public static List<PlayerInfo> allClients = new List<PlayerInfo>();

        public static void Start()
        {
            SetExit(false);
            _isRunning = true;
            Log("Starting Blaze...");
            new Thread(tBlazeMain).Start();
            idCounter = 1;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        public static void Stop()
        {
            Log("Backend stopping...");
            if (lBlaze != null) lBlaze.Stop();
            SetExit(true);
            Log("Done.");
        }

        public static void tBlazeMain(object obj)
        {
            try
            {
                Log("[MAIN] Blaze starting...");
                Profiles.Refresh();
                lBlaze = new TcpListener(IPAddress.Parse(ProviderInfo.backendIP), 10071);
                Log("[MAIN] Blaze bound to " + ProviderInfo.backendIP + ":10071");
                lBlaze.Start();
                Log("[MAIN] Blaze listening...");
                TcpClient client;
                while (!GetExit())
                {
                    client = lBlaze.AcceptTcpClient();
                    new Thread(tBlazeClientHandler).Start(client);
                }
            }
            catch (Exception ex)
            {
                LogError("MAIN", ex);
            }
        }

        public static void tBlazeClientHandler(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream ns = client.GetStream();
            PlayerInfo pi = new PlayerInfo();
            allClients.Add(pi);
            pi.userId = idCounter++;
            if (idCounter > 100)
                idCounter = 0;
            pi.exIp = 0;
            pi.ns = ns;
            pi.timeout = new System.Diagnostics.Stopwatch();
            pi.timeout.Start();
            Log("[CLNT] #" + pi.userId + " Handler started");
            try
            {
                while (!GetExit())
                {
                    byte[] data = Helper.ReadContentTCP(ns);
                    if (data != null && data.Length != 0)
                        ProcessPackets(data, pi, ns);
                    Thread.Sleep(1);
                    if (pi.timeout.ElapsedMilliseconds > 5000 * 60)
                        throw new Exception("Client timed out!");
                }
            }
            catch (Exception ex)
            {
                LogError("CLNT", ex, "Handler " + pi.userId);
            }
            client.Close();
            Log("[CLNT] #" + pi.userId + " Client disconnected", System.Drawing.Color.Orange);
            BlazeServer.allClients.Remove(pi);
        }

        public static void ProcessPackets(byte[] data, PlayerInfo pi, NetworkStream ns)
        {
            List<Blaze.Packet> packets = Blaze.FetchAllBlazePackets(new MemoryStream(data));

            if (Config.MakePacket.ToLower() == "true")
            {
                Logger.LogPacket("CLNT", Convert.ToInt32(pi.userId), data);
            }

            foreach (Blaze.Packet p in packets)
            {
                Log("[CLNT] #" + pi.userId + " " + Blaze.PacketToDescriber(p),System.Drawing.Color.Gray);

                switch (p.Component)
                {
                    case 0x1:
                        AuthenticationComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x4:
                        GameManagerComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x7:
                        StatsComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x9:
                        UtilComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x19:
                        AssociationListsComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x23:
                        AccountsComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x801:
                        RSPComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x803:
                        InventoryComponent.HandlePacket(p, pi, ns);
                        break;
                    case 0x7802:
                        UserSessionsComponent.HandlePacket(p, pi, ns);
                        break;
                       default:
                        Log("[CLNT] #" + pi.userId + " Component: " + p.Component  + " not found.", System.Drawing.Color.Red);
                        break;
                }
            }
        }

        public static void SetExit(bool state)
        {
            lock (_sync)
            {
                _exit = state;
            }
        }

        public static bool GetExit()
        {
            bool result;
            lock (_sync)
            {
                result = _exit;
            }
            return result;
        }

        public static void Log(string s, object color = null)
        {
            if (box == null) return;
            try
            {
                box.Invoke(new Action(delegate
                {
                    string stamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " : ";
                    Color c;
                    if (color != null)
                        c = (Color)color;
                    else
                        c = Color.Black;
                    box.SelectionStart = box.TextLength;
                    box.SelectionLength = 0;
                    box.SelectionColor = c;
                    box.AppendText(stamp + s + "\n");
                    BackendLog.Write(stamp + s + "\n");
                    box.SelectionColor = box.ForeColor;
                    box.ScrollToCaret();
                }));
            }
            catch { }
        }

        public static void LogError(string who, Exception e, string cName = "")
        {
            string result = "";
            if (who != "") result = "[" + who + "] " + cName + " ERROR: ";
            result += e.Message;
            if (e.InnerException != null)
                result += " - " + e.InnerException.Message;
            Log(result);
        }
    }
}
