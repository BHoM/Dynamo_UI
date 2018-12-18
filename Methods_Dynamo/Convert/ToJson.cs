using Autodesk.DesignScript.Runtime;

namespace BH.UI.Dynamo.Methods
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
