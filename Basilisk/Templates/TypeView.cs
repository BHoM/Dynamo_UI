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
    public abstract class TypeView<T> : INodeViewCustomization<T> where T : TypeNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public int MenuMaxDepth {get; set; } = 10;

        public int MenuStartingDepth { get; set; } = 2;

        public string MenuLabel { get; set; } = "Select type";



        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public abstract IEnumerable<Type> GetRelevantTypes();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public TypeView()
        {
            CreateTypeTree();
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(T model, NodeView nodeView)
        {
            m_Node = model;

            // Set up the menu for the user to choose the component type
            if (model.Type == null)
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
            if (m_Node.Type == null)
            {
                AppendTypeTreeToMenu(m_TypeTree, menu);
                AppendSearchMenu(m_TypeTree, menu);
            }
        }

        /*******************************************/

        protected virtual void CreateTypeTree()
        {
            m_TypeTree = new Tree<Type> { Name = MenuLabel };

            foreach (Type type in GetRelevantTypes())
            {
                try
                {
                    // Make sure the part of the tree corresponding to the namespace exists
                    Tree<Type> tree = m_TypeTree;
                    foreach (string part in type.Namespace.Split('.').Skip(MenuStartingDepth).Take(MenuMaxDepth))
                    {
                        if (!tree.Children.ContainsKey(part))
                            tree.Children.Add(part, new Tree<Type> { Name = part });
                        tree = tree.Children[part];
                    }

                    // Add the methods to the tree
                    AddTypeToTree(tree, new List<string> { type.Name }, type);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /*************************************/

        protected virtual void AddTypeToTree(Tree<Type> tree, IEnumerable<string> path, Type type)
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
                AddTypeToTree(children[name], path.Skip(1), type);
            }
        }


        /*******************************************/

        protected virtual void AppendTypeTreeToMenu(Tree<Type> tree, ContextMenu menu)
        {
            MenuItem treeMenu = new MenuItem { Header = tree.Name };
            menu.Items.Add(treeMenu);
            foreach (Tree<Type> childTree in tree.Children.Values.OrderBy(x => x.Name))
                AppendTypeTreeToMenu(childTree, treeMenu);
        }

        /*******************************************/

        protected virtual void AppendTypeTreeToMenu(Tree<Type> tree, MenuItem menu)
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
                    AppendTypeTreeToMenu(childTree, treeMenu);
            }
        }

        /*******************************************/

        protected virtual void Item_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (!m_TypeLinks.ContainsKey(item))
                return;

            m_Node.Type = m_TypeLinks[item];
            m_Node.RegisterAllPorts();  
        }

        /*******************************************/

        protected virtual void AppendSearchMenu(Tree<Type> methods, ContextMenu menu)
        {
            m_Menu = menu;
            m_TypeList = GetTypeList(methods);

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
            foreach (Tree<Type> tree in m_TypeList.Where(x => parts.All(y => x.Name.ToLower().Contains(y))).Take(12).OrderBy(x => x.Name))
            {
                MenuItem methodItem = new MenuItem { Header = tree.Name };
                methodItem.Click += Item_Click;
                m_Menu.Items.Add(methodItem);
                m_SearchResultItems.Add(methodItem);
                m_TypeLinks[methodItem] = tree.Value;
            }
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree)
        {
            return tree.Children.Values.SelectMany(x => GetTypeList(x, ""));
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<Type>> GetTypeList(Tree<Type> tree, string path)
        {
            if (path.Length > 0 && !tree.Name.StartsWith("("))
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<Type>[] { new Tree<Type> { Value = tree.Value, Name = path + tree.Name } };
            else
                return tree.Children.Values.SelectMany(x => GetTypeList(x, path + tree.Name));
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected Tree<Type> m_TypeTree;
        protected T m_Node = null;
        protected ComboBox m_Dropdown = new ComboBox();
        protected Dictionary<MenuItem, Type> m_TypeLinks = new Dictionary<MenuItem, Type>();
        private IEnumerable<Tree<Type>> m_TypeList = new List<Tree<Type>>();
        protected List<MenuItem> m_SearchResultItems = new List<MenuItem>();
        private ContextMenu m_Menu;


        /*******************************************/
    }
}
