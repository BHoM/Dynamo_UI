using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("GetInfo")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Get information about the BHoM, a specific dll or method")]
    [IsDesignScriptCompatible]
    public class GetInfoComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new GetInfoCaller();


        /*******************************************/
    }
}
