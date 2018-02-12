using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class AdapterView : MethodView<AdapterNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public AdapterView()
        {
            MenuLabel = "Select adapter";
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type adapterType = typeof(Adapter.BHoMAdapter);
            return BH.Engine.Reflection.Query.AdapterTypeList().Where(x => x.IsSubclassOf(adapterType)).SelectMany(x => x.GetConstructors());
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
