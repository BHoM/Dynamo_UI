using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSI = Robot_Adapter.Structural.Interface;

namespace Robot
{
    public static class Application
    {
        public static object RobotApp()
        {
            return new RSI.RobotAdapter();
        }
    }
}
