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

namespace Project
{
    /// <summary>
    /// Robot bar tools
    /// BuroHappold
    /// <class name="Bar">Robot bar tools</class>
    /// </summary>
    public class Project
    {
        internal Project()
        {
            BHoM.Global.Project project = new BHoM.Global.Project();
        }
    }
}

