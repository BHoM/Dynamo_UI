using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Explode")]
    [NodeCategory("Basilisk.Engine")]
    [NodeDescription("Explode an object into its properties")]
    [IsDesignScriptCompatible]
    public class ExplodeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ExplodeCaller();


        /*******************************************/
        /**** Constructor                       ****/
        /*******************************************/


        /*******************************************/
    }
}
