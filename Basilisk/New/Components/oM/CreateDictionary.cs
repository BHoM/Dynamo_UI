using BH.Engine.Dynamo.Objects;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("CreateDictionary")]
    [NodeCategory("Basilisk.oM")]
    [NodeDescription("Create a dictionary")]
    [IsDesignScriptCompatible]
    public class CreateDictionaryComponent : CallerComponent
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override Caller Caller { get; } = new CreateDictionaryCaller();


        /*******************************************/
    }
}
