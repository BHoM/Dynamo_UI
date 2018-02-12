using Autodesk.DesignScript.Runtime;

namespace BH.UI.Basilisk.Methods
{
    [IsVisibleInDynamoLibrary(false)]
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string ToJson(object obj)
        {
            return Engine.Serialiser.Convert.ToJson(obj);
        }

        /***************************************************/
    }
}
