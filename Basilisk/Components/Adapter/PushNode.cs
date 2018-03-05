using BH.UI.Basilisk.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Push")]
    [NodeDescription("Push objects to the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Objects", "Tag", "Config", "Active")]
    [InPortTypes("object", "object[]", "string", "object", "bool")]
    [InPortDescriptions("Adapter", "Objects to push", "Tag to apply to the objects being pushed\nDefault: \"\"", "Push config (custom object)\nDefault: null", "Execute the push\nDefault: false")]
    [OutPortNames("Objects")]
    [OutPortTypes("object[]")]
    [OutPortDescriptions("Pushed objects")]
    [IsDesignScriptCompatible]
    public class PushNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PushNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Push";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 2, AstFactory.BuildStringNode("") },
                { 3, AstFactory.BuildNullNode() },
                { 4, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}