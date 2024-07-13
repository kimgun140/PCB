using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PCB.Models
{
    public class CSharpToPython
    {
        Socket CSharpSock { get; set; }
        public CSharpToPython() { }
        public void Connect()
        {

            CSharpSock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            CSharpSock.Connect(System.Net.IPAddress.Parse("10.10.21.105"), 10001);
            //CSharpSock.Connect(System.Net.IPAddress.Parse("10.10.21.105"), 10001);
        }

        public void OrderEqtExit() // 서버 끄기
        {
            Connect();
            string msg = "0";
            SendRecv(msg);
            CSharpSock.Close();
        }

        public void OrderEqtPass() //1차 검사에서 통과 정상일때 3차 정상 위치로 이동
        {
            Connect();
            string msg = "1";
            SendRecv(msg);
            Disconnect();

        }
        public void OrderEqtFirstErr() //2차 검사 위치로 이동 1차에서 불량일때
        {
            Connect();
            string msg = "2";
            SendRecv(msg);
            Disconnect();
        }
        public void OrderEqtFirstErrPass() //2차 검사에서 통과일때 3차 정상 위치로 이동
        {
            Connect();
            string msg = "3";
            SendRecv(msg);
            Disconnect();
        }
        public void OrderEqtFirstErrErr() // 2차 검사에서 3차 불량 위치로 이동
        {
            Connect();
            string msg = "4";
            SendRecv(msg);
            Disconnect();
        }
        public void OrderEqtFirstPos() // 1차 카메라 위치로 이동
        {
            Connect();
            string msg = "5";
            SendRecv(msg);
            Disconnect();
        }
        public void OrderEqtSecondPos() // 2차 카메라 위치로 이동
        {
            Connect();
            string msg = "6";
            SendRecv(msg);
            Disconnect();
        }
        public void OrderEqtInitPos() // 초기 위치로 이동
        {
            Connect();
            string msg = "7";
            SendRecv(msg);
            Disconnect();
        }
        public void Disconnect()
        {
            string msg = "9";
            SendRecv(msg);
            CSharpSock.Close();
        }
        public void SendRecv(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            CSharpSock.Send(data);
            CSharpSock.Receive(data);
        }
    }
}


