/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
using Dynamo.Configuration;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls.Primitives;

namespace BH.UI.Dynamo.Templates
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
