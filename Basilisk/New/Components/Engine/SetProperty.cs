using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("SetProperty")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Set the value of a property with a given name for an object")]
    [IsDesignScriptCompatible]
    public class SetPropertyComponent : MethodCallComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MethodCaller MethodCaller { get; } = new SetPropertyCaller();


        /*******************************************/
    }
}
