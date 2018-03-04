using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("GetProperty")]
    [NodeDescription("Get a property from an object")]
    [NodeCategory("Basilisk.Engine")]
    [InPortNames("object", "property")]
    [InPortTypes("object", "string")]
    [InPortDescriptions("object to get the property from", "property name")]
    [OutPortNames("value")]      
    [OutPortTypes("object")]
    [OutPortDescriptions("property value of the object")]
    [IsDesignScriptCompatible]
    public class GetPropertyNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public GetPropertyNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.Query", "GetProperty", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}
