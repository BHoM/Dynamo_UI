using Autodesk.DesignScript.Runtime;
using BH.Engine.Dynamo;
using BH.oM.Base;
using BH.oM.Geometry;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Linq;

namespace BH.UI.Dynamo.Methods
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object GetProperty(object obj, string name)
        {
            if (obj is CustomObject)        //TODO: This should be moved to the Reflection_Engine
            {
                CustomObject custom = obj as CustomObject;

                switch (name)
                {
                    case "Name":
                        return custom.Name;
                    case "Tags":
                        return custom.Tags;
                    case "BHoM_Guid":
                        return custom.BHoM_Guid;
                    default:
                        {
                            if (custom.CustomData.ContainsKey(name))
                            {
                                object result = custom.CustomData[name];
                                if (result is IGeometry)
                                    result = BH.Engine.Dynamo.Convert.IToDesignScript(result as dynamic);
                                return result;
                            }
                            else
                                return null;
                        }
                }                
            }
            else
            {
                object result = Engine.Reflection.Query.PropertyValue(obj, name);
                if (result is IGeometry)
                    result = BH.Engine.Dynamo.Convert.IToDesignScript(result as dynamic);
                else if (result is IEnumerable && !(result is string))
                {
                    result = ((IEnumerable)result).Cast<object>().Select(x => BH.Engine.Dynamo.Convert.IToDesignScript(x as dynamic));
                }
                return result;
            }
                
        }

        /***************************************************/
    }
}
