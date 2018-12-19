using BH.oM.Base;
using BH.UI.Dynamo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class ModifyObjectView : MethodView<ModifyObjectNode> 
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string MethodGroup { get; set; } = "Modify";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ModifyObjectView() {}


        /*******************************************/
    }
}
