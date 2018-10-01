using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Move")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Copy objects from a source adapter to a target adapter")]
    [IsDesignScriptCompatible]
    public class MoveComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new MoveCaller();


        /*******************************************/
    }
}
