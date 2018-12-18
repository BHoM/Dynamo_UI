using BH.oM.Base;
using BH.oM.DataManipulation.Queries;
using BH.UI.Dynamo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class QueryView : MethodView<QueryNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public QueryView() {}


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type queryType = typeof(IQuery);
            return BH.Engine.Reflection.Query.BHoMMethodList().Where(x => queryType.IsAssignableFrom(x.ReturnType)).OrderBy(x => x.Name);
        }

        /*******************************************/
    }
}
