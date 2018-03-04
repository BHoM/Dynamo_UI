using Dynamo.Graph.Nodes;
using BH.UI.Basilisk.Templates;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Type")]
    [NodeDescription("Creates a type to choose from the context menu")]
    [NodeCategory("Basilisk.oM")]
    [IsDesignScriptCompatible]
    public class BHoMTypeNode : TypeNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMTypeNode()
        {
            RegisterAllPorts();
        }


        /*******************************************/
    }
}
