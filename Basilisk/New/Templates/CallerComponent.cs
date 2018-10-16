using BH.oM.UI;
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
using BH.UI.Basilisk.Global;
using System.Xml;
using Dynamo.Graph;

namespace BH.UI.Basilisk.Templates
{
    public abstract class CallerComponent : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract Caller Caller { get; }

        protected Guid InstanceID { get; } = Guid.NewGuid();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerComponent() : base()
        {
            Category = "Basilisk." + Caller.Category;
            ArgumentLacing = LacingStrategy.Shortest;

            Caller.SetDataAccessor(new DataAccessor_Dynamo());
            Caller.ItemSelected += (sender, e) => RefreshComponent();
            BH.Engine.Dynamo.Compute.Callers[InstanceID.ToString()] = Caller;

            RefreshComponent();
        }

        /*******************************************/

        static CallerComponent()
        {
            GlobalSearchMenu.Activate();
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RefreshComponent()
        {
            NickName = Caller.Name;
            Description = Caller.Description;

            RegisterInputs();
            RegisterOutputs();
            ValidateConnections();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        // TODO: Look into FunctionDefinitionNode and ArgumentSignatureNode as well as NodeToCode to figure out how to force input types (lacing) and potentially generic method call 
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Check if the component has all the inputs it needs
            if (!IsReady(inputAstNodes))
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

            // Apppend replication guide to the input parameter based on lacing strategy
            UseLevelAndReplicationGuide(inputAstNodes);

            // Define function to call
            Tuple<List<AssociativeNode>, List<AssociativeNode>> processed = GetProcessedInputs(inputAstNodes);
            AssociativeNode callerId = AstFactory.BuildStringNode(InstanceID.ToString());
            List<AssociativeNode> arguments = new List<AssociativeNode>() { callerId }.Concat(processed.Item1).ToList();
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Compute", "RunCaller", arguments);

            // Produce the output
            List<AssociativeNode> transforms = processed.Item2;
            List<AssociativeNode> assignments = CreateOutputAssignments(functionCall, callerId);
            return transforms.Concat(assignments).ToList();
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var componentString = xmlDoc.CreateElement("Component");
            componentString.SetAttribute("value", Caller.Write());
            element.AppendChild(componentString);
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
                }
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected bool IsReady(List<AssociativeNode> inputAstNodes)
        {
            List<bool> hasDefaultList = Caller.InputParams.Select(x => x.HasDefaultValue).ToList();
            bool isReady = inputAstNodes != null && inputAstNodes.Count == hasDefaultList.Count();
            if (isReady)
            {
                for (int i = 0; i < inputAstNodes.Count; i++)
                {
                    if (inputAstNodes[i].Kind == AstKind.Null && !hasDefaultList[i])
                    {
                        isReady = false;
                        break;
                    }
                }
            }

            return isReady;
        }

        /*******************************************/

        protected Tuple<List<AssociativeNode>, List<AssociativeNode>> GetProcessedInputs(List<AssociativeNode> inputAstNodes)
        {
            // Get the params from the method caller and make sure they are the correct length
            List<ParamInfo> paramInfos = Caller.InputParams;
            if (paramInfos.Count != inputAstNodes.Count)
                return new Tuple<List<AssociativeNode>, List<AssociativeNode>>(inputAstNodes, new List<AssociativeNode>());

            // Define inputs transforms if any
            string prefix = Guid.NewGuid().ToString() + "_";
            List<AssociativeNode> arguments = new List<AssociativeNode>();
            List<AssociativeNode> transforms = new List<AssociativeNode>();

            for (int i = 0; i < inputAstNodes.Count; i++)
            {
                int depth = paramInfos[i].Depth();
                string transformerName = (depth == 1) ? "ListWrapper" : (depth == 2) ? "TreeWrapper" : "";

                if (transformerName == "")
                    arguments.Add(inputAstNodes[i]);
                else
                {
                    AssociativeNode transform = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Create", transformerName, new List<AssociativeNode> { inputAstNodes[i] });
                    AssociativeNode newVar = AstFactory.BuildIdentifier(prefix + i.ToString());
                    transforms.Add(AstFactory.BuildAssignment(newVar, transform));
                    arguments.Add(newVar);
                }
            }
            
            return new Tuple<List<AssociativeNode>, List<AssociativeNode>>(arguments, transforms);
        }

        /*******************************************/

        protected List<AssociativeNode> CreateOutputAssignments(AssociativeNode functionCall, AssociativeNode callerId)
        {
            List<ParamInfo> outParams = Caller.OutputParams;
            List<AssociativeNode> assignments = new List<AssociativeNode>();

            int nbOutputs = Math.Min(outParams.Count, OutPorts.Count);
            
            if (outParams.Count == 0)
            {
                //Do nothing ?
            }
            else if (outParams.Count == 1 && nbOutputs == 1)
            {
                assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall));
            }
            else
            {
                AssociativeNode newVar = AstFactory.BuildIdentifier(InstanceID.ToString());
                assignments.Add(AstFactory.BuildAssignment(newVar, functionCall));

                for (int i = 0; i < nbOutputs; i++)
                {
                    AssociativeNode accessor = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Query", "ItemFromCustom", new List<AssociativeNode> { newVar, AstFactory.BuildIntNode(i) });
                    assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(i), accessor));
                }
            }

            return assignments;
        }

        /*******************************************/

        protected void RegisterInputs()
        {
            if (Caller == null)
                return;

            List<ParamInfo> inputs = Caller.InputParams;
            int nbNew = inputs.Count;
            int nbOld = InPorts.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                InPorts[i].SetPortData(inputs[i].ToPortData());

            for (int i = nbOld - 1; i >= nbNew; i--)
                InPorts.RemoveAt(i);

            for (int i = nbOld; i < nbNew; i++)
                AddPort(PortType.Input, inputs[i].ToPortData(), i);

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/

        protected void RegisterOutputs()
        {
            if (Caller == null)
                return;

            List<ParamInfo> outputs = Caller.OutputParams;
            int nbNew = outputs.Count;
            int nbOld = OutPorts.Count;

            for (int i = 0; i < Math.Min(nbNew, nbOld); i++)
                OutPorts[i].SetPortData(outputs[i].ToPortData());

            for (int i = nbOld - 1; i >= nbNew; i--)
                OutPorts.RemoveAt(i);

            for (int i = nbOld; i < nbNew; i++)
                AddPort(PortType.Output, outputs[i].ToPortData(), i);

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/
    }
}
