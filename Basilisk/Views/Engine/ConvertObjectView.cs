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
        /**** Properties                        ****/
        /*******************************************/

        public override string MethodGroup { get; set; } = "Convert";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ConvertObjectView() {}


        /*******************************************/
    }
}
