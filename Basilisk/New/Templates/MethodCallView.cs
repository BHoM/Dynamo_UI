using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.DataStructure;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Basilisk.Templates;
using System.Windows.Controls;
using System.Linq;
using BH.Engine.DataStructure;
using BH.UI.Templates;

namespace BH.UI.Basilisk.Templates
{
    public abstract class MethodCallView<T> : INodeViewCustomization<T> where T : MethodCallComponent
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodCallView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(T component, NodeView nodeView)
        {
            m_Node = component;
            Caller caller = component.MethodCaller;

            if (caller != null)
            {
                caller.Selector.AddToMenu(nodeView.MainContextMenu);
                caller.ItemSelected += Caller_MethodSelected;
            }
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected void Caller_MethodSelected(object sender, object e)
        {
            m_Node.RefreshtMethod();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected MethodCallComponent m_Node = null;


        /*******************************************/
    }
}
