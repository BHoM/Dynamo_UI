using BH.Adapter;
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

        public AdapterView() { }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<MethodBase> GetRelevantMethods()
        {
            Type adapterType = typeof(BHoMAdapter);
            return BH.Engine.Reflection.Query.AdapterTypeList().Where(x => x != null && x.IsSubclassOf(adapterType)).OrderBy(x => x.Name).SelectMany(x => x.GetConstructors());
        }

        /*******************************************/
    }
}
