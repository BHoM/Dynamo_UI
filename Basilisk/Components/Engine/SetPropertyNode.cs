using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("SetProperty")]
    [NodeDescription("Set a specific property of an object")]
    [NodeCategory("Basilisk.Engine")]
    [InPortNames("object", "property", "value")]
    [InPortTypes("object", "string", "object")]
    [InPortDescriptions("object to modify", "property name", "new value")]
    [OutPortNames("  ")]      
    [OutPortTypes("modified object")]
    [OutPortDescriptions("modified object")]
    [IsDesignScriptCompatible]
    public class SetPropertyNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public SetPropertyNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.Query", "SetProperty", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}
