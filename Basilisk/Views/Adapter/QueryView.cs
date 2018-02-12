using BH.oM.Base;
using BH.oM.Queries;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class QueryView : MethodView<QueryNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public QueryView()
        {
            MenuLabel = "Select query";
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type queryType = typeof(IQuery);
            return BH.Engine.Reflection.Query.BHoMTypeList().Where(x => queryType.IsAssignableFrom(x)).SelectMany(x => x.GetConstructors());
        }

        /*******************************************/

        protected override IEnumerable<string> GetMethodPath(MethodBase method)
        {

            if (method is MethodInfo)
                return new List<string> { ((MethodInfo)method).ReturnType.Name };
            else if (method is ConstructorInfo)
                return new List<string> { ((ConstructorInfo)method).DeclaringType.Name };
            else
                return new List<string>();
        }

        /*******************************************/

        protected virtual Type GetPathType(MethodInfo method)
        {
            return method.ReturnType;
        }

        /*******************************************/
    }
}
