using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
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

        public static object BHoMData(string fileName, string itemName)
        {
            if (fileName == "" || itemName == "")
                return null;
            else
            {
                IEnumerable<IBHoMObject> result = Engine.Library.Query.Library(fileName).Where(x => x.Name == itemName);

                if (result.Count() > 0)
                    return result.First();
                else
                    return null;
            }
        }

        /***************************************************/
    }
}
