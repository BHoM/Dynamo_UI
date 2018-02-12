using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class BHoMObjectView : MethodView<BHoMObjectNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMObjectView()
        {
            MenuLabel = "Select constructor";
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            return BH.Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == "Create");
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
