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

using Dynamo.Graph.Nodes;
using BH.UI.Dynamo.Templates;
using System;
using System.Collections.Generic;
using ProtoCore.AST.AssociativeAST;
using System.Xml;
using Dynamo.Graph;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Enum (old)")]
    [NodeDescription("Creates an enum to choose from the context menu")]
    [NodeCategory("BHoM.oM")]
    [IsDesignScriptCompatible]
    public class BHoMEnumNode : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Type EnumType { get; set; } = null;

        public string EnumValue { get; set; } = "";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMEnumNode()
        {
            OutPortData.Add(new PortData("  ", "selected enum"));
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Make sure the enum has been assigned
            if (EnumType == null || EnumValue == "")
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };


            // Create the Build assignment for outpout 0
            List<AssociativeNode> arguments = new List<AssociativeNode> { AstFactory.BuildStringNode(EnumType.AssemblyQualifiedName), AstFactory.BuildStringNode(EnumValue) };
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.BHoM.Methods.Create", "BHoMEnum", arguments);
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            if (EnumType != null)
            {
                var typeName = xmlDoc.CreateElement("EnumType");
                typeName.SetAttribute("value", EnumType.AssemblyQualifiedName);
                element.AppendChild(typeName);

                var methodName = xmlDoc.CreateElement("EnumValue");
                methodName.SetAttribute("value", EnumValue);
                element.AppendChild(methodName);
            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            Type type = null;
            string methodName = "";
            List<Type> paramTypes = new List<Type>();

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "EnumType":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                        {
                            string enumTypeString = node.Attributes["value"].Value;

                            //Fix for namespace change in structure
                            if (enumTypeString.Contains("oM.Structural"))
                            {
                                enumTypeString = enumTypeString.Replace("oM.Structural", "oM.Structure");
                            }
                            EnumType = Type.GetType(enumTypeString);
                        }
                        break;
                    case "EnumValue":
                        if (node.Attributes != null && node.Attributes["value"] != null)
                            EnumValue = node.Attributes["value"].Value;
                        break;
                }
            }
        }

        /***************************************************/
    }
}
