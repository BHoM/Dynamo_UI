using BH.oM.Base;
using BH.UI.Basilisk.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
{
    public class ComputeObjectView : MethodView<ComputeObjectNode> 
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string MethodGroup { get; set; } = "Compute";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ComputeObjectView() {}

        /*******************************************/
    }
}
