using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public FromSocket()
        {
            m_Socket = new Socket_Engine.SocketServer();
            m_Socket.MessageReceived += MessageRecieved;
        }

        //private int _port;
//private bool _active; 

        public static string FromSocketByPort(int port = 8800, bool active = false)
        {
            FromSocket s = new FromSocket();

            
            if (active)
            {               
                s.m_Socket.Listen(port);
                s.m_Socket.MessageReceived += s.MessageRecieved;
                return s.m_Message;
            }
            else
            {
                return null;
            }
        }         

        private Socket_Engine.SocketServer m_Socket;
        private string m_Message = "";

        private void MessageRecieved(string message)
        {
            m_Message = message;
            //ExpireSolution(true);
        }

        //private void ExpireSolution(bool v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
