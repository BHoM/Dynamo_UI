using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Query")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Query information about a BHoM object")]
    [IsDesignScriptCompatible]
    public class QueryComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new QueryCaller();


        /*******************************************/
    }
}
