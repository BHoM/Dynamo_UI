using BH.UI.Dynamo.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("UpdateProperty (old)")]
    [NodeDescription("Update property of a selection of objectsin the external software")]
    [NodeCategory("BHoM.Adapter")]
    [InPortNames("Adapter", "Filter", "Property", "NewValue", "Config", "Active")]
    [InPortTypes("object", "object", "string", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "Filter Query", "Name of the property to change", "New value to assign to the property", "UpdateProperty config\nDefault: null", "Execute the update\nDefault: false")]
    [OutPortNames("#Updated")]
    [OutPortTypes("int")]
    [OutPortDescriptions("Number of objects updated")]
    [IsDesignScriptCompatible]
    public class UpdatePropertyNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public UpdatePropertyNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "UpdateProperty";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 4, AstFactory.BuildNullNode() },
                { 5, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}