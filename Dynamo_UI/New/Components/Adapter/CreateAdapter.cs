using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("CreateAdapter")]
    [NodeCategory("BHoM.Adapter")]
    [NodeDescription("Creates an instance of a selected type of Adapter")]
    [IsDesignScriptCompatible]
    public class CreateAdapterComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateAdapterCaller();


        /*******************************************/
    }
}
