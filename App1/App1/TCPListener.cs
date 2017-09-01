using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace App1
{
    class TCPListener
    {
        string ip = "39.108.122.78";
        Int32 port = 63440;
        public static TcpClient client;
        public static NetworkStream stream;
        public TCPListener()
        {
            client = new TcpClient(ip, port); 
            
            stream = client.GetStream();                                         
        }
    }
}
