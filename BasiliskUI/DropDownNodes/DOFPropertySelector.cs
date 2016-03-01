using System.Collections.Generic;
using CoreNodeModels;
using Dynamo.Nodes;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Graph.Nodes;

namespace BasiliskNodesUI
{
    /// <summary>
    /// Dropdown selector for a structural object
    /// </summary>
    [NodeName("GetProperty")]
    [NodeDescription("Get the property of a structural dof object")]
    [NodeCategory("Basilisk.Structural.DOF")]
    [NodeSearchable(true)]
    [NodeSearchTags("BH", "Buro", "DOF", "Property", "Get")]
    [IsDesignScriptCompatible]
    public class DOFPropertySelector : DSDropDownBase
    {
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public DOFPropertySelector() : base("Property")
        {
            InPortNamesAttribute in_names = new InPortNamesAttribute("dof");
            RegisterAllPorts();
        }
        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();

            BHoM.Structural.DOF dummyDOF = new BHoM.Structural.DOF();
            List<string> propertyNames = dummyDOF.GetPropertyNames();

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
                new System.Func<BHoM.Structural.DOF, string, object>(Structural.Structure.GetPropertyByName),
                new List<AssociativeNode>() { inputAstNodes[0], AstFactory.BuildStringNode(Items[SelectedIndex].Name) });

            var assign = AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), nodeFunction);


            return new List<AssociativeNode> { assign };
        }
    }
}
