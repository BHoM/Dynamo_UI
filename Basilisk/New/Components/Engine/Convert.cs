using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Convert")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Convert to and from a BHoM object")]
    [IsDesignScriptCompatible]
    public class ConvertComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new ConvertCaller();


        /*******************************************/
    }
}
