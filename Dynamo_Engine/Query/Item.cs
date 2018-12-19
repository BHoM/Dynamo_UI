using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Dynamo
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object Item(string callerId, int index)
        {
            if (Compute.MultiResults.ContainsKey(callerId))
            {
                object[] array = Compute.MultiResults[callerId];
                if (index >= 0 && index < array.Length)
                    return array[index];
                else
                    return null;
            }
            else
                return null;
        }

        /***************************************************/

        public static object ItemFromCustom(object container, int index)
        {
            CustomObject custom = container as CustomObject;
            if (custom == null)
                return null;

            string key = index.ToString();
            if (custom.CustomData.ContainsKey(key))
                return custom.CustomData[key];
            else
                return null;
        }

        /***************************************************/
    }
}

