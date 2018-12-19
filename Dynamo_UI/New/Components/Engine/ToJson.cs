using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("ToJson")]
    [NodeCategory("BHoM.Engine")]
    [NodeDescription("Convert a BHoMObject To a Json string")]
    [InPortTypes("object")]
    [IsDesignScriptCompatible]
    public class ToJsonComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ToJsonCaller();


        /*******************************************/
    }
}
