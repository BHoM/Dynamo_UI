using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class ConvertObjectView : MethodView<ConvertObjectNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ConvertObjectView()
        {
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            return BH.Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == "Convert"); 
        }

        /*******************************************/
    }
}
