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

        /// <summary>
        /// Gerenate Wind CFD analysis
        /// </summary>
        /// <param name="windDirection"></param>
        /// <param name="windParams"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        public static bool Generate(RobotToolkit.WindDirection windDirection, RobotToolkit.WindParams windParams, bool activate = false)
        {
            if (activate)
            {
                RobotToolkit.WindCFD.Generate(windDirection, windParams);
            }
            return true;
        }

        /// <summary>
        /// Set Wind CFD analysis parameters
        /// </summary>
        /// <param name="xPositive"></param>
        /// <param name="xNegative"></param>
        /// <param name="yPositive"></param>
        /// <param name="yNegative"></param>
        /// <param name="quadrants"></param>
        /// <returns></returns>
        public static WindDirection SetParameters(bool xPositive = true, bool xNegative = false, bool yPositive = true, bool yNegative = false, bool quadrants = false)
        {
            WindDirection windDir = new WindDirection();
            windDir.SetDirection(xPositive, xNegative, yPositive, yNegative, quadrants);
            return windDir;
        }      

        /// <summary>
        /// Set wind parameters for Wind CFD
        /// </summary>
        /// <param name="deviationPercent"></param>
        /// <param name="terrainLevel"></param>
        /// <param name="velocity"></param>
        /// <param name="openingsClosed"></param>
        /// <returns></returns>
         public static WindParams SetParameters(double deviationPercent = 0.5, double terrainLevel = 0, double velocity = 24.385, bool openingsClosed = true)
        {
            WindParams windParams = new WindParams();
            windParams.SetParameters(deviationPercent, terrainLevel, velocity, openingsClosed);
            return windParams;
        }      
       
    }
    }

