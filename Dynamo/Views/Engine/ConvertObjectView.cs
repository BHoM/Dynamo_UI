using BH.oM.Base;
using BH.UI.Dynamo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
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
