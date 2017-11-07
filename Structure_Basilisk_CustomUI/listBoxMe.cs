using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

    public class listBoxMe : NodeModel
    {
        private double _sliderValue;

        public listBoxMe()
        {
            OutPortData.Add(new PortData("BHoM Object", "Your custom created BHoM object"));
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
        }
       
    }
}
