using BH.Engine.Dynamo.Objects;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Dynamo.Graph;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Explode")]
    [NodeCategory("BHoM.Engine")]
    [NodeDescription("Explode an object into its properties")]
    [IsDesignScriptCompatible]
    public class ExplodeComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new ExplodeCaller();


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);
            RegisterOutputs();
        }

        /*******************************************/
    }
}
