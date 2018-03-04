using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class BHoMTypeView : TypeView<BHoMTypeNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMTypeView()
        {
            MenuLabel = "Select type";
        }


        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public override IEnumerable<Type> GetRelevantTypes()
        {
            return BH.Engine.Reflection.Query.BHoMTypeList();
        }


        /*******************************************/
    }
}
