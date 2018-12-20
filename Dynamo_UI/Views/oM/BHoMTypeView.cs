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

using BH.Engine.DataStructure;
using BH.Engine.Reflection;
using BH.oM.Base;
using BH.oM.DataStructure;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class BHoMTypeView : INodeViewCustomization<BHoMTypeNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMTypeView()
        {
            if (m_TypeTree == null || m_TypeList == null)
            {
                List<Type> types = Engine.Reflection.Query.BHoMTypeList();
                IEnumerable<string> paths = types.Select(x => x.ToText(true));

                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                m_TypeTree = Engine.DataStructure.Create.Tree(types, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y)).ToList()).ToList(), "select a type").ShortenBranches();
                m_TypeList = paths.Zip(types, (k, v) => new Tuple<string, Type>(k, v)).ToList();
            }
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(BHoMTypeNode model, NodeView nodeView)
        {
            m_Node = model;

            // Set up the menu for the user to choose the component type
            if (model.Type == null)
            {
                SelectorMenu<Type> selector = new SelectorMenu<Type>(nodeView.MainContextMenu, Item_Click);
                selector.AppendTree(m_TypeTree);
                selector.AppendSearchBox(m_TypeList);
            }
        }

        /*******************************************/

        public void Dispose() { }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void Item_Click(object sender, Type type)
        {
            m_Node.Type = type;
            if (m_Node.Type == null)
                return;

            m_Node.RegisterAllPorts();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected BHoMTypeNode m_Node = null;
        protected static Tree<Type> m_TypeTree = null;
        protected static List<Tuple<string, Type>> m_TypeList = null;


        /*******************************************/
    }
}
