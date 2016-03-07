using System.Collections.Generic;
using Dynamo.Nodes;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Graph.Nodes;
using CoreNodeModels;

namespace BasiliskBarForcesUI
{
    /// <summary>
    /// Dropdown selector for a structural object
    /// </summary>
    [NodeName("GetProperty")]
    [NodeDescription("Get the property of a structural barForce object")]
    [NodeCategory("Basilisk.Structural.BarForce")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "BarForce", "Property", "Get")]
    [IsDesignScriptCompatible]
    [InPortNames("barForce")]
    [InPortDescriptions("barForce")]
    [InPortTypes("dynamic")]

    public class BarForcePropertySelector : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public BarForcePropertySelector() : base("Property")
        {

        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();

            List<string> propertyNames = new List<string>();
            foreach (var prop in typeof(BHoM.Structural.Results.Bars.BarForce).GetProperties())
            {
                propertyNames.Add(prop.Name);
            }
            propertyNames.Sort();

            for (int i = 0; i < propertyNames.Count; i++)
            {
                Items.Add(new DynamoDropDownItem(propertyNames[i], i));
            }
            SelectedIndex = 1;
        }

        /// <summary>
        /// Set the barForce function and outputs through associated barForces and AST
        /// </summary>
        /// <param name="inputAstNodes"></param>
        /// <returns></returns>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            var barForceFunction = AstFactory.BuildFunctionCall(

                new System.Func<BHoM.Structural.Results.Bars.BarForce, string, object>(Structural.Structure.GetPropertyByName),

                new List<AssociativeNode>() { inputAstNodes[0], AstFactory.BuildStringNode(Items[SelectedIndex].Name) });

            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), barForceFunction);

            return new List<AssociativeNode> { assign };
        }
    }
}
