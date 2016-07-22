using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotToolkit;

namespace Robot
{
    public static class Application
    {
        public static object RobotApp()
        {
            return new RobotAdapter();
        }
    }
}
