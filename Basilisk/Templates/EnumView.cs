using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.DataStructure;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Basilisk.Templates;
using System.Windows.Controls;
using System.Linq;
using System.Collections;

namespace BH.UI.Basilisk.Views
{
    public abstract class EnumView<T> : INodeViewCustomization<T> where T : EnumNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public int MenuMaxDepth {get; set; } = 10;

        public int MenuStartingDepth { get; set; } = 2;

        public string MenuLabel { get; set; } = "Select enum";



        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public abstract IEnumerable<Type> GetRelevantTypes();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public EnumView()
        {
            m_Dropdown = new ComboBox();
            m_Dropdown.SelectionChanged += M_Dropdown_SelectionChanged;
            CreateEnumTree();
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(T model, NodeView nodeView)
        {
            m_Node = model;

            // Ad the dropdown list
            nodeView.inputGrid.Children.Add(m_Dropdown);

            // Set up the menu for the user to choose the component type
            if (model.EnumType == null)
                AppendAdditionalComponentMenuItems(nodeView.MainContextMenu);
        }

        /*******************************************/

        public void Dispose()
        {
            
        }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void AppendAdditionalComponentMenuItems(ContextMenu menu)
        {
            if (m_Node.EnumType == null)
            {
                AppendEnumTreeToMenu(m_EnumTree, menu);
                AppendSearchMenu(m_EnumTree, menu);
            }
        }

        /*******************************************/

        protected virtual void CreateEnumTree()
        {
            m_EnumTree = new Tree<Type> { Name = MenuLabel };

            foreach (Type type in GetRelevantTypes())
            {
                try
                {
                    // Make sure the part of the tree corresponding to the namespace exists
                    Tree<Type> tree = m_EnumTree;
                    foreach (string part in type.Namespace.Split('.').Skip(MenuStartingDepth).Take(MenuMaxDepth))
                    {
                        if (!tree.Children.ContainsKey(part))
                            tree.Children.Add(part, new Tree<Type> { Name = part });
                        tree = tree.Children[part];
                    }

                    // Add the methods to the tree
                    AddEnumToTree(tree, new List<string> { type.Name }, type);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /*************************************/

        protected virtual void AddEnumToTree(Tree<Type> tree, IEnumerable<string> path, Type type)
        {
            Dictionary<string, Tree<Type>> children = tree.Children;

            if (path.Count() == 0)
            {
                string name = type.Name;
                bool isIMethod = (name.Length > 1 && Char.IsUpper(name[1]));

                if (isIMethod)
                    name = name.Substring(1);

                if (isIMethod || !children.ContainsKey(name))
                    children[name] = new Tree<Type> { Value = type, Name = type.Name };
            }
            else
            {
                string name = path.First();
                if (!children.ContainsKey(name))
                    children.Add(name, new Tree<Type> { Name = name });
                AddEnumToTree(children[name], path.Skip(1), type);
            }
        }


        /*******************************************/

        protected virtual void AppendEnumTreeToMenu(Tree<Type> tree, ContextMenu menu)
        {
            MenuItem treeMenu = new MenuItem { Header = tree.Name };
            menu.Items.Add(treeMenu);
            foreach (Tree<Type> childTree in tree.Children.Values.OrderBy(x => x.Name))
                AppendEnumTreeToMenu(childTree, treeMenu);
        }

        /*******************************************/

        protected virtual void AppendEnumTreeToMenu(Tree<Type> tree, MenuItem menu)
        {
            if (tree.Children.Count == 0 || (tree.Children.Count == 1 && tree.Children.Values.First().Children.Count == 0))
            {
                MenuItem methodItem = new MenuItem { Header = tree.Name };
                methodItem.Click += Item_Click;
                menu.Items.Add(methodItem);
                if (tree.Children.Count == 0)
                    m_TypeLinks[methodItem] = tree.Value;
                else
                    m_TypeLinks[methodItem] = tree.Children.Values.First().Value;
            }
            else
            {
                MenuItem treeMenu = new MenuItem { Header = tree.Name };
                menu.Items.Add(treeMenu);
                foreach (Tree<Type> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendEnumTreeToMenu(childTree, treeMenu);
            }
        }

        /*******************************************/

        protected virtual void Item_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (!m_TypeLinks.ContainsKey(item))
                return;

            m_Node.EnumType = m_TypeLinks[item];
            if (m_Node.EnumType == null)
                return;
            else
            {
                m_Dropdown.Items.Clear();
                Array values = Enum.GetValues(m_Node.EnumType);
                IEnumerator enumerator = values.GetEnumerator();
                while (enumerator.MoveNext())
                    m_Dropdown.Items.Add(new ComboBoxItem { Content = enumerator.Current.ToString()  });
                m_Dropdown.SelectedIndex = 0;


                m_Node.EnumValue = Enum.GetValues(m_Node.EnumType).GetValue(0).ToString();
                m_Node.MarkNodeAsModified();
            }
                
        }

        /*******************************************/


        private void M_Dropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_Node.EnumValue = ((ComboBoxItem)m_Dropdown.SelectedItem).Content.ToString();
            m_Node.RegisterAllPorts();
        }

        /*******************************************/

        protected virtual void AppendSearchMenu(Tree<Type> methods, ContextMenu menu)
        {
            m_Menu = menu;
            m_MethodList = GetMethodList(methods);

            MenuItem label = new MenuItem { Header = "Search", IsCheckable = false };
            menu.Items.Add(label);

            TextBox textBox = new TextBox { Text = "" };
            textBox.TextChanged += Search_TextChanged;
            menu.Items.Add(textBox);

            m_Menu.Items.Add(new Separator());
        }

        /*******************************************/

        protected virtual void Search_TextChanged(object sender, EventArgs e)
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
            foreach (Tree<Type> tree in m_MethodList.Where(x => parts.All(y => x.Name.ToLower().Contains(y))).Take(12).OrderBy(x => x.Name))
            {
                MenuItem methodItem = new MenuItem { Header = tree.Name };
                methodItem.Click += Item_Click;
                m_Menu.Items.Add(methodItem);
                m_SearchResultItems.Add(methodItem);
                m_TypeLinks[methodItem] = tree.Value;
            }
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<Type>> GetMethodList(Tree<Type> tree)
        {
            return tree.Children.Values.SelectMany(x => GetMethodList(x, ""));
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<Type>> GetMethodList(Tree<Type> tree, string path)
        {
            if (path.Length > 0 && !tree.Name.StartsWith("("))
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<Type>[] { new Tree<Type> { Value = tree.Value, Name = path + tree.Name } };
            else
                return tree.Children.Values.SelectMany(x => GetMethodList(x, path + tree.Name));
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected Tree<Type> m_EnumTree;
        protected T m_Node = null;
        protected ComboBox m_Dropdown = new ComboBox();
        protected Dictionary<MenuItem, Type> m_TypeLinks = new Dictionary<MenuItem, Type>();
        private IEnumerable<Tree<Type>> m_MethodList = new List<Tree<Type>>();
        protected List<MenuItem> m_SearchResultItems = new List<MenuItem>();
        private ContextMenu m_Menu;


        /*******************************************/
    }
}
