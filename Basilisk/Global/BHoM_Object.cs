using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    /// <summary>
    /// BHoM Object
    /// </summary>
    public static class BHoM_Object
    {
        /// <summary></summary>
        public static object SetPropertyValueByName(object bhomObject, string propName, object value)
        {
            System.Reflection.PropertyInfo prop = bhomObject.GetType().GetProperty(propName);
            if (prop != null)
                prop.SetValue(bhomObject, value);

            return bhomObject;
        }

        /// <summary></summary>
        public static object GetPropertyValueByName(object bhomObject, string propName)
        {
            System.Reflection.PropertyInfo prop = bhomObject.GetType().GetProperty(propName);
            if (prop == null) return null;

            return prop.GetValue(bhomObject);
        }

        /// <summary></summary>
        public static List<string> GetPropertyNames(object bhomObject)
        {
            List<string> properties = new List<string>();
            foreach (System.Reflection.PropertyInfo prop in bhomObject.GetType().GetProperties())
            {
                properties.Add(prop.Name);
            }
            return properties;
        }

        /// <summary></summary>
        public static string ToJSON(object bhomObject)
        {
            return (bhomObject as BHoM.Global.BHoMObject).ToJSON();
        }

        /// <summary></summary>
        public static object FromJSON(string json)
        {
            return BHoM.Global.BHoMObject.FromJSON(json);
        }
    }
}
