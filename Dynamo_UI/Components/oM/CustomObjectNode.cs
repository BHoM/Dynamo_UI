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

using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CustomObj (old)")]
    [NodeDescription("Creates a custom BHoMObject from custom inputs")]
    [NodeCategory("BHoM.oM")]
    /*[OutPortNames("object")]      // Not good. Output disapear as soon as an input is added
    [OutPortTypes("object")]
    [OutPortDescriptions("Custom BHoM Object")]*/
    [IsDesignScriptCompatible]
    public class CustomObjectNode : VariableInputNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public List<string> InputNames { get; set; } = new List<string>();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CustomObjectNode() 
        {
            InPortData.Add(new PortData("item0", ""));  //TODO: Would be great to know what is the alternative because attributes are providing weird behavious
            OutPortData.Add(new PortData("object", "Custom BHoM Object"));  // It seems those not deprecated after all: https://github.com/DynamoDS/Dynamo/issues/7207
                                                                            // Actually, check this: https://github.com/DynamoDS/Dynamo/pull/7301
            base.RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Disabled;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override string GetInputName(int index)
        {
            if (index < InputNames.Count)
                return InputNames[index];
            else
                return "item" + index;
        }

        /*******************************************/

        protected override string GetInputTooltip(int index)
        {
            return "";
        }

        /*******************************************/

        protected override void RemoveInput()
        {
            if (InPortData.Count > 1)
                base.RemoveInput();
        }

        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                List<AssociativeNode> names = new List<AssociativeNode>();
                for (int i = 0; i < inputAstNodes.Count; i++)
                    names.Add(AstFactory.BuildStringNode(GetInputName(i)));

                // Create the Build assignment for outpout 0
                List<AssociativeNode> arguments = new List<AssociativeNode> { AstFactory.BuildExprList(names) }.Concat(inputAstNodes).ToList();
                AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.BHoM.Methods.Create", "CustomObject", arguments);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var nameList = xmlDoc.CreateElement("InputNames");
            element.AppendChild(nameList);
            for (int i = 0; i < InputNames.Count; i++)
            {
                var name = xmlDoc.CreateElement("InputName");
                name.SetAttribute("value", InputNames[i]);
                nameList.AppendChild(name);
            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            InputNames = new List<string>();

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

            if (InputNames.Count == InPortData.Count)
            {
                for (int i = 0; i < InputNames.Count; i++)
                    InPortData[i].NickName = InputNames[i];
            }

            RegisterAllPorts();
        }

        /*******************************************/
    }
}
