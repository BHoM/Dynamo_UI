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

using BH.Engine.Data;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.Data.Collections;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace BH.UI.Dynamo.Views
{
    public class BHoMEnumView : INodeViewCustomization<BHoMEnumNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMEnumView()
        {
            m_Dropdown = new ComboBox();
            m_Dropdown.SelectionChanged += M_Dropdown_SelectionChanged;

            if (m_TypeTree == null || m_TypeList == null)
            {
                List<Type> types = Engine.Reflection.Query.BHoMEnumList();
                IEnumerable<string> paths = types.Select(x => x.ToText(true));

                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                m_TypeTree = Engine.Data.Create.Tree(types, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y)).ToList()).ToList(), "select a type").ShortenBranches();
                m_TypeList = paths.Zip(types, (k, v) => new Tuple<string, Type>(k, v)).ToList();
            }
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(BHoMEnumNode model, NodeView nodeView)
        {
            m_Node = model;

            // Ad the dropdown list
            nodeView.inputGrid.Children.Add(m_Dropdown);

            // Set up the menu for the user to choose the component type
            if (model.EnumType == null)
            {
                SelectorMenu<Type> selector = new SelectorMenu<Type>(nodeView.MainContextMenu, Item_Click);
                selector.AppendTree(m_TypeTree);
                selector.AppendSearchBox(m_TypeList);
            }
            else
            {
                m_Dropdown.Items.Clear();
                Array values = Enum.GetValues(m_Node.EnumType);
                IEnumerator enumerator = values.GetEnumerator();
                while (enumerator.MoveNext())
                    m_Dropdown.Items.Add(new ComboBoxItem { Content = enumerator.Current.ToString() });

                if (model.EnumValue != "")
                {
                    foreach (ComboBoxItem item in m_Dropdown.Items)
                    {
                        if (item.Content.ToString() == model.EnumValue)
                        {
                            m_Dropdown.SelectedItem = item;
                            break;
                        } 
                    }
                }
            }
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void Item_Click(object sender, Type type)
        {
            m_Node.EnumType = type;
            if (m_Node.EnumType == null)
                return;
            else
            {
                m_Dropdown.Items.Clear();
                Array values = Enum.GetValues(m_Node.EnumType);
                IEnumerator enumerator = values.GetEnumerator();
                while (enumerator.MoveNext())
                    m_Dropdown.Items.Add(new ComboBoxItem { Content = enumerator.Current.ToString() });
                m_Dropdown.SelectedIndex = 0;

                m_Node.EnumValue = Enum.GetValues(m_Node.EnumType).GetValue(0).ToString();
                m_Node.MarkNodeAsModified();
            }
        }

        /*******************************************/

        private void M_Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Node.EnumValue = ((ComboBoxItem)m_Dropdown.SelectedItem).Content.ToString();
            m_Node.RegisterAllPorts();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected BHoMEnumNode m_Node = null;
        protected ComboBox m_Dropdown = new ComboBox();

        protected static Tree<Type> m_TypeTree = null;
        protected static List<Tuple<string, Type>> m_TypeList = null;


        /*******************************************/
    }
}
