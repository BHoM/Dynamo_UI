using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("GetProperty")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Get the value of a property with a given name from an object")]
    [IsDesignScriptCompatible]
    public class GetPropertyComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new GetPropertyCaller();


        /*******************************************/
    }
}
