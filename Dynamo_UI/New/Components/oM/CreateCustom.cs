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

using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CreateCustom")]
    [NodeCategory("BHoM.oM")]
    [NodeDescription("Creates an instance of a selected type of BHoM object by manually defining its properties (default type is CustomObject)")]
    [IsDesignScriptCompatible]
    public class CreateCustomComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateCustomCaller();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var nameList = xmlDoc.CreateElement("InputNames");
            element.AppendChild(nameList);
            foreach (string input in Caller.InputParams.Select(x => x.Name))
            {
                var name = xmlDoc.CreateElement("InputName");
                name.SetAttribute("value", input);
                nameList.AppendChild(name);
            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            base.DeserializeCore(element, context);

            List<string> InputNames = new List<string>();

            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "InputNames")
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Attributes != null && child.Attributes["value"] != null)
                            InputNames.Add(child.Attributes["value"].Value);
                    }
                }
            }

            CreateCustomCaller caller = Caller as CreateCustomCaller;
            caller.SetInputs(InputNames);
            RegisterInputs();
        }


        /*******************************************/
    }
}
