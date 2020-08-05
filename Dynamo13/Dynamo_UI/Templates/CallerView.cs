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

namespace BH.UI.Dynamo.Templates
{
    public abstract class CallerView<T> : INodeViewCustomization<T> where T : CallerComponent
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
            Global.GlobalSearchMenu.DynamoModel = nodeView.ViewModel.DynamoViewModel.Model; //TODO: Find a better way to do this

            m_Node = component;
            m_View = nodeView;
            m_DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;
            Caller caller = component.Caller;

            if (caller != null)
            {
                caller.AddToMenu(nodeView.MainContextMenu);
                caller.Modified += Caller_ItemSelected;
            }
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void Caller_ItemSelected(object sender, object e)
        {
            m_Node.RefreshComponent();

            Caller caller = m_Node.Caller;

            if (caller != null && m_View != null)
                caller.AddToMenu(m_View.MainContextMenu);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected CallerComponent m_Node = null;
        protected NodeView m_View = null;
        protected EngineController m_DynamoEngine = null;

        /*******************************************/
    }
}

