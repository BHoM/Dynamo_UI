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
    [NodeDescription("Get the property of a structural section property object")]
    [NodeCategory("Basilisk.Structural.SectionProperty")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "Section", "Property", "Get")]
    [IsDesignScriptCompatible]
    public class SectionPropertySelector : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public SectionPropertySelector() : base("Property")
        {
            InPortData.Add(new PortData("sectionProperty", "Input BHoM bar object"));
            RegisterInputPorts();
        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();
            BHoM.Structural.SectionProperties.SectionFactory sec_factory = new BHoM.Structural.SectionProperties.SectionFactory();
            BHoM.Structural.SectionProperties.SteelISection sec_prop = (BHoM.Structural.SectionProperties.SteelISection)sec_factory.Create(BHoM.Structural.SectionProperties.ShapeType.SteelI);
            
            

            BHoM.Collections.Dictionary<string, object> properties = sec_prop.GetProperties();
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
