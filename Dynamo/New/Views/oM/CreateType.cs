using BH.oM.Base;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class CreateTypeView : CallerView<CreateTypeComponent> 
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        protected override void Caller_ItemSelected(object sender, object e)
        {
            base.Caller_ItemSelected(sender, e);
            m_Node.MarkNodeAsModified(true);
            m_Node.OnNodeModified(true);
        }

        /*******************************************/
    }
}
