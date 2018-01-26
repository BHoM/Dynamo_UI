using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Basilisk.Components.Constructors
{
    [NodeName("CustomObj")]
    [NodeDescription("Creates a custom BHoMObject from custom inputs")]
    [NodeCategory("Alligator.oM")]
    [IsDesignScriptCompatible]
    public class CustomObject : VariableInputNode
    {
        protected override string GetInputName(int index)
        {
            return "item" + index;
        }

        protected override string GetInputTooltip(int index)
        {
            return "";
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                string name = "";
                Dictionary<string, object> properties = new Dictionary<string, object>();

                foreach (AssociativeNode node in inputAstNodes)
                {
                    properties[node.Name] = "Hello";
                }

                var functionCall = AstFactory.BuildFunctionCall(
                        new Func<Dictionary<string, object>, string, oM.Base.CustomObject>(Engine.Base.Create.CustomObject),
                        new List<AssociativeNode> { AstFactory.BuildPrimitiveNodeFromObject(properties), AstFactory.BuildStringNode(name) }
                );

                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }
    }
}
