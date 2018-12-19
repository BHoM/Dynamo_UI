using BH.oM.Base;
using BH.UI.Dynamo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
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
