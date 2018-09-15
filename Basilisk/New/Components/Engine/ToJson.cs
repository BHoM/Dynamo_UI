using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("ToJson")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Convert a BHoMObject To a Json string")]
    [InPortTypes("object")]
    [IsDesignScriptCompatible]
    public class ToJsonComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new ToJsonCaller();


        /*******************************************/
    }
}
