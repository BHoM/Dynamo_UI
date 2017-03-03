using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Socket_Engine;

namespace Socket_Basilisk
{
    public class Message
    {
        public string M { get; set; }
        public Message(string message)
        {
            M = message;
        }
    }

    public class FromSocket
    {
        public FromSockets()
        {
            m_Socket = new Socket_Engine.SocketServer();
            m_Socket.MessageReceived += MessageRecieved();
        }

        private int _port;
        //private bool _active; 

        public static string FromSocket ByPort(int port = 8800, bool active = false)
        {            
            if (active)
            {
                _port = port;

                //FromSocket s = new FromSocket();
                //s.m_Socket.Listen(port);
                //s.m_Socket.MessageReceived += s.MessageRecieved;
            }
        }         

        private Socket_Engine.SocketServer m_Socket;
        private string m_Message = "";

        private void MessageRecieved(string message)
        {
            m_Message = message;
            //return m_Message;
            //ExpireSolution(true);
        }

        //private void ExpireSolution(bool v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
