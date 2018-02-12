using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.DataStructure;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Basilisk.Templates;
using System.Windows.Controls;
using System.Linq;

namespace BH.UI.Basilisk.Views
{
    public abstract class MethodView<T> : INodeViewCustomization<T> where T : MethodNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public int MenuMaxDepth {get; set; } = 10;

        public int MenuStartingDepth { get; set; } = 2;

        public string MenuLabel { get; set; } = "Select method";



        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public abstract IEnumerable<MethodBase> GetRelevantMethods();


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodView()
        {
            CreateMethodTree();
        }


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(T model, NodeView nodeView)
        {
            m_Node = model;

            // Set up the menu for the user to choose the component type
            if (model.Method == null)
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
            if (m_Node.Method == null)
            {
                AppendMethodTreeToMenu(m_MethodTree, menu);
                AppendSearchMenu(m_MethodTree, menu);
            }
        }

        /*******************************************/

        protected virtual void CreateMethodTree()
        {
            m_MethodTree = new Tree<MethodBase> { Name = MenuLabel };

            foreach (IGrouping<Type, MethodBase> group in GetRelevantMethods().GroupBy(GetPathType))
            {
                Type type = group.Key;
                List<MethodBase> methods = group.ToList();

                try
                {
                    // Make sure the part of the tree corresponding to the namespace exists
                    Tree<MethodBase> tree = m_MethodTree;
                    foreach (string part in type.Namespace.Split('.').Skip(MenuStartingDepth).Take(MenuMaxDepth))
                    {
                        if (!tree.Children.ContainsKey(part))
                            tree.Children.Add(part, new Tree<MethodBase> { Name = part });
                        tree = tree.Children[part];
                    }

                    // Add the methods to the tree
                    foreach (MethodBase method in methods)
                        AddMethodToTree(tree, GetMethodPath(method), method);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        /*******************************************/

        protected virtual Type GetPathType(MethodBase method)
        {
            return method.DeclaringType;
        }


        /*******************************************/

        protected virtual IEnumerable<string> GetMethodPath(MethodBase method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            string typeName = "Global";
            if (parameters.Length > 0)
            {
                Type type = parameters[0].ParameterType;
                if (type.IsGenericType)
                    type = type.GenericTypeArguments.First();
                typeName = type.Name;
            }

            return new List<string> { typeName, method.Name };
        }


        /*******************************************/

        protected virtual string GetMethodString(MethodBase method)
        {
            ParameterInfo[] parameters = method.GetParameters();

            string name = method.Name + "(";
            if (parameters.Length > 0)
                name += parameters.Select(x => x.Name).Aggregate((x, y) => x + ", " + y);
            name += ")";

            return name;
        }


        /*************************************/

        protected virtual void AddMethodToTree(Tree<MethodBase> tree, IEnumerable<string> path, MethodBase method)
        {
            Dictionary<string, Tree<MethodBase>> children = tree.Children;

            if (path.Count() == 0)
            {
                string name = method.Name;
                bool isIMethod = (name.Length > 1 && Char.IsUpper(name[1]));

                if (isIMethod)
                    name = name.Substring(1);

                name = GetMethodString(method);

                if (isIMethod || !children.ContainsKey(name))
                    children[name] = new Tree<MethodBase> { Value = method, Name = name };
            }
            else
            {
                string name = path.First();
                if (!children.ContainsKey(name))
                    children.Add(name, new Tree<MethodBase> { Name = name });
                AddMethodToTree(children[name], path.Skip(1), method);
            }
        }


        /*******************************************/

        protected virtual void AppendMethodTreeToMenu(Tree<MethodBase> tree, ContextMenu menu)
        {
            MenuItem treeMenu = new MenuItem { Header = tree.Name };
            menu.Items.Add(treeMenu);
            foreach (Tree<MethodBase> childTree in tree.Children.Values.OrderBy(x => x.Name))
                AppendMethodTreeToMenu(childTree, treeMenu);
        }

        /*******************************************/

        protected virtual void AppendMethodTreeToMenu(Tree<MethodBase> tree, MenuItem menu)
        {
            if (tree.Children.Count == 0 || (tree.Children.Count == 1 && tree.Children.Values.First().Children.Count == 0))
            {
                MenuItem methodItem = new MenuItem { Header = tree.Name };
                methodItem.Click += Item_Click;
                menu.Items.Add(methodItem);
                if (tree.Children.Count == 0)
                    m_MethodLinks[methodItem] = tree.Value;
                else
                    m_MethodLinks[methodItem] = tree.Children.Values.First().Value;
            }
            else
            {
                MenuItem treeMenu = new MenuItem { Header = tree.Name };
                menu.Items.Add(treeMenu);
                foreach (Tree<MethodBase> childTree in tree.Children.Values.OrderBy(x => x.Name))
                    AppendMethodTreeToMenu(childTree, treeMenu);
            }
        }

        /*******************************************/

        protected virtual void Item_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            if (!m_MethodLinks.ContainsKey(item))
                return;

            m_Node.Method = m_MethodLinks[item];
            if (m_Node.Method == null)
                return;
        }

        /*******************************************/

        protected virtual void AppendSearchMenu(Tree<MethodBase> methods, ContextMenu menu)
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
            foreach (Tree<MethodBase> tree in m_MethodList.Where(x => parts.All(y => x.Name.ToLower().Contains(y))).Take(12).OrderBy(x => x.Name))
            {
                MenuItem methodItem = new MenuItem { Header = tree.Name };
                methodItem.Click += Item_Click;
                m_Menu.Items.Add(methodItem);
                m_SearchResultItems.Add(methodItem);
                m_MethodLinks[methodItem] = tree.Value;
            }
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<MethodBase>> GetMethodList(Tree<MethodBase> tree)
        {
            return tree.Children.Values.SelectMany(x => GetMethodList(x, ""));
        }

        /*******************************************/

        protected virtual IEnumerable<Tree<MethodBase>> GetMethodList(Tree<MethodBase> tree, string path)
        {
            if (path.Length > 0 && !tree.Name.StartsWith("("))
                path = path + '.';

            if (tree.Children.Count == 0)
                return new Tree<MethodBase>[] { new Tree<MethodBase> { Value = tree.Value, Name = path + tree.Name } };
            else
                return tree.Children.Values.SelectMany(x => GetMethodList(x, path + tree.Name));
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected Tree<MethodBase> m_MethodTree;
        protected T m_Node = null;
        protected Dictionary<MenuItem, MethodBase> m_MethodLinks = new Dictionary<MenuItem, MethodBase>();
        private IEnumerable<Tree<MethodBase>> m_MethodList = new List<Tree<MethodBase>>();
        protected List<MenuItem> m_SearchResultItems = new List<MenuItem>();
        private ContextMenu m_Menu;


        /*******************************************/
    }
}
