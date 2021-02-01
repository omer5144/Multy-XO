using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XO.communication
{
    public class Client : Communication
    {
        private IPEndPoint remoteEP;
        private Socket sender;
        private byte[] bytes;
        private bool opend;

        public Client(string ip, int port)
        {
            bytes = new byte[1024];


            IPAddress ipAddress = IPAddress.Parse(ip);
            this.remoteEP = new IPEndPoint(ipAddress, port);
            sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
       
           

            

            opend = false;
        }

        public void Open()
        {
            if (opend)
                return;
            sender.Connect(remoteEP);
            opend = true;
        }

        public string Send(string msg)
        {
            if (!opend)
                return null;

            sender.Send(Encoding.ASCII.GetBytes(msg));
            return msg;
        }

        public string Recv()
        {
            if (!opend)
                return null;

            int bytesRec = sender.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }

        public void Close()
        {
            if (!opend)
                return;

            sender.Shutdown(SocketShutdown.Both);
            sender.Close();

            opend = false;
        }
    }
}
