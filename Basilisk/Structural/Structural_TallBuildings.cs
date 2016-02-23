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
using DynamoServices;

namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="TallBuildings">Tall buildings structural for Dynamo</class>
    /// </summary>
    public class TallBuildings
    {        
        internal TallBuildings(){}

        /// <summary>
         /// Create bars by curves
        /// </summary>
        /// <param name="curves"></param>
        /// <param name="facetLength"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Bars", "Nodes"})]
        public static Dictionary<string, object> CreateByCurves(IEnumerable<Curve> curves,
        double facetLength = 0)     
        {
            Dictionary<string, object> bars_out = new Dictionary<string, object>();
            return bars_out;
        }

        
    
    }

}