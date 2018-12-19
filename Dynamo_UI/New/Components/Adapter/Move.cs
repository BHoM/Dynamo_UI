using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Move")]
    [NodeCategory("BHoM.Adapter")]
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
