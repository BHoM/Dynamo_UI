using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("CreateQuery")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Creates an instance of a selected type of adapter query")]
    [IsDesignScriptCompatible]
    public class CreateQueryComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateQueryCaller();


        /*******************************************/
    }
}
