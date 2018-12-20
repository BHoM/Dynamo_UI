/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
