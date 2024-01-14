using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace BF4Emu
{
    public static class Helper
    {
        public static List<string> ConvertStringList(string data)
        {
            List<string> res = new List<string>();
            string t = data.Replace("{", "");
            string[] t2 = t.Split('}');
            foreach (string line in t2)
                if (line.Trim() != "")
                    res.Add(line.Trim());
            return res;
        }
        public static void ConvertDoubleStringList(string data, out List<string> list1, out List<string> list2)
        {
            List<string> res1 = new List<string>();
            List<string> res2 = new List<string>();
            string t = data.Replace("{", "");
            string[] t2 = t.Split('}');
            foreach (string line in t2)
                if (line.Trim() != "")
                {
                    string[] t3 = line.Trim().Split(';');
                    res1.Add(t3[0].Trim());
                    res2.Add(t3[1].Trim());
                }
            list1 = res1;
            list2 = res2;
        }
        public static byte[] ReadContentSSL(SslStream sslStream)
        {
            MemoryStream res = new MemoryStream();
            byte[] buff = new byte[0x10000];
            sslStream.ReadTimeout = 100;
            int bytesRead;
            try
            {
                while ((bytesRead = sslStream.Read(buff, 0, 0x10000)) > 0)
                    res.Write(buff, 0, bytesRead);
            }
            catch { }
            sslStream.Flush();
            return res.ToArray();
        }
        public static byte[] ReadContentTCP(NetworkStream Stream)
        {
            MemoryStream res = new MemoryStream();
            byte[] buff = new byte[0x10000];
            Stream.ReadTimeout = 100;
            int bytesRead;
            try
            {
                while ((bytesRead = Stream.Read(buff, 0, 0x10000)) > 0)
                    res.Write(buff, 0, bytesRead);
            }
            catch { }
            Stream.Flush();
            return res.ToArray();
        }
        public static void RunShell(string file, string command)
        {
            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = file;
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();
        }

        public static string BytesToLabel(byte[] data)
        {
            byte[] result = new byte[4];
            result[3] = (byte)((data[2] & 0x3F) + 0x20);
            result[2] = (byte)(((data[2] & 0xC0) >> 6) + ((data[1] & 0x0F) << 2) + 0x20);
            result[1] = (byte)(((data[1] & 0xF0) >> 4) + ((data[0] & 0x03) << 4) + 0x20);
            result[0] = (byte)(((data[0] & 0xFC) >> 2) + 0x20);
            return Encoding.UTF8.GetString(result);
        }

        public static byte[] LabelToBytes(string Label)
        {
            byte[] result = new byte[3];
            byte[] buff = Encoding.UTF8.GetBytes(Label);
            for (int i = 0; i < 4; i++)
                buff[i] -= 0x20;
            result[0] = (byte)(((buff[0] & 0x3F) << 2) | ((buff[1] & 0x30) >> 4));
            result[1] = (byte)(((buff[1] & 0x0F) << 4) | ((buff[2] & 0x3C) >> 2));
            result[2] = (byte)(((buff[2] & 0x03) << 6) | (buff[3] & 0x3F));
            return result;
        }

        public static long ReadCompressedInteger(Stream s)
        {
            long result = 0;
            byte b = (byte)s.ReadByte();
            result += (b & 0x3F);
            int currshift = 6;
            while ((b & 0x80) != 0)
            {
                b = (byte)s.ReadByte();
                result |= ((long)(b & 0x7F) << currshift);
                currshift += 7;
            }
            return result;
        }

        public static void WriteCompressedInteger(Stream s, long l)
        {
            if (l < 0x40)
                s.WriteByte((byte)l);
            else
            {
                byte curbyte = (byte)((l & 0x3F) | 0x80);
                s.WriteByte(curbyte);
                long currshift = l >> 6;
                while (currshift >= 0x80)
                {
                    curbyte = (byte)((currshift & 0x7F) | 0x80);
                    currshift >>= 7;
                    s.WriteByte(curbyte);
                }
                s.WriteByte((byte)currshift);
            }
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string ByteArrayToHexString(byte[] buff)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in buff)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }


    }
}
