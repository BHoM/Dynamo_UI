using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BH.oM.Geometry;
using BHSE = BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using ADG = Autodesk.DesignScript.Geometry;
using AD = BH.Adapter.DesignScript;
using BHES = BH.Engine.Structure;
using ADR = Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using BHM = BH.oM.Materials;
using BH.oM.Base;

namespace Basilisk.Structure
{
    [NodeName("I'm a God")]
    [NodeDescription("Example Node Model, multiplies AxB")]
    [NodeCategory("Basilisk.Structure.Create.")]
    [InPortTypes("double", "double")]
    [InPortDescriptions("Number A", "Numnber B")]
    [OutPortNames("C")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Product of AxB")]
    [IsDesignScriptCompatible]
    public partial class NodeConstraint : NodeModel
    {
        public NodeConstraint()
        {
            RegisterAllPorts();
        }

        public double MultiplyTwoNumbers(double a, double b)
        {
            double res = a * b;
            return res;
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!HasConnectedInput(0) || !HasConnectedInput(1))
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
              AstFactory.BuildFunctionCall(
                new Func<double, double, double>(MultiplyTwoNumbers),
                new List<AssociativeNode> { inputAstNodes[0], inputAstNodes[1] });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}