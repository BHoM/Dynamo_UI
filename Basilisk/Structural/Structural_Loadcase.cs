using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

namespace Structural.Loads
{

    /// <summary>
    /// Nodal load class - to create and set BHoM nodal load objects.
    /// BuroHappold
    /// </summary>
    public class Loadcase
    {
       internal Loadcase(){}

        /// <summary>
        /// Create a loadcase by name and number
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
       public static object Create(string name, int number)
        {
            return new Dictionary<string, object> { { "Loadcase", new BHoM.Structural.Loads.Loadcase(number, name) } };
        }

    }
}
