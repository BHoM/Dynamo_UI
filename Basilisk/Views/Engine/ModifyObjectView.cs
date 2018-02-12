using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class ModifyObjectView : MethodView<ModifyObjectNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ModifyObjectView()
        {
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            return BH.Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == "Modify"); 
        }

        /*******************************************/
    }
}
