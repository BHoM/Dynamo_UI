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
        /**** Properties                        ****/
        /*******************************************/

        public override string MethodGroup { get; set; } = "Create";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMObjectView() {}


        /*******************************************/
    }
}
