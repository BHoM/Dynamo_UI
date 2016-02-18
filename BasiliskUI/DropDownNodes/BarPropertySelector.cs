using System.Collections.Generic;
using DSCoreNodesUI;
using Dynamo.Nodes;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;

namespace BasiliskNodesUI
{
    /// <summary>
    /// Dropdown selector for a structural object
    /// </summary>
    [NodeName("GetProperty")]
    [NodeDescription("Get the property of a structural bar object")]
    [NodeCategory("Basilisk.Structural.Bar")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "Bar", "Property", "Get")]
    [IsDesignScriptCompatible]
    public class BarPropertySelector : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public BarPropertySelector() : base("Property")
        {
            InPortData.Add(new PortData("bar", "Input BHoM bar object"));
            RegisterInputPorts();
        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();

            BHoM.Structural.Bar dummyBar = new BHoM.Structural.Bar();
            BHoM.Collections.Dictionary<string, object> properties = dummyBar.GetProperties();
            List<string> propertyNames = properties.KeyList();

            for (int i = 0; i < propertyNames.Count; i++)
            {
                Items.Add(new DynamoDropDownItem(propertyNames[i], i));
            }
            SelectedIndex = 1;
        }

        /// <summary>
        /// Set the node function and outputs through associated nodes and AST
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var nodeFunction = AstFactory.BuildFunctionCall(
                new System.Func<BHoM.Structural.Bar, string, object>(Structural.Structure.GetPropertyByName),
                new List<AssociativeNode>() { inputAstNodes[0], AstFactory.BuildStringNode(Items[SelectedIndex].Name) });

            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), nodeFunction);
            
            return new List<AssociativeNode> { assign };
        }
    }
}
