using Autodesk.DesignScript.Runtime;

namespace BH.UI.Basilisk.Methods
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static object FromJson(string json)
        {
            return Engine.Serialiser.Convert.FromJson(json);
        }

        /***************************************************/
    }
}
