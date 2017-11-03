using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;

namespace Structure_Basilisk_CustomUI
{
    [NodeName("HelloGui")]
    [NodeDescription("Example Node Model, multiplies A x the value of the slider")]
    [NodeCategory("HelloDynamo")]
    [InPortNames("A")]
    [InPortTypes("double")]
    [InPortDescriptions("Number A")]
    [OutPortNames("C")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Product of AxSlider")]
    [IsDesignScriptCompatible]

    public class listBoxMe : NodeModel
    {
        private double _sliderValue;


        public double SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                RaisePropertyChanged("SliderValue");
                OnNodeModified(false);
            }
        }

        public listBoxMe()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
        }
       
    }
}
