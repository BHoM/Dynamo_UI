using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using RobotToolkit;
using BHoM.Structural;

namespace Robot
{
    /// <summary>
    /// Robot WindCFD tools
    /// BuroHappold
    /// <class name="RobotPanels">Robot panel tools</class>
    /// </summary>
    public class WindCFD
    {
        internal WindCFD() {}

        public static bool Generate(RobotToolkit.WindDirection windDirection, RobotToolkit.WindParams windParams, bool activate = false)
        {
            if (activate)
            {
                RobotToolkit.WindCFD.Generate(windDirection, windParams);
            }
            return true;
        }

        public static WindDirection SetParameters(bool xPositive = true, bool xNegative = false, bool yPositive = true, bool yNegative = false, bool quadrants = false)
        {
            WindDirection windDir = new WindDirection();
            windDir.SetDirection(xPositive, xNegative, yPositive, yNegative, quadrants);
            return windDir;
        }      

         public static WindParams SetParameters(double deviationPercent = 0.5, double terrainLevel = 0, double velocity = 24.385, bool openingsClosed = true)
        {
            WindParams windParams = new WindParams();
            windParams.SetParameters(deviationPercent, terrainLevel, velocity, openingsClosed);
            return windParams;
        }      
       
    }
    }

