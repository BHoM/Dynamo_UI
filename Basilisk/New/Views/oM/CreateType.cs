using BH.oM.Base;
using BH.UI.Basilisk.Components;
using BH.UI.Basilisk.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
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
