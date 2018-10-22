using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("ToJson (old)")]
    [NodeDescription("Convert a BHoMObject To a Json string")]
    [NodeCategory("Basilisk.Engine")]
    [InPortNames("obj")]
    [InPortTypes("object")]
    [InPortDescriptions("object to be converted")]
    [OutPortNames("json")]      
    [OutPortTypes("string")]
    [OutPortDescriptions("string representing the object in json")]
    [IsDesignScriptCompatible]
    public class ToJsonNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ToJsonNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.Convert", "ToJson", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}
