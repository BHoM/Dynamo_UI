using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Delete")]
    [NodeCategory("Basilisk.Adapter")]
    [NodeDescription("Delete objects in the external software")]
    [IsDesignScriptCompatible]
    public class DeleteComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new DeleteCaller();


        /*******************************************/
    }
}
