/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.Base;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using BH.UI.Components;
using Dynamo.Controls;
using Dynamo.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Dynamo.ViewModels;

namespace BH.UI.Dynamo.Views
{
    public class CreateCustomView : CallerView<CreateCustomComponent> 
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public override void CustomizeView(CreateCustomComponent nodeModel, NodeView nodeView)
        {
            base.CustomizeView(nodeModel, nodeView);

            m_View = nodeView;
            SetButtons();
            AppendInputNameMenu(true);

            nodeModel.InPorts.CollectionChanged += InPorts_CollectionChanged;
        }

        /*******************************************/

        protected override void Caller_ItemSelected(object sender, object e)
        {
            SetButtons();
            base.Caller_ItemSelected(sender, e);
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void SetButtons() //TODO: Consider having one pair of buttons per input to allow more flexible changes
        {
            if (m_ButtonPanel != null)
                m_ButtonPanel.Children.Clear();
            else
            {
                m_ButtonPanel = new WrapPanel
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Orientation = Orientation.Vertical
                };
                m_View.inputGrid.Children.Add(m_ButtonPanel);
            }

            for (int i = 0; i < m_Node.Caller.InputParams.Count; i++)
            {
                var button = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
                button.Click += RemoveButton_Click;
                m_ButtonPanel.Children.Add(button);
            }
            
            var addButton = new DynamoNodeButton() { Content = "+", Width = 26, Height = 26 };
            addButton.Click += AddButton_Click;
            m_ButtonPanel.Children.Add(addButton);
        }

        /*******************************************/

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
            int inputCount = m_Node.InPorts.Count;
            
            var button = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
            button.Click += RemoveButton_Click;
            m_ButtonPanel.Children.Insert(inputCount, button );

            caller.AddInput(inputCount, "item" + inputCount);
            m_Node.RefreshComponent();

            var a = m_View.ContentGrid.Children;
        }

        /*******************************************/

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DynamoNodeButton button = sender as DynamoNodeButton;
            int index = m_ButtonPanel.Children.IndexOf(button);
            m_ButtonPanel.Children.Remove(button);

            CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
            caller.RemoveInput(m_Node.InPorts[index].Name);
            m_Node.RefreshComponent();
        }

        /*******************************************/

        protected void AppendInputNameMenu(bool firstTime = false)
        {
            if (firstTime)
            {
                m_Menu = m_View.MainContextMenu;
                m_Menu.Items.Add(new Separator());

                MenuItem label = new MenuItem { Header = "Input Names", IsCheckable = false };
                m_Menu.Items.Add(label);
            }

            for (int i = m_Node.InPorts.Count; i < m_InputNameItems.Count; i++)
                m_Menu.Items.Remove(m_InputNameItems[i]);

            for (int i = m_InputNameItems.Count; i < m_Node.InPorts.Count; i++)
            {
                TextBox textBox = new TextBox { Text = m_Node.InPorts[i].Name };
                textBox.TextChanged += TextBox_TextChanged;
                m_Menu.Items.Add(textBox);
                m_InputNameItems.Add(textBox);
            }
        }

        /*******************************************/

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_InputsNeedRefreshing = true;
        }

        /*******************************************/

        private void InPorts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AppendInputNameMenu();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected WrapPanel m_ButtonPanel = null;
        protected List<TextBox> m_InputNameItems = new List<TextBox>();
        protected ContextMenu m_Menu;
        protected bool m_InputsNeedRefreshing = false;

        /*******************************************/
    }
}

