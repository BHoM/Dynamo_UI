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
using Dynamo.Engine;
using Dynamo.Configuration;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls.Primitives;

namespace BH.UI.Basilisk.Templates
{
    public abstract class ValueListView<T> : INodeViewCustomization<T> where T : CallerValueList
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ValueListView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public virtual void CustomizeView(T component, NodeView nodeView)
        {
            m_Node = component;
            m_DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;
            Caller caller = component.Caller;

            if (caller != null)
            {
                caller.AddToMenu(nodeView.MainContextMenu);
                caller.ItemSelected += Caller_ItemSelected;
            }

            m_Combo = new ComboBox
            {
                Width = System.Double.NaN,
                MinWidth = 100,
                Height = Configurations.PortHeightInPixels,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(m_Combo, 0);
            Grid.SetRow(m_Combo, 0);
            nodeView.inputGrid.Children.Add(m_Combo);

            foreach (string name in m_Node.Caller.GetChoiceNames())
                m_Combo.Items.Add(name);
            if (m_Node.SelectedIndex > 0)
                m_Combo.SelectedIndex = m_Node.SelectedIndex;

            m_Combo.SelectionChanged += M_Combo_SelectionChanged;
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void Caller_ItemSelected(object sender, object e)
        {
            m_Combo.Items.Clear();
            foreach (string name in m_Node.Caller.GetChoiceNames())
                m_Combo.Items.Add(name);
            m_Combo.SelectedIndex = 0;
            m_Node.RefreshComponent();
        }

        /*******************************************/

        private void M_Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Node.SelectedIndex = ((ComboBox)sender).SelectedIndex;
            m_Node.RefreshComponent();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected CallerValueList m_Node = null;
        protected EngineController m_DynamoEngine = null;
        protected ComboBox m_Combo = null;

        /*******************************************/
    }
}
