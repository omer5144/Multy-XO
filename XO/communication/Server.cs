using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XO.communication
{
    public class Server : Communication
    {
        private Socket listener;
        private Socket handler;
        private byte[] bytes;
        private bool opened;

        public Server(int port)
        {
            listener = null;
            handler = null;

            bytes = new byte[1024];

            IPAddress ipAddress =IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(1);

            opened = false;
        }

        public void Open()
        {
            if (opened)
                return;
            handler = listener.Accept();
            opened = true;
        }

        public string Send(string msg)
        {
            if (!opened)
                return null;

            handler.Send(Encoding.ASCII.GetBytes(msg));
            return msg;
        }

        public string Recv()
        {
            if (!opened)
                return null;

            int bytesRec = handler.Receive(bytes);
            return Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }

        public void Close()
        {
            if (!opened)
                return;

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

            opened = false;
        }
    }
}
