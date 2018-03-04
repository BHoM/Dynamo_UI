using Autodesk.DesignScript.Runtime;
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class Query
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object SetProperty(object obj, string name, object value)
        {
            if (obj is CustomObject)        //TODO: This should be moved to the Reflection_Engine
            {
                CustomObject custom = obj as CustomObject;

                switch (name)
                {
                    case "Name":
                        custom.Name = value as string;
                        break;
                    case "Tags":
                        if (value is IEnumerable)
                        {
                            IEnumerator enumerator = ((IEnumerable)value).GetEnumerator();
                            HashSet<string> set = new HashSet<string>();
                            while (enumerator.MoveNext())
                                set.Add(enumerator.Current.ToString());
                            custom.Tags = set;
                        }
                        break;
                    case "BHoM_Guid":
                        custom.BHoM_Guid = (Guid)value;
                        break;
                    default:
                        custom.CustomData[name] = value;
                        break;
                }
                return obj;
            }
            else
            {
                Engine.Reflection.Modify.SetPropertyValue(obj, name, value); //TODO: Need to make a copy
                return obj;
            }
        }

        /***************************************************/
    }
}
