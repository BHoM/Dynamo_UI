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
using BH.Adapter;
using BH.oM.Base;
using BH.oM.Data.Requests;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool Move(BHoMAdapter source, BHoMAdapter target, object query = null, CustomObject pullConfig = null, CustomObject pushConfig = null, bool active = false)
        {
            if (query == null)
                query = new FilterRequest();
            else if (!(query is IRequest))
                throw new System.Exception("This component only accepts objects of type IRequest");

            if (active)
            {
                Dictionary<string, object> pullConf = (pullConfig != null) ? pullConfig.CustomData : null;
                Dictionary<string, object> pushConf = (pushConfig != null) ? pushConfig.CustomData : null;

                bool result = source.Move(target, query as IRequest, pullConf, pushConf);
                return result;
            }
            else
                return false;
        }

        /***************************************************/
    }
}
