using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CreateType")]
    [NodeCategory("BHoM.oM")]
    [NodeDescription("Creates a selected type definition")]
    [IsDesignScriptCompatible]
    public class CreateTypeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateTypeCaller();


        /*******************************************/
    }
}
