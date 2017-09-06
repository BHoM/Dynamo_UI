using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.Engine.Base;
using BH.Engine.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHB = BH.oM.Base;

namespace Base
{
    /// <summary>
    /// BHoM Object
    /// </summary>
    public static class BHoMObject
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BHB.BHoMObject SetPropertyValue(BHB.BHoMObject bhomObject, string propName, object value)
        {
            BHB.BHoMObject clone = bhomObject.GetShallowClone();
            clone.SetPropertyValue(propName, value);
            return clone;
        }

        /***************************************************/

        public static object GetPropertyValue(BHB.BHoMObject bhomObject, string propName)
        {
            return bhomObject.GetPropertyValue(propName);
        }

        /***************************************************/

        public static BHB.BHoMObject SetCustomData(BHB.BHoMObject bhomObject, string propName, object value)
        {
            BHB.BHoMObject clone = bhomObject.GetShallowClone();
            clone.CustomData[propName] = value;
            return clone;
        }

        /***************************************************/

        public static object GetCustomData(BHB.BHoMObject bhomObject, string propName = "")
        {
            if (propName.Length == 0)
                return bhomObject.CustomData;
            else
            {
                object value = null;
                bhomObject.CustomData.TryGetValue(propName, out value);
                return value;
            }
        }

        /***************************************************/

        public static List<string> GetPropertyNames(object bhomObject)
        {
            return bhomObject.GetPropertyNames();
        }

        /***************************************************/
        
        public static BH.oM.Geometry.IBHoMGeometry GetGeometry(BHB.BHoMObject bhomObject)
        {
            return bhomObject.GetGeometry();
        }

        /***************************************************/

        public static string ToString(object bhomObject)
        {
            return bhomObject.ToJson();
        }

        /***************************************************/

        public static BHB.CustomObject CreateObject(List<string> propertyNames, List<object> propertyValues)
        {
            return new BHB.CustomObject(propertyNames, propertyValues);
        }

        /***************************************************/

        [MultiReturn(new[] { "propertyNames", "propertyValues" })]
        public static Dictionary<string, object> ExplodeObject(BHB.BHoMObject obj)
        {
            Dictionary<string, object> properties = obj.GetPropertyDictionary();

            return new Dictionary<string, object>
            {
                { "propertyNames", properties.Keys },
                { "propertyValues", properties.Values }
            };
        }
    }
}
