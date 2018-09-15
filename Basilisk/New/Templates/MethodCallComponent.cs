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

namespace BH.UI.Basilisk.Templates
{
    public abstract class MethodCallComponent : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public abstract MethodCaller MethodCaller { get; }

        protected Guid InstanceID { get; } = Guid.NewGuid();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodCallComponent() : base()
        {
            Category = "Basilisk." + MethodCaller.Category;
            ArgumentLacing = LacingStrategy.Shortest;

            MethodCaller.SetDataAccessor(new DataAccessor_Dynamo());
            BH.Engine.Dynamo.Compute.Callers[InstanceID.ToString()] = MethodCaller;

            RefreshtMethod();
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RefreshtMethod()
        {
            NickName = MethodCaller.Name;
            Description = MethodCaller.Description;

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
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Compute", "RunMethodCaller", arguments);

            // Produce the output
            List<AssociativeNode> transforms = processed.Item2;
            List<AssociativeNode> assignments = CreateOutputAssignments(functionCall, callerId);
            return transforms.Concat(assignments).ToList();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected bool IsReady(List<AssociativeNode> inputAstNodes)
        {
            List<bool> hasDefaultList = MethodCaller.InputParams.Select(x => x.HasDefaultValue).ToList();
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
            List<ParamInfo> paramInfos = MethodCaller.InputParams;
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
            List<ParamInfo> outParams = MethodCaller.OutputParams;
            List<AssociativeNode> assignments = new List<AssociativeNode>();
            
            if (outParams.Count == 0)
            {
                //Do nothing ?
            }
            else if (outParams.Count == 1)
            {
                assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall));
            }
            else
            {
                AssociativeNode newVar = AstFactory.BuildIdentifier(InstanceID.ToString());
                assignments.Add(AstFactory.BuildAssignment(newVar, functionCall));

                for (int i = 0; i < outParams.Count; i++)
                {
                    AssociativeNode accessor = AstFactory.BuildFunctionCall("BH.Engine.Dynamo.Query", "Item", new List<AssociativeNode> { callerId, AstFactory.BuildIntNode(i) });
                    assignments.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(i), accessor));
                }
            }

            return assignments;
        }

        /*******************************************/

        protected void RegisterInputs()
        {
            if (MethodCaller == null)
                return;

            List<PortData> inputs = MethodCaller.InputParams.Select(x => x.ToPortData()).ToList();

            for (int i = 0; i < inputs.Count; i++)
                AddPort(PortType.Input, inputs[i], i);

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/

        protected void RegisterOutputs()
        {
            if (MethodCaller == null)
                return;

            List<PortData> outputs = MethodCaller.OutputParams.Select(x => x.ToPortData()).ToList();

            for (int i = 0; i < outputs.Count; i++)
                AddPort(PortType.Output, outputs[i], i);

            RaisesModificationEvents = true;
            OnNodeModified();
        }

        /*******************************************/
    }
}
