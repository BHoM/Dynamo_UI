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
using BH.UI.Base.Components;
using Dynamo.Controls;
using Dynamo.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Dynamo.ViewModels;
using BH.oM.UI;
using Dynamo.Graph.Nodes;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
        }

        /*******************************************/

        protected override void OnCallerModified(object sender, CallerUpdate update)
        {
            SetButtons();
            base.OnCallerModified(sender, update);
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
                m_ButtonPanel = new UniformGrid { 
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Columns = 2
                };
                m_View.inputGrid.Children.Add(m_ButtonPanel);
            }

            List<ParamInfo> inputs = m_Node.Caller.InputParams.Where(x => x.IsSelected).ToList();
            for (int i = 0; i < inputs.Count(); i++)
            {
                // Add the remove button
                var removeButton = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
                removeButton.Click += RemoveButton_Click;
                m_ButtonPanel.Children.Add(removeButton);

                // Add the edit button
                var editButton = new DynamoNodeButton() { Content = "\u270E", Width = 26, Height = 26 };
                editButton.Click += EditButton_Click;
                m_ButtonPanel.Children.Add(editButton);
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

            // Add the remove button
            var removeButton = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
            removeButton.Click += RemoveButton_Click;
            m_ButtonPanel.Children.Insert(2*inputCount, removeButton );

            // Add the edit button
            var editButton = new DynamoNodeButton() { Content = "\u270E", Width = 26, Height = 26 };
            editButton.Click += EditButton_Click;
            m_ButtonPanel.Children.Insert(2 * inputCount + 1, editButton);

            // Add the input port
            string name = "item" + inputCount;
            caller.AddInput(inputCount, name);
            m_Node.InPorts.Add(new PortModel(PortType.Input, m_Node, new PortData(name, "")));
        }

        /*******************************************/

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DynamoNodeButton button = sender as DynamoNodeButton;
            int index = m_ButtonPanel.Children.IndexOf(button);
            m_ButtonPanel.Children.Remove(button);
            m_ButtonPanel.Children.RemoveAt(index);

            index /= 2;
            CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
            caller.RemoveInput(m_Node.InPorts[index].Name);
            m_Node.InPorts.RemoveAt(index);
        }

        /*******************************************/

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DynamoNodeButton button = sender as DynamoNodeButton;
            int index = m_ButtonPanel.Children.IndexOf(button) / 2;
            PortModel port = m_Node.InPorts[index];

            Window mainWindow = Global.Extension.DynamoWindow;
            Point mousePosition = System.Windows.Input.Mouse.GetPosition(mainWindow);
            var dialog = new Window {
                Title = "Edit Name",
                Height = 150,
                Width = 250,
                ResizeMode = ResizeMode.NoResize,
                Left = mousePosition.X + mainWindow.Left - 125,
                Top = mousePosition.Y + mainWindow.Top - 75
            };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            dialog.Content = grid;

            TextBox inputBox = new TextBox {
                Text = port.Name,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            Grid.SetRow(inputBox, 0);
            Grid.SetColumn(inputBox, 0);
            Grid.SetColumnSpan(inputBox, 2);
            grid.Children.Add(inputBox);

            Button okButton = new Button { Content = "OK", Margin = new Thickness(10) };
            okButton.Click += (s, a) => {
                m_EditedName = inputBox.Text;
                dialog.DialogResult = true;
                dialog.Close();
            };
            Grid.SetRow(okButton, 1);
            Grid.SetColumn(okButton, 0);
            grid.Children.Add(okButton);

            Button cancelButton = new Button { Content = "Cancel", Margin = new Thickness(10) };
            cancelButton.Click += (s, a) => {
                dialog.DialogResult = false;
                dialog.Close();
            };
            Grid.SetRow(cancelButton, 1);
            Grid.SetColumn(cancelButton, 1);
            grid.Children.Add(cancelButton);

            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                // Dynamo doesn't provide functionality to change the name of a port so have to be a bit creative.
                string oldName = m_Node.InPorts[index].Name;
                m_Node.Caller.UpdateInput(index, m_EditedName);
                port.GetType().GetProperty("Name").SetValue(port, m_EditedName);

                // Make sure to also manually update the wpf content of the node
                TextBlock textBlock = FindVisualChildren<TextBlock>(m_View).Where(x => x.Text == oldName).FirstOrDefault();
                if (textBlock != null)
                    textBlock.Text = m_EditedName;

                // Ask the node to refresh
                m_Node.OnNodeModified(true);
                m_View.UpdateLayout();
            }
        }

        /*******************************************/

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected ContextMenu m_Menu;
        protected static string m_EditedName;

        /*******************************************/
    }
}

