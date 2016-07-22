using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using DS = Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using RobotToolkit;
using BHoM.Structural;

namespace Robot
{
    /// <summary>
    /// Robot panel tools
    /// BuroHappold
    /// <class name="SAT">Robot SAT tools</class>
    /// </summary>
    public class RobotSAT
    {
        internal RobotSAT() {}
        /// <summary>
        /// Generates an SAT surface model in Robot through export of a SAT file. Note that if 'withOrigin' is set to true
        /// a dummy surface 10 units square is generated at 0,0,0 to force the SAT geometry to maintain it's relationship
        /// to the origin.
        /// BuroHappold
        /// </summary>
        /// <param name="geometry">Input surfaces to convert to panels</param>
        /// <param name="withOrigin">If set to true, a dummy surface 10x10 units square is exported at 0,0,0 to force origin to origin export</param>
        /// <param name="activate">Set true to activate the node</param>
        /// <search>
        /// BH, robot, SAT
        /// </search>
        public static Dictionary<string, object> CreateBySurfaces(IEnumerable<DS.Geometry> geometry, bool withOrigin = true, bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> createSAT_out = new Dictionary<string, object>();
            if (withOrigin)
            {
                DS.Point[] dummy_panel_pnts = new[] {DS.Point.ByCoordinates(0,0,0), DS.Point.ByCoordinates(10,0,0), DS.Point.ByCoordinates(10,10,0), DS.Point.ByCoordinates(0,10,0)};
                DS.Geometry[] dummy_panel = new[] { (DS.Geometry)DS.Surface.ByPerimeterPoints(dummy_panel_pnts)};
                geometry = dummy_panel.Concat(geometry);
            }
            if (activate)
            {
                string path1 = @"c:\Users";
                string path2 = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                string path3 = "Desktop";
                string path4 = "new.sat";
                string fileName = Path.Combine(path1, path2, path3, path4);
                Autodesk.DesignScript.Geometry.Geometry.ExportToSAT(geometry, fileName);
                RobotToolkit.FileIO fi = new FileIO(fileName);
             }
            return createSAT_out;
        }
    }
}

