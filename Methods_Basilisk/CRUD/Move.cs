using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Basilisk.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static bool Move(BHoMAdapter source, BHoMAdapter target, object query = null, CustomObject config = null, bool active = false)
        {
            if (query == null)
                query = new FilterQuery();
            else if (!(query is IQuery))
                throw new System.Exception("This component only accepts objects of type IQuery");

            if (active)
            {
                Dictionary<string, object> conf = (config != null) ? config.CustomData : null;
                bool result = source.PullTo(target, query as IQuery, conf);
                return result;
            }
            else
                return false;
        }

        /***************************************************/
    }
}
