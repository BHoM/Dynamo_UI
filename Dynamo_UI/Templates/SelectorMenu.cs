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

using BH.oM.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BH.UI.Dynamo.Templates
{
    public class SelectorMenu<T>
    {
        /*************************************/
        /**** Public Events               ****/
        /*************************************/

        public event EventHandler<T> ItemSelected;


        /*************************************/
        /**** Constructors                ****/
        /*************************************/

        public SelectorMenu(ContextMenu menu, EventHandler<T> callback)
        {
            ItemSelected += callback;
            m_Menu = menu;
        }


        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public void AppendTree(Tree<T> tree)
        {
            m_Menu.Items.Add(new Separator());

            MenuItem treeMenu = new MenuItem { Header = tree.Name };
            m_Menu.Items.Add(treeMenu);
            foreach (Tree<T> childTree in tree.Children.Values.OrderBy(x => x.Name))
                AppendMenuTree(childTree, treeMenu);
        }

        /*************************************/

        public void AppendSearchBox(List<Tuple<string, T>> itemList)
        {
            m_ItemList = itemList;

            m_Menu.Items.Add(new Separator());

            MenuItem label = CreateMenuItem("Search");
            label.FontWeight = FontWeights.Bold;
            m_Menu.Items.Add(label);

            m_SearchBox = new TextBox { Text = "" };
            m_SearchBox.TextChanged += Search_TextChanged;
            m_Menu.Items.Add(m_SearchBox);

            m_Menu.Items.Add(new Separator());
        }


        /*************************************/
        /**** Protected Methods           ****/
        /*************************************/

        protected void AppendMenuTree(Tree<T> tree, MenuItem menu)
        {
            if (tree.Children.Count > 0)
            {
                MenuItem treeMenu = CreateMenuItem(tree.Name);
                menu.Items.Add(treeMenu);
                foreach (Tree<T> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendMenuTree(childTree, treeMenu);
            }
            else
            {
                T method = tree.Value;
                MenuItem methodItem = CreateMenuItem(tree.Name, Item_Click);
                menu.Items.Add(methodItem);
                m_ItemLinks[methodItem] = tree.Value;
            }
        }

        /*************************************/

        protected MenuItem CreateMenuItem(string text, EventHandler click = null, bool enabled = true, bool @checked = false)
        {
            MenuItem item = new MenuItem { Header = text, IsCheckable = @checked };
            if (click != null)
                item.Click += Item_Click;

            return item;
        }

        /*************************************/

        protected void Item_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (!m_ItemLinks.ContainsKey(item))
                return;

            if (ItemSelected != null)
                ItemSelected(this, m_ItemLinks[item]);
        }

        /*************************************/

        protected void Search_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box == null) return;

            // Clear the old items
            foreach (MenuItem item in m_SearchResultItems)
                m_Menu.Items.Remove(item);
            m_SearchResultItems.Clear();

            // Add the new ones
            string text = box.Text.ToLower();
            string[] parts = text.Split(' ');
            foreach (Tuple<string, T> tree in m_ItemList.Where(x => parts.All(y => x.Item1.ToLower().Contains(y))).Take(12).OrderBy(x => x.Item1))
            {
                MenuItem methodItem = CreateMenuItem(tree.Item1, Item_Click);
                m_Menu.Items.Add(methodItem);
                m_SearchResultItems.Add(methodItem);
                m_ItemLinks[methodItem] = tree.Item2;
            }
        }


        /*************************************/
        /**** Protected Fields            ****/
        /*************************************/

        protected ContextMenu m_Menu;
        protected TextBox m_SearchBox;
        protected List<Tuple<string, T>> m_ItemList = new List<Tuple<string, T>>();
        protected Dictionary<MenuItem, T> m_ItemLinks = new Dictionary<MenuItem, T>();
        protected List<MenuItem> m_SearchResultItems = new List<MenuItem>();


        /*************************************/
    }
}
