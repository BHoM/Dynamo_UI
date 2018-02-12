using Dynamo.Graph.Nodes;
using BH.UI.Basilisk.Templates;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Compute Object")]
    [NodeDescription("Run Heavy Computation on a BHoM Object")]
    [NodeCategory("Basilisk.Engine")]
    [IsDesignScriptCompatible]
    public class ComputeObjectNode : MethodNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ComputeObjectNode() {}

        /*******************************************/
    }
}
