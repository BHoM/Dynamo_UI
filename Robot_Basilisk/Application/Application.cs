using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSI = BH.Adapter.Robot;

namespace Basilisk.Robot
{
    public static class Robot
    {
        public static RSI.RobotAdapter RobotApplication()
        {
            RSI.RobotAdapter app = new RSI.RobotAdapter();
            return app;
        }
    }
}
