using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("FromJson")]
    [NodeCategory("BHoM.Engine")]
    [NodeDescription("Convert a Json string to a BHoMObject")]
    [IsDesignScriptCompatible]
    public class FromJsonComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new FromJsonCaller();


        /*******************************************/
    }
}
