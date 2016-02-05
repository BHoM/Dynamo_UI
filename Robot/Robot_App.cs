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
    /// Robot Application tools
    /// BuroHappold
    /// <class name="Applicaiton">Robot application tools</class>
    /// </summary>
    public class RobotApp
    {
        internal RobotApp() { }
        /// <summary>
        /// Clears elements from a structural analysis model, and provides a true/false output to trigger downstream nodes
        /// BuroHappold
        /// </summary>
        /// <param name="clearStructure"></param>
        /// <param name="deleteNodes"></param>
        /// <param name="deleteIsolatedNodes"></param>
        /// <param name="deleteBars"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        /// <search>BH, clear, delete</search>
        public static bool ClearStructure(bool clearStructure = false, bool deleteNodes = false, bool deleteIsolatedNodes = false, bool deleteBars = false, bool activate = false)
        {
            if (activate == true)
            {
                RobotToolkit.FileIO robot = new RobotToolkit.FileIO();
                if (clearStructure) RobotToolkit.General.ClearStructure();
                if (deleteBars) RobotToolkit.Bar.DeleteBars("all");
                if (deleteBars) RobotToolkit.Node.DeleteNodes("all");
                RobotToolkit.General.RefreshView();
            }
            return true;
        }
        /// <summary>
        /// Get free Robot object model numbers - so that new objects can be created using higher index values. 
        /// BuroHappold
        /// </summary>
        /// <param name="activate"></param>
        /// <returns></returns>
        /// <search>BH, robot, getfree</search>
        
        [MultiReturn(new[] { "NodeNumber", "BarNumber", "ObjectNumber" })]
        public static Dictionary<string,object> GetFreeNumbers(bool activate = false)
        {
              //Output dictionary definition
                Dictionary<string, object> freenums_out = new Dictionary<string, object>();
   
            if (activate == true)
            {
              
                int freeNodeNum = 0;
                int freeBarNum = 0;
                int freeObjNum = 0;
                RobotToolkit.General.GetFreeNumbers(out freeNodeNum, out freeBarNum, out freeObjNum);

                freenums_out.Add("NodeNumber", freeNodeNum);
                freenums_out.Add("BarNumber", freeBarNum);
                freenums_out.Add("ObjectNumber", freeObjNum);
            }
            return freenums_out;
        }

        [CanUpdatePeriodically(true)]
        public static void RunCalculations(bool activate = false)
        {
            if(activate)
            {
                RobotToolkit.App.RunCalculations();
            }
        }
    
        }   
   }

