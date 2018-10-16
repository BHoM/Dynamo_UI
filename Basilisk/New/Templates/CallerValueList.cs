﻿using BH.oM.UI;
using BH.UI.Templates;
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

namespace BH.UI.Basilisk.Templates
{
    public abstract class CallerValueList : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract MultiChoiceCaller Caller { get; }

        protected Guid InstanceID { get; } = Guid.NewGuid();

        public int SelectedIndex = -1;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerValueList() : base()
        {
            Category = "Basilisk." + Caller.Category;
            ArgumentLacing = LacingStrategy.Shortest;

            Caller.SetDataAccessor(new DataAccessor_Dynamo());
            BH.Engine.Dynamo.Compute.Callers[InstanceID.ToString()] = Caller;

            AddPort(PortType.Output, Caller.OutputParams.First().ToPortData(), 0);

            RefreshComponent();
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RefreshComponent()
        {
            NickName = Caller.Name;
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
