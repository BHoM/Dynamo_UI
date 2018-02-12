using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Pull")]
    [NodeDescription("Pull objects from the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Query", "Config", "Active")]
    [InPortTypes("object", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "BHoM Query", "Pull config", "Execute the pull")]
    [OutPortNames("Objects")]
    [OutPortTypes("object[]")]
    [OutPortDescriptions("Objects obtained from the query")]
    [IsDesignScriptCompatible]
    public class PullNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PullNode()
        {
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                var functionCall = AstFactory.BuildFunctionCall("Methods.CRUD", "Pull", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}