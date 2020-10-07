/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using BH.UI.Base;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Dynamo;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Dynamo.Objects;
using BH.Engine.UI;
using System.Reflection;
using System.Collections;
using BH.UI.Dynamo.Global;
using System.Xml;
using Dynamo.Graph;
using Dynamo.Graph.Connectors;
using Newtonsoft.Json;

namespace BH.UI.Dynamo.Templates
{
    public abstract partial class CallerComponent : NodeModel
    {
        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            // This is marked as dperecated by it still the only way to get copy/paste to work

            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var componentString = xmlDoc.CreateElement("Component");
            componentString.SetAttribute("value", Caller.Write());
            element.AppendChild(componentString);
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            // This is marked as dperecated by it still the only way to get copy/paste to work

            base.DeserializeCore(element, context);

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Component":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                        {
                            Caller.Read(node.Attributes["value"].Value);
                        }
                        break;
                }
            }

            // Do Dynamo's job once more (should have been covered by base.DeserializeCore if it wasn't buggy)
            if (InPorts.Count == 0)
            {
                foreach (ParamInfo param in Caller.InputParams.Where(x => x.IsSelected))
                    InPorts.Add(new PortModel(PortType.Input, this, param.ToPortData()));
            }
            if (OutPorts.Count == 0)
            {
                foreach (ParamInfo param in Caller.OutputParams.Where(x => x.IsSelected))
                    OutPorts.Add(new PortModel(PortType.Output, this, param.ToPortData()));
            }
        }

        /*******************************************/
    }
}

