using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("FromJson (old)")]
    [NodeDescription("Convert a Json string back into a BHoMObject")]
    [NodeCategory("BHoM.Engine")]
    [InPortNames("json")]
    [InPortTypes("string")]
    [InPortDescriptions("string representing the object in json")]
    [OutPortNames("obj")]
    [OutPortTypes("object")]
    [OutPortDescriptions("object to be converted back")]
    [IsDesignScriptCompatible]
    public class FromJsonNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public FromJsonNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.Convert", "FromJson", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}
