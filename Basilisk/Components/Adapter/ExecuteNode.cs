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
    [InPortDescriptions("Adapter", "Command to run", "Parameters of the command", "Execute config", "Execute")]
    [OutPortNames("Success")]
    [OutPortTypes("bool")]
    [OutPortDescriptions("Coomand ran successfully")]
    [IsDesignScriptCompatible]
    public class ExecuteNode : NodeModel
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExecuteNode()
        {
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                var functionCall = AstFactory.BuildFunctionCall("Methods.CRUD", "Execute", inputAstNodes);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }


        /*******************************************/
    }
}