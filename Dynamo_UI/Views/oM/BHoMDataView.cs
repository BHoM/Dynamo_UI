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
using System.Windows.Controls;

namespace BH.UI.Dynamo.Views
{
    public class BHoMDataView : INodeViewCustomization<BHoMDataNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMDataView()
        {
            m_Dropdown = new ComboBox();
            m_Dropdown.SelectionChanged += M_Dropdown_SelectionChanged;

            if (m_FileTree == null || m_FileList == null)
            {
                List<string> names = Engine.Library.Query.LibraryNames();

                m_FileTree = Engine.Data.Create.Tree(names, names.Select(x => x.Split('\\').ToList()).ToList(), "select a dataset").ShortenBranches();
                m_FileList = names.Select(x => new Tuple<string, string>(x, x)).ToList();
            }
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(BHoMDataNode model, NodeView nodeView)
        {
            m_Node = model;

            // Ad the dropdown list
            nodeView.inputGrid.Children.Add(m_Dropdown);

            // Set up the menu for the user to choose the component type
            if (model.FileName == "")
            {
                SelectorMenu<string> selector = new SelectorMenu<string>(nodeView.MainContextMenu, Item_Click);
                selector.AppendTree(m_FileTree);
                selector.AppendSearchBox(m_FileList);
            }
            else
            {
                m_Dropdown.Items.Clear();
                List<IBHoMObject> objects = Engine.Library.Query.Library(model.FileName);
                foreach (IBHoMObject obj in objects)
                    m_Dropdown.Items.Add(new ComboBoxItem { Content = obj.Name });

                if (model.ItemName != "")
                {
                    foreach(ComboBoxItem item in m_Dropdown.Items)
                    {
                        if (item.Content.ToString() == model.ItemName)
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

        protected virtual void Item_Click(object sender, string fileName)
        {
            m_Node.FileName = fileName;
            if (m_Node.FileName == null)
                return;
            else
            {
                m_Dropdown.Items.Clear();
                List<IBHoMObject> objects = Engine.Library.Query.Library(fileName);
                foreach (IBHoMObject obj in objects)
                    m_Dropdown.Items.Add(new ComboBoxItem { Content = obj.Name });
                m_Dropdown.SelectedIndex = 0;

                m_Node.ItemName = (objects.Count > 0) ? objects[0].Name : "";
                m_Node.MarkNodeAsModified();
            }
        }

        /*******************************************/

        private void M_Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Node.ItemName = ((ComboBoxItem)m_Dropdown.SelectedItem).Content.ToString();
            m_Node.RegisterAllPorts();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected BHoMDataNode m_Node = null;
        protected ComboBox m_Dropdown = new ComboBox();

        protected static Tree<string> m_FileTree = null;
        protected static List<Tuple<string, string>> m_FileList = null;


        /*******************************************/
    }
}
