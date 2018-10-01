using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Push")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Push objects to the external software")]
    [IsDesignScriptCompatible]
    public class PushComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new PushCaller();


        /*******************************************/
    }
}
