using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Execute")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Execute command in the external software")]
    [IsDesignScriptCompatible]
    public class ExecuteComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        protected override MethodCaller MethodCaller { get; } = new ExecuteCaller();


        /*******************************************/
    }
}
