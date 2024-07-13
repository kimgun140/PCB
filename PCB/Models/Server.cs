using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PCB.View;
using PCB.Models;
using PCB.VIEW;
using System.Windows;
using static OpenCvSharp.Stitcher;

namespace PCB.Models
{
    public class Server
    {


        const int BUFFSIZE = 1024;

        const char REQ_INF = (char)0x01;
        const char SEN_INF = (char)0x02;
        const char INSPEC1 = (char)0x03;
        const char INSPEC2 = (char)0x04;
        const char INSPEC3 = (char)0x05;
        const char PROGRAM_OVER = (char)0x06;

        const string VOID = "";
        const string SEP = "\n";

        public Socket Socket { get; private set; }
        public string? Address { get; set; }
        public string? Port { get; set; }
        public string? recinfo { get; set; }

        public void Connect()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            IPAddress localIPAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(localIPAddress, 0);
            sock.Bind(localEndPoint);
            sock.Connect(IPAddress.Parse("10.10.21.113"), 5555);
            string temp = sock.LocalEndPoint.ToString();
            string[] temp2 = temp.Split(':');
            this.Address = temp2[0];
            this.Port = temp2[1];
            this.Socket = sock;
        }

        public void send_REQ_INF()
        {
            
            Server server = new Server();
            //server.Connect();
            try
            {
                string msg = REQ_INF.ToString();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string Truefalse(int data)
        {
            string truefalse;
            if (data == 0)
            {
                truefalse = "불량";
            }
            else
            {
                truefalse = "정상";
            }
            return truefalse;
        }

        public void send_INSPECTION1(PCBinfo pcbinfo)
        {
            Connect();
            try
            {
                string msg = INSPEC1 + SEP + pcbinfo.Status;
                byte[] data = Encoding.UTF8.GetBytes(msg);
                Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void send_INSPECTION2(PCBinfo pcbinfo)
        {
            Connect();
            try
            {
                string msg = INSPEC2 + SEP + pcbinfo.MCU + SEP + pcbinfo.LTC + SEP + pcbinfo.ADC + SEP + pcbinfo.DAC + SEP + pcbinfo.XTR + SEP + pcbinfo.LED1 + SEP + pcbinfo.LED2;
                byte[] data = Encoding.UTF8.GetBytes(msg);
                MessageBox.Show($"data: {msg}");
                Socket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        // 동기적으로 구현한 수신(1회)
        public void recvMsgSync()
        {
            StringBuilder ab = new StringBuilder();
            string Answer;

            byte[] data = new byte[BUFFSIZE];
            int size = Socket.Receive(data);
            switch (data[0])
            {
                case (byte)SEN_INF:
                    {
                        data = data.Skip(1).ToArray();
                        ab.Append(Encoding.UTF8.GetString(data, 0, size - 1));
                        Answer = ab.ToString();
                        recinfo = Answer;
                        Console.WriteLine(Answer);
                        ab.Clear();
                    }
                    break;

                default:
                    return;
            }
        }
    }
}

