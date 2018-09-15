using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("UpdateProperty")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Update a specific property of objects from the external software")]
    [IsDesignScriptCompatible]
    public class UpdatePropertyComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new UpdatePropertyCaller();


        /*******************************************/
    }
}
