using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Dynamo.Controls;
using Dynamo.Wpf;

namespace Structure_Basilisk_CustomUI
{
    [NodeName("Create BHoM Object")]
    [NodeDescription("Example Node Model, multiplies A x the value of the slider")]
    [NodeCategory("Basilisk.Base.BHoMObject")]
    [IsDesignScriptCompatible]

    public class listBoxMe : VariableInputNode
    {
        private double _sliderValue;

        protected override string GetInputName(int index)
        {
            return "";
        }

        protected override string GetInputTooltip(int index)
        {
            return "Add your custom data";
        }

        protected override void RemoveInput()
        {
            if (InPorts.Count > 3)
                base.RemoveInput();
        }

        protected override void AddInput()
        {
            base.AddInput();
            
        }

        public listBoxMe()
        {
            InPortData.Add(new PortData("Name", "Input the name of your custom object"));
            InPortData.Add(new PortData("Tags", "Input the tags of your custom object"));
            InPortData.Add(new PortData("", "Input the tags of your custom object"));
            OutPortData.Add(new PortData("BHoM Object", "Your custom created BHoM object"));
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
        }
       
    }

    public class listBoxNodeView : VariableInputNodeViewCustomization, INodeViewCustomization<listBoxMe>
    {
        public void CustomizeView(listBoxMe model, NodeView nodeView)
        {
            UserControl1 listbox = new UserControl1()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
            nodeView.inputGrid.Children.Add(listbox);
            listbox.DataContext = model;
            base.CustomizeView(model, nodeView);
        }

        public void Dispose()
        {
        }
    }
}
