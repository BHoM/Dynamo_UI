using BH.UI.Basilisk.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Move (old)")]
    [NodeDescription("Move objects from one external software to another")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Source", "Target", "Query", "Config", "Active")]
    [InPortTypes("object", "object", "object", "object", "bool")]
    [InPortDescriptions("Adapter the data is pulled from", "Adapter the data is pushed to", "BHoM Query\nDefault: new FilterQuery()", "Move config (custom object)\nDefault: null", "Execute the move\nDefault: false")]
    [OutPortNames("Success")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("Confirms if the operation was successful")]
    [IsDesignScriptCompatible]
    public class MoveNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MoveNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Move";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 2, AstFactory.BuildNullNode() },
                { 3, AstFactory.BuildNullNode() },
                { 4, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
        }


        /*******************************************/
    }
}