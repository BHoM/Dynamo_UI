using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using System.Collections.Generic;

namespace BH.UI.Dynamo.Methods
{
    public static partial class CRUD
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IEnumerable<object> Pull(BHoMAdapter adapter, object query = null, CustomObject config = null, bool active = false)
        {
            if (query == null)
                query = new FilterQuery();
            else if (!(query is IQuery))
                throw new System.Exception("This component only accepts objects of type IQuery");

            if (active)
            {
                Dictionary<string, object> conf = (config != null) ? config.CustomData : null;
                IEnumerable<object> result = adapter.Pull(query as IQuery, conf);
                return result;
            }
            else
                return new List<object>();
        }

        /***************************************************/
    }
}
