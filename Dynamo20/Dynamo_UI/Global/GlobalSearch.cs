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

using BH.UI.Base.Components;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using BH.UI.Base.Global;
using BH.UI.Base;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Dynamo.Models.DynamoModel;
using System.Diagnostics;

namespace BH.UI.Dynamo.Global
{
    public static class GlobalSearchMenu
    {
        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate(DynamoView window)
        {
            if (window != null)
            {
                Debug.WriteLine("Trying to activate global menu for a window");

                try
                {
                    // Get the Dynamo model
                    DynamoViewModel viewModel = window.DataContext as DynamoViewModel;
                    if (viewModel == null)
                        return;

                    if (!m_ActiveWindows.Contains(window))
                    {
                        // Activate the global menu
                        GlobalSearch.Activate(window);
                        GlobalSearch.ItemSelected += (sender, request) => GlobalSearch_ItemSelected(viewModel.Model, request);
                        m_ActiveWindows.Add(window);

                        Debug.WriteLine("Global menu activated for new window");
                    }
                    else
                    {
                        Debug.WriteLine("Global menu already activated for that window");
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine("Error on global menu activation: " + e.Message);
                }
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static void GlobalSearch_ItemSelected(DynamoModel dynamoModel, oM.UI.ComponentRequest request)
        {
            CallerComponent node = null;
            if (request != null && request.CallerType != null && m_CallerComponentDic.ContainsKey(request.CallerType))
                node = Activator.CreateInstance(m_CallerComponentDic[request.CallerType]) as CallerComponent;


            if (node != null && dynamoModel != null)
            {
                CreateNodeCommand command = new CreateNodeCommand(node, 0, 0, true, false);
                dynamoModel.ExecuteCommand(command);
                node.Caller.SetItem(request.SelectedItem);
            }
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static List<DynamoView> m_ActiveWindows = new List<DynamoView>();

        private static Dictionary<Type, Type> m_CallerComponentDic = new Dictionary<Type, Type>
        {
            { typeof(CreateAdapterCaller),      typeof(CreateAdapterComponent) },
            { typeof(CreateRequestCaller),        typeof(CreateRequestComponent) },
            { typeof(RemoveCaller),             typeof(RemoveComponent) },
            { typeof(ExecuteCaller),            typeof(ExecuteComponent) },
            { typeof(MoveCaller),               typeof(MoveComponent) },
            { typeof(PullCaller),               typeof(PullComponent) },
            { typeof(PushCaller),               typeof(PushComponent) },

            { typeof(ComputeCaller),            typeof(ComputeComponent) },
            { typeof(ConvertCaller),            typeof(ConvertComponent) },
            { typeof(ExplodeCaller),            typeof(ExplodeComponent) },
            { typeof(FromJsonCaller),           typeof(FromJsonComponent) },
            { typeof(GetInfoCaller),            typeof(GetInfoComponent) },
            { typeof(GetPropertyCaller),        typeof(GetPropertyComponent) },
            { typeof(ModifyCaller),             typeof(ModifyComponent) },
            { typeof(QueryCaller),              typeof(QueryComponent) },
            { typeof(SetPropertyCaller),        typeof(SetPropertyComponent) },
            { typeof(ToJsonCaller),             typeof(ToJsonComponent) },

            { typeof(CreateCustomCaller),       typeof(CreateCustomComponent) },
            { typeof(CreateDataCaller),         typeof(CreateDataComponent) },
            { typeof(CreateDictionaryCaller),   typeof(CreateDictionaryComponent) },
            { typeof(CreateEnumCaller),         typeof(CreateEnumComponent) },
            { typeof(CreateObjectCaller),       typeof(CreateObjectComponent) },
            { typeof(CreateTypeCaller),         typeof(CreateTypeComponent) }
        };

        /*******************************************/
    }
}

