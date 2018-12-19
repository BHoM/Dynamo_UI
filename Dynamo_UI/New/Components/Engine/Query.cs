using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Query")]
    [NodeCategory("BHoM.Engine")]
    [NodeDescription("Query information about a BHoM object")]
    [IsDesignScriptCompatible]
    public class QueryComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new QueryCaller();


        /*******************************************/
    }
}
