using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("UpdateProperty")]
    [NodeDescription("Update property of a selection of objectsin the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Filter", "Property", "NewValue", "Config", "Active")]
    [InPortTypes("object", "object", "string", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "Filer Query", "Name of the property to change", "New value to assign to the property", "UpdateProperty config", "Execute the update")]
    [OutPortNames("#Updated")]
    [OutPortTypes("int")]
    [OutPortDescriptions("Number of objects updated")]
    [IsDesignScriptCompatible]
    public class UpdatePropertyNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdatePropertyNode()
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
                var functionCall = AstFactory.BuildFunctionCall("Methods.CRUD", "UpdateProperty", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}