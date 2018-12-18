using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using CoreNodeModels;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CreateEnum")]
    [NodeCategory("BHoM.oM")]
    [NodeDescription("Creates a selected enum value")]
    [IsDesignScriptCompatible]
    public class CreateEnumComponent : CallerValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MultiChoiceCaller Caller { get; } = new CreateEnumCaller();


        /*******************************************/
    }
}
