/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using CoreNodeModels;
using System.Xml;
using Dynamo.Graph;
using Newtonsoft.Json;
using BH.UI.Dynamo.Global;

namespace BH.UI.Dynamo.Templates
{
    public abstract class CallerValueList : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        [JsonIgnore]
        public abstract MultiChoiceCaller Caller { get; }

        [JsonIgnore]
        protected Guid InstanceID { get; } = Guid.NewGuid();

        public int SelectedIndex = -1;

        public string SerialisedCaller
        {
            get { return Caller.Write(); }
            set { Caller.Read(value); }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerValueList() : base()
        {
            Category = "BHoM." + Caller.Category;
            ArgumentLacing = LacingStrategy.Auto;

            string instanceId = InstanceID.ToString();
            DataAccessor_Dynamo dataAccessor = new DataAccessor_Dynamo();
            dataAccessor.DynamoModel = Extension.DynamoModel;
            Caller.SetDataAccessor(dataAccessor);
            BH.Engine.Dynamo.Compute.Callers[instanceId] = Caller;
            BH.Engine.Dynamo.Compute.DataAccessors[instanceId] = dataAccessor;
            BH.Engine.Dynamo.Compute.Nodes[instanceId] = this;

            OutPorts.Add(new PortModel(PortType.Output, this, Caller.OutputParams.First().ToPortData()));

            RefreshComponent();
        }

        /*******************************************/

        [JsonConstructor]
        public CallerValueList(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            Category = "BHoM." + Caller.Category;
            ArgumentLacing = LacingStrategy.Auto;

            string instanceId = InstanceID.ToString();
            DataAccessor_Dynamo dataAccessor = new DataAccessor_Dynamo();
            dataAccessor.DynamoModel = Extension.DynamoModel;
            Caller.SetDataAccessor(dataAccessor);
            BH.Engine.Dynamo.Compute.Callers[instanceId] = Caller;
            BH.Engine.Dynamo.Compute.DataAccessors[instanceId] = dataAccessor;
            BH.Engine.Dynamo.Compute.Nodes[instanceId] = this;
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RefreshComponent()
        {
            Name = Caller.Name;
            Description = Caller.Description;

            MarkNodeAsModified(true);
            OnNodeModified(true);
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        // TODO: Look into FunctionDefinitionNode and ArgumentSignatureNode as well as NodeToCode to figure out how to force input types (lacing) and potentially generic method call 
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Check if the component has all the inputs it needs
            if (SelectedIndex == -1)
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

            // Apppend replication guide to the input parameter based on lacing strategy
            UseLevelAndReplicationGuide(inputAstNodes);

            // Define function to call
            AssociativeNode callerId = AstFactory.BuildStringNode(InstanceID.ToString());
            AssociativeNode valueIndex = AstFactory.BuildIntNode(SelectedIndex);
            List<AssociativeNode> arguments = new List<AssociativeNode>() { callerId, valueIndex };
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Compute", "RunCaller", arguments);

            // Produce the output
            return new List<AssociativeNode> { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var componentString = xmlDoc.CreateElement("Component");
            componentString.SetAttribute("value", Caller.Write());
            element.AppendChild(componentString);

            var selectionIndex = xmlDoc.CreateElement("SelectionIndex");
            selectionIndex.SetAttribute("value", SelectedIndex.ToString());
            element.AppendChild(selectionIndex);
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Component":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                            Caller.Read(node.Attributes["value"].Value);
                        break;

                    case "SelectionIndex":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                            SelectedIndex = int.Parse(node.Attributes["value"].Value);
                        break;
                }
            }
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/


        /*******************************************/
    }
}





