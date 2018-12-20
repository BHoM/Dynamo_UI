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
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Methods
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
