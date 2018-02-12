using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Delete")]
    [NodeDescription("Delete objects from the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Filter", "Config", "Active")]
    [InPortTypes("object", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "FilterQuery", "Delete config", "Execute the delete")]
    [OutPortNames("#deleted")]
    [OutPortTypes("int")]
    [OutPortDescriptions("Number of objects deleted")]
    [IsDesignScriptCompatible]
    public class DeleteNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public DeleteNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.CRUD", "Delete", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}