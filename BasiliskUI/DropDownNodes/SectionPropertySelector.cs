using System.Collections.Generic;
using Dynamo.Nodes;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Graph.Nodes;
using CoreNodeModels;

namespace BasiliskNodesUI
{
    /// <summary>
    /// Dropdown selector for a structural object
    /// </summary>
    [NodeName("GetProperty")]
    [NodeDescription("Get the property of a structural sectionProperty object")]
    [NodeCategory("Basilisk.Structural.SectionProperty")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "SectionProperty", "Property", "Get")]
    [IsDesignScriptCompatible]
    [InPortNames("sectionProperty")]
    [InPortDescriptions("sectionProperty")]
    [InPortTypes("dynamic")]

    public class SectionPropertyPropertySelector : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public SectionPropertyPropertySelector() : base("Property")
        {

        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();


            BHoM.Structural.SectionProperties.SectionFactory sectionPropertyFactory = new BHoM.Structural.SectionProperties.SectionFactory(new BHoM.Global.Project());
            BHoM.Structural.SectionProperties.SectionProperty dummySectionProperty = sectionPropertyFactory.CreateSteelI("dummy");
            List<string> propertyNames = dummySectionProperty.GetPropertyNames();

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

                new System.Func<BHoM.Structural.SectionProperties.SectionProperty, string, object>(Structural.Structure.GetPropertyByName),

                new List<AssociativeNode>() { inputAstNodes[0], AstFactory.BuildStringNode(Items[SelectedIndex].Name) });

            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), nodeFunction);

            return new List<AssociativeNode> { assign };
        }
    }
}
