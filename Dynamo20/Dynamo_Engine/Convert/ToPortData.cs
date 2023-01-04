/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using BH.oM.UI;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Dynamo
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static PortData ToPortData(this ParamInfo info)
        {
            if (info.HasDefaultValue && !info.IsRequired)
            {
                object defaultValue = info.DefaultValue;
                AssociativeNode defaultNode = null;

                if (defaultValue == null)
                    defaultNode = AstFactory.BuildNullNode();
                else
                {
                    switch (defaultValue.GetType().FullName)
                    {
                        case "System.Boolean":
                            defaultNode = AstFactory.BuildBooleanNode((bool)defaultValue);
                            break;
                        case "System.Double":
                            defaultNode = AstFactory.BuildDoubleNode((double)defaultValue);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            defaultNode = AstFactory.BuildIntNode((int)defaultValue);
                            break;
                        case "System.String":
                            defaultNode = AstFactory.BuildStringNode((string)defaultValue);
                            break;
                        default:
                            break;
                    }
                }

                if (defaultNode == null)
                {
                    BH.Engine.Base.Compute.RecordError("Port " + info.Name + " failed to assigned it default value of " + defaultValue.ToString());
                    return new PortData(info.Name, info.Description);
                }   
                else
                    return new PortData(info.Name, info.Description, defaultNode);
            }
            else
            {
                return new PortData(info.Name, info.Description);
            }
        }

        /***************************************************/
    }
}




