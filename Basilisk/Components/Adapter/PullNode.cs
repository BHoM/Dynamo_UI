using BH.UI.Basilisk.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Pull")]
    [NodeDescription("Pull objects from the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Query", "Config", "Active")]
    [InPortTypes("object", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "BHoM Query\nDefault: new FilterQuery()", "Pull config\nDefault: null", "Execute the pull\nDefault: false")]
    [OutPortNames("Objects")]
    [OutPortTypes("object[]")]
    [OutPortDescriptions("Objects obtained from the query")]
    [IsDesignScriptCompatible]
    public class PullNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PullNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Pull";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 1, AstFactory.BuildNullNode() },
                { 2, AstFactory.BuildNullNode() },
                { 3, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}