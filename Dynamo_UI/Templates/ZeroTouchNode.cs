using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.UI.Dynamo.Templates
{
    public abstract class ZeroTouchNode : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public Dictionary<int, AssociativeNode> DefaultValues { get; set; } = new Dictionary<int, AssociativeNode>();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Handle the default values for inputs
            bool canRun = true;
            for (int i = 0; i < inputAstNodes.Count; i++)
            {
                if (inputAstNodes[i].Kind == AstKind.Null)
                {
                    if (DefaultValues.ContainsKey(i))
                        inputAstNodes[i] = DefaultValues[i];
                    else
                    {
                        canRun = false;
                        break;
                    }
                }
            }

            // Handle the production of the output
            if (!canRun)
            {
                //TODO: Add a warning message for the component
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                var functionCall = AstFactory.BuildFunctionCall(ClassName, MethodName, inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }

        /*******************************************/
    }
}
