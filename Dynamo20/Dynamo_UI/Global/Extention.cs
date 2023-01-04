/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using Dynamo.Extensions;
using Dynamo.Wpf.Extensions;
using System.Diagnostics;
using System.Windows.Input;

namespace BH.UI.Dynamo.Global
{
    public class Extension : IViewExtension
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public string Name { get; } = "BHoM Extension";

        public string UniqueId { get; } = Guid.NewGuid().ToString();

        public static string RevitVersion { get; private set; } = "";

        public static DynamoView DynamoWindow { get; private set; } = null;

        public static DynamoModel DynamoModel { get; private set; } = null;


        /*******************************************/
        /**** IExtension Methods                ****/
        /*******************************************/

        public void Startup(ViewStartupParams sp)
        {
            // Get the Revit version i ncase some version need to be treated differently
            string[] revitPath = sp.PathManager.HostApplicationDirectory.Split(new char[] { '/', '\\' });
            RevitVersion = revitPath.Where(x => x.StartsWith("Revit"))
                .Select(x => x.Split(new char[] { ' ', '_' }))
                .Where(x => x.Length == 2)
                .Select(x => x[1])
                .FirstOrDefault();

            // Preload all the BHoM content
            Engine.Base.Compute.LoadAllAssemblies();
            Engine.Base.Query.AllTypeList();
            Engine.Base.Query.AllMethodList();
        }

        /*******************************************/

        public void Loaded(ViewLoadedParams sp)
        {
            DynamoWindow = sp.DynamoWindow as DynamoView;
            if (DynamoWindow != null)
            {
                DynamoWindow.GotFocus += (sender, e) =>
                {
                    Debug.WriteLine("Window got focus");
                    Global.GlobalSearchMenu.Activate(DynamoWindow);

                    DynamoViewModel viewModel = DynamoWindow.DataContext as DynamoViewModel;
                    if (viewModel != null)
                        DynamoModel = viewModel.Model;
                };
            }     
        }

        /*******************************************/

        public void Shutdown()
        {
        }

        /*******************************************/

        public void Dispose()
        {
        }

        /*******************************************/
    }
}




