using BH.oM.Base;
using BH.UI.Dynamo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class QueryObjectView : MethodView<QueryObjectNode> 
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public override string MethodGroup { get; set; } = "Query";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public QueryObjectView() {}


        /*******************************************/
    }
}
