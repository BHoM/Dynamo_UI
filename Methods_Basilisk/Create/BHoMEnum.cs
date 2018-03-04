using Autodesk.DesignScript.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object BHoMEnum(string typeName, string enumName)
        {
            Type type = Type.GetType(typeName);
            if (type == null)
                return null;
            else
            {
                IEnumerator enumerator = Enum.GetValues(type).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.ToString() == enumName)
                        return enumerator.Current;
                }
                return null;
            }
                
        }

        /***************************************************/
    }
}
