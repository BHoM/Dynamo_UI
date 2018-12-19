using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.DataStructure;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Dynamo.Templates;
using System.Windows.Controls;
using System.Linq;
using BH.Engine.DataStructure;
using BH.UI.Templates;
using Dynamo.Engine;

namespace BH.UI.Dynamo.Templates
{
    public abstract class CallerView<T> : INodeViewCustomization<T> where T : CallerComponent
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public CallerView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public virtual void CustomizeView(T component, NodeView nodeView)
        {
            Global.GlobalSearchMenu.DynamoModel = nodeView.ViewModel.DynamoViewModel.Model; //TODO: Find a better way to do this

            m_Node = component;
            m_DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;
            Caller caller = component.Caller;

            if (caller != null)
            {
                caller.AddToMenu(nodeView.MainContextMenu);
                caller.ItemSelected += Caller_ItemSelected;
            }
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void Caller_ItemSelected(object sender, object e)
        {
            m_Node.RefreshComponent();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected CallerComponent m_Node = null;
        protected EngineController m_DynamoEngine = null;

        /*******************************************/
    }
}
