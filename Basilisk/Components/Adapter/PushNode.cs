using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Push")]
    [NodeDescription("Push objects to the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Objects", "Tag", "Config", "Active")]
    [InPortTypes("object", "object[]", "string", "object", "bool")]
    [InPortDescriptions("Adapter", "Objects to push", "Tag to apply to the objects being pushed", "Push config (custom object)", "Execute the push")]
    [OutPortNames("Success", "Objects")]
    [OutPortTypes("object[]")]
    [OutPortDescriptions("Pushed objects")]
    [IsDesignScriptCompatible]
    public class PushNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PushNode()
        {
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (inputAstNodes[0].Kind == AstKind.Null || inputAstNodes[0].Kind == AstKind.Null)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                var functionCall = AstFactory.BuildFunctionCall("Methods.CRUD", "Push", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}