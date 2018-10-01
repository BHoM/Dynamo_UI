﻿using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("CreateData")]
    [NodeCategory("Basilisk.oM")]
    [NodeDescription("Creates a BhoM object from the reference datasets")]
    [IsDesignScriptCompatible]
    public class CreateDataComponent : CallerValueList
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override MultiChoiceCaller Caller { get; } = new CreateDataCaller();


        /*******************************************/
    }
}
