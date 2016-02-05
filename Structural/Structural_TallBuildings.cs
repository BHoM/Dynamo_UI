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
using Dynamo.Services;

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

 
        [MultiReturn(new[] { "Bars", "Nodes"})]
        public static Dictionary<string, object> CreateByCurves(IEnumerable<Curve> curves,
        double facetLength = 0)     
        {
            Dictionary<string, object> bars_out = new Dictionary<string, object>();
           

            
            
            return bars_out;
        }

        
    
    }

}