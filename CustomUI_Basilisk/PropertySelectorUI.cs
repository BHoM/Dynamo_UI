using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasiliskNodesUI
{
    /*/// <summary>
    /// Dropdown selector for a structural object
    /// </summary>
    [NodeName("GetProperty")]
    [NodeDescription("Get the property of a BhoM object")]
    [NodeCategory("Basilisk.Global.BHoM_Object")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "BHoM", "Object", "Property", "Get")]
    [IsDesignScriptCompatible]
    [InPortNames("bhomObject")]
    [InPortDescriptions("BHoM Object")]
    [InPortTypes("dynamic")]

    class PropertySelectorUI : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public PropertySelectorUI() : base("Property")
        {

        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();


            BHoM.Structural.NodeFactory nodeFactory = new BHoM.Structural.NodeFactory(new BHoM.Global.Project());
            BHoM.Structural.Node dummyNode = nodeFactory.Create();
            List<string> propertyNames = dummyNode.GetPropertyNames();

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

                new System.Func<BHoM.Structural.Node, string, object>(Structural.Structure.GetPropertyByName),

                new List<AssociativeNode>() { inputAstNodes[0], AstFactory.BuildStringNode(Items[SelectedIndex].Name) });

            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), nodeFunction);

            return new List<AssociativeNode> { assign };
        }
    }*/
}
