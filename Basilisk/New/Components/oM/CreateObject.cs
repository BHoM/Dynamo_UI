﻿using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("CreateObject")]
    [NodeCategory("Basilisk.oM")]
    [NodeDescription("Creates an instance of a selected type of BHoM object")]
    [IsDesignScriptCompatible]
    public class CreateObjectComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateObjectCaller();


        /*******************************************/
    }
}