using BH.UI.Basilisk.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Delete")]
    [NodeDescription("Delete objects from the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Filter", "Config", "Active")]
    [InPortTypes("object", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "FilterQuery\nDefault: new FilterQuery()", "Delete config\nDefault: null", "Execute the delete\nDefault: false")]
    [OutPortNames("#deleted")]
    [OutPortTypes("int")]
    [OutPortDescriptions("Number of objects deleted")]
    [IsDesignScriptCompatible]
    public class DeleteNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public DeleteNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Delete";

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