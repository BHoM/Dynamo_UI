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

using BH.oM.Base;
using BH.oM.UI;
using BH.UI.Components;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using Dynamo.Controls;
using Dynamo.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BH.UI.Dynamo.Views
{
    public class CreateObjectView : CallerView<CreateObjectComponent> 
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public override void CustomizeView(CreateObjectComponent nodeModel, NodeView nodeView)
        {
            base.CustomizeView(nodeModel, nodeView);

            m_View = nodeView;

            CreateObjectCaller caller = nodeModel.Caller as CreateObjectCaller;
            if (caller != null)
            {
                if (caller.SelectedItem is Type)
                    SetButtons();

                caller.InputToggled += Caller_InputToggled;
            }   
        }

        /*******************************************/

        protected override void Caller_ItemSelected(object sender, object e)
        {
            CreateObjectCaller caller = m_Node.Caller as CreateObjectCaller;
            if (caller.SelectedItem is Type)
                SetButtons();

            base.Caller_ItemSelected(sender, e);
        }

        /*******************************************/

        private void Caller_InputToggled(object sender, Tuple<ParamInfo, bool> e)
        {
            CreateObjectCaller caller = m_Node.Caller as CreateObjectCaller;
            if (caller.SelectedItem is Type)
                SetButtons();
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void SetButtons() 
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
        }

        /*******************************************/

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DynamoNodeButton button = sender as DynamoNodeButton;
            int index = m_ButtonPanel.Children.IndexOf(button);
            m_ButtonPanel.Children.Remove(button);

            CreateObjectCaller caller = m_Node.Caller as CreateObjectCaller;
            List<string> inputs = m_Node.InPorts.Select(x => x.Name).ToList();
            caller.RemoveInput(inputs[index]);
            inputs.RemoveAt(index);
            m_Node.RefreshComponent();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected WrapPanel m_ButtonPanel = null;

        /*******************************************/
    }
}
