using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
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
                                return custom.CustomData[name];
                            else
                                return null;
                        }
                }                
            }
            else
                return Engine.Reflection.Query.PropertyValue(obj, name);
        }

        /***************************************************/
    }
}
