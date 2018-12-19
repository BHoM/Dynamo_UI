using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("SetProperty")]
    [NodeCategory("BHoM.Engine")]
    [NodeDescription("Set the value of a property with a given name for an object")]
    [IsDesignScriptCompatible]
    public class SetPropertyComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new SetPropertyCaller();


        /*******************************************/
    }
}
