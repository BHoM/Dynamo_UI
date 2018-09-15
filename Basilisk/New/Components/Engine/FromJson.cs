using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("FromJson")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Convert a Json string to a BHoMObject")]
    [IsDesignScriptCompatible]
    public class FromJsonComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new FromJsonCaller();


        /*******************************************/
    }
}
