﻿using BlazeLibWV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;

namespace CNCEmu
{
    public static class RedirectorServer
    {
        public static readonly object _sync = new object();
        public static bool _exit;
        public static bool _isRunning = false;
        public static bool useSSL = false;
        public static RichTextBox box = null;
        public static TcpListener lRedirector = null;
        public static int targetPort = 3659;
        public static string redi = "redirector.pfx";

        public static void Start()
        {
            SetExit(false);
            _isRunning = true;
            Log("Starting Redirector...");
            new Thread(tRedirectorMain) { IsBackground = true }.Start();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        public static void Stop()
        {
            Log("Backend stopping...");
            if (lRedirector != null) lRedirector.Stop();
            SetExit(true);
            Log("Done.");
        }

        public static void tRedirectorMain(object obj)
        {
            X509Certificate2 cert = null;
            try
            {
                Log("[REDI] Redirector starting...");
                lRedirector = new TcpListener(IPAddress.Parse(ProviderInfo.backendIP), 42127);
                Log("[REDI] Redirector bound to port: 42127");
                lRedirector.Start();
                if (useSSL)
                {
                    Log("[REDI] Loading Cert...");
                    cert = new X509Certificate2(redi, "123456");
                }
                Log("[REDI] Redirector listening...");
                TcpClient client;
                while (!GetExit())
                {
                    client = lRedirector.AcceptTcpClient();
                    Log("[REDI] Client connected");
                    if (useSSL)
                    {
                        SslStream sslStream = new SslStream(client.GetStream(), false);
                        sslStream.AuthenticateAsServer(cert, false, SslProtocols.Default | SslProtocols.None | SslProtocols.Ssl2 | SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, false);
                        byte[] data = Helper.ReadContentSSL(sslStream);
                        MemoryStream m = new MemoryStream();
                        m.Write(data, 0, data.Length);
                        data = CreateRedirectorPacket();
                        m.Write(data, 0, data.Length);
                        sslStream.Write(data);
                        sslStream.Flush();
                        client.Close();
                    }
                    else
                    {
                        NetworkStream stream = client.GetStream();
                        byte[] data = Helper.ReadContentTCP(stream);
                        MemoryStream m = new MemoryStream();
                        m.Write(data, 0, data.Length);
                        data = CreateRedirectorPacket();
                        m.Write(data, 0, data.Length);
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("REDI", ex);
            }
        }

        public static byte[] CreateRedirectorPacket()
        {
            List<Blaze.Tdf> Result = new List<Blaze.Tdf>();
            List<Blaze.Tdf> VALU = new List<Blaze.Tdf>();
            VALU.Add(Blaze.TdfString.Create("HOST", ProviderInfo.serverIP));
            VALU.Add(Blaze.TdfInteger.Create("IP\0\0", Blaze.GetIPfromString(ProviderInfo.serverIP)));
            VALU.Add(Blaze.TdfInteger.Create("PORT", targetPort));
            Blaze.TdfUnion ADDR = Blaze.TdfUnion.Create("ADDR", 0, Blaze.TdfStruct.Create("VALU", VALU));
            Result.Add(ADDR);
            Result.Add(Blaze.TdfInteger.Create("SECU", 0)); //Change to 1 for SSL 
            Result.Add(Blaze.TdfInteger.Create("XDNS", 0));
            return Blaze.CreatePacket(5, 1, 0, 0x1000, 0, Result);
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

        public static void Log(string s)
        {
            if (box == null) return;
            try
            {
                box.Invoke(new Action(delegate
                {
                    string stamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " : ";
                    box.AppendText(stamp + s + "\n");
                    BackendLog.Write(stamp + s + "\n");
                    box.SelectionStart = box.Text.Length;
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
