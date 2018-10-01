using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("CreateCustom")]
    [NodeCategory("Basilisk.oM")]
    [NodeDescription("Creates an instance of a selected type of BHoM object by manually defining its properties (default type is CustomObject)")]
    [IsDesignScriptCompatible]
    public class CreateCustomComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateCustomCaller();


        /*******************************************/
    }
}
