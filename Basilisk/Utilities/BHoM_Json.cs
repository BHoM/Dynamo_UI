using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;

namespace Utilities
{
    /// <summary>
    /// Utility tools
    /// BuroHappold
    /// <class name="BHUtilityTools">Utility tools for Dynamo</class>
    /// </summary>
    public class Json
    {
        /// <summary></summary>
        internal Json() { } 

        /// <summary>
        /// Creates Json from a BHoM object. 
        /// BuroHappold
        /// </summary>
        /// <param name="element"></param> 
        /// <returns></returns>
        /// <search>BH, utility, json</search>
        [RegisterForTrace]
        public static string CreateJson(BHoM.Structural.Grid element)
        {
            return element.JSON();
        }
    }
}
