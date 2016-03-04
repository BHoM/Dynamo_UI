using System.Collections.Generic;
using Dynamo.Nodes;
using Dynamo.Models;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Graph.Nodes;
using CoreNodeModels;
using Autodesk.DesignScript.Runtime;
using DynamoServices;

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
    [InPortNames("bar")]
    [InPortDescriptions("bar")]
    [InPortTypes("dynamic")]
    
    public class BarPropertySelector : DSDropDownBase
    {        
        /// <summary>
        /// Set the inputs (InPorts)
        /// </summary>
        public BarPropertySelector() : base("Property")
        {
            //this.PropertyChanged += OnPropertyChanged;
        }

        void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           PopulateItems();
           
        }

        public override void Dispose()
        {
            PropertyChanged -= OnPropertyChanged;
            base.Dispose();
        }

        /// <summary>
        /// Set the dropdown list
        /// </summary>
        public override void PopulateItems()
        {
            Items.Clear();
                       
            BHoM.Structural.BarFactory barFactory = new BHoM.Structural.BarFactory(new BHoM.Global.Project());
            BHoM.Structural.Bar dummyBar = barFactory.Create();
            List<string> propertyNames = dummyBar.GetPropertyNames();

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
