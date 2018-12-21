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

using BH.UI.Components;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using BH.UI.Global;
using BH.UI.Templates;
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

namespace BH.UI.Dynamo.Global
{
    public static class GlobalSearchMenu
    {
        public static DynamoModel DynamoModel { get; set; } = null;


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            if (!m_Activated && Application.Current != null)
            {
                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                GlobalSearch.Activate(window);
                GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;
                m_Activated = true;
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            CallerComponent node = null;
            if (request != null && request.CallerType != null && m_CallerComponentDic.ContainsKey(request.CallerType))
                node = Activator.CreateInstance(m_CallerComponentDic[request.CallerType]) as CallerComponent;


            if (node != null && DynamoModel != null)
            {
                CreateNodeCommand command = new CreateNodeCommand(node, 0, 0, true, false);
                DynamoModel.ExecuteCommand(command);
                node.Caller.SetItem(request.SelectedItem);
                node.RefreshComponent();
            }
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static bool m_Activated = false;

        private static Dictionary<Type, Type> m_CallerComponentDic = new Dictionary<Type, Type>
        {
            { typeof(CreateAdapterCaller),      typeof(CreateAdapterComponent) },
            { typeof(CreateQueryCaller),        typeof(CreateQueryComponent) },
            { typeof(DeleteCaller),             typeof(DeleteComponent) },
            { typeof(ExecuteCaller),            typeof(ExecuteComponent) },
            { typeof(MoveCaller),               typeof(MoveComponent) },
            { typeof(PullCaller),               typeof(PullComponent) },
            { typeof(PushCaller),               typeof(PushComponent) },
            { typeof(UpdatePropertyCaller),     typeof(UpdatePropertyComponent) },

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
