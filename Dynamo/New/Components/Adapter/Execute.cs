using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Execute")]
    [NodeCategory("BHoM.Adapter")]
    [NodeDescription("Execute command in the external software")]
    [IsDesignScriptCompatible]
    public class ExecuteComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ExecuteCaller();


        /*******************************************/
    }
}
