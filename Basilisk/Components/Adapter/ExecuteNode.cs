using BH.UI.Basilisk.Templates;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Execute")]
    [NodeDescription("Execute a command in the external software")]
    [NodeCategory("Basilisk.Adapter")]
    [InPortNames("Adapter", "Command", "Params", "Config", "Active")]
    [InPortTypes("object", "string", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "Command to run", "Parameters of the command\nDefault: null", "Execute config\nDefault: null", "Execute\nDefault: false")]
    [OutPortNames("Success")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("Coomand ran successfully")]
    [IsDesignScriptCompatible]
    public class ExecuteNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExecuteNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Execute";

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