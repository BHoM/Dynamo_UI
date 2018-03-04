using Dynamo.Graph.Nodes;
using BH.UI.Basilisk.Templates;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Enum")]
    [NodeDescription("Creates an enum to choose from the context menu")]
    [NodeCategory("Basilisk.oM")]
    [IsDesignScriptCompatible]
    public class BHoMEnumNode : EnumNode
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMEnumNode()
        {
            RegisterAllPorts();
        }


        /*******************************************/
    }
}
