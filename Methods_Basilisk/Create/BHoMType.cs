using Autodesk.DesignScript.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Basilisk.Methods
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object BHoMType(string typeName)
        {
            return Type.GetType(typeName);
        }

        /***************************************************/
    }
}
