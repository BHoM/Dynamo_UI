using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Compute")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Run a computationally intensive calculations")]
    [IsDesignScriptCompatible]
    public class ComputeComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new ComputeCaller();


        /*******************************************/
    }
}
