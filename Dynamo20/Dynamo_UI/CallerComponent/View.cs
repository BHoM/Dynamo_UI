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

using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.Data.Collections;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Dynamo.Templates;
using System.Windows.Controls;
using System.Linq;
using BH.Engine.Data;
using BH.UI.Base;
using Dynamo.Engine;
using BH.oM.UI;
using System.Collections.ObjectModel;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;
using BH.Engine.Dynamo.Objects;

namespace BH.UI.Dynamo.Templates
{
    public abstract partial class CallerView<T> : INodeViewCustomization<T> where T : CallerComponent
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
            m_Node = component;
            m_View = nodeView;
            DataAccessor_Dynamo.DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;

            if (component != null)
            {
                component.ModifiedByCaller += OnCallerModified;

                // Add the menu
                if (component.Caller != null)
                {
                    component.Caller.AddToMenu(nodeView.MainContextMenu);
                    RemoveOutputSelectionMenu(nodeView.MainContextMenu);
                }      
            }   
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected virtual void OnCallerModified(object sender, CallerUpdate update)
        {
            if (update == null)
                return;

            // Update the menu
            if (update.Cause == CallerUpdateCause.ItemSelected || update.Cause == CallerUpdateCause.ReadFromSave)
            {
                Caller caller = m_Node.Caller;
                if (caller != null && m_View != null)
                {
                    caller.AddToMenu(m_View.MainContextMenu);
                    RemoveOutputSelectionMenu(m_View.MainContextMenu);
                }    
            }

            // Dynamo NodeViewModel is bugged and will always add items added to NodeModel.InPorts at the end of its list regardless of where they really 
            // So we need to fix that ourselves
            if (m_Node != null && m_View != null)
            {
                FixPortOrder(m_Node.InPorts, m_View.ViewModel.InPorts);
                FixPortOrder(m_Node.OutPorts, m_View.ViewModel.OutPorts);
                m_View.UpdateLayout();
            }
 
        }

        /*******************************************/

        protected virtual void FixPortOrder(ObservableCollection<PortModel> nodePorts, ObservableCollection<PortViewModel> viewPorts)
        {
            List<string> names = nodePorts.Select(x => x.Name).ToList();

            for (int i = 0; i < viewPorts.Count; i++)
            {
                string name = viewPorts[i].PortName;
                int index = names.FindIndex(x => x == name);
                if (index != i)
                    viewPorts.Move(i, index);
            }
        }

        /*******************************************/

        protected virtual void RemoveOutputSelectionMenu(ContextMenu menu)
        {
            // Dynamo is too buggy when it comes to dynamically changing the output params so we disable that functionality
            // See this issue on GitHub that was closed withotu being resolved: https://github.com/DynamoDS/Dynamo/issues/8912
            MenuItem outputSelector = menu.Items.OfType<MenuItem>().Where(x => x.Header.ToString() == "Add/Remove Outputs").FirstOrDefault();
            if (outputSelector != null)
                menu.Items.Remove(outputSelector);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected CallerComponent m_Node = null;
        protected NodeView m_View = null;

        /*******************************************/
    }
}

