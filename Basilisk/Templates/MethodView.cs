using System;
using System.Collections.Generic;
using System.Reflection;
using BH.oM.DataStructure;
using Dynamo.Wpf;
using Dynamo.Controls;
using BH.UI.Basilisk.Templates;
using System.Windows.Controls;
using System.Linq;
using BH.Engine.Reflection.Convert;
using BH.Engine.DataStructure;

namespace BH.UI.Basilisk.Views
{
    public abstract class MethodView<T> : INodeViewCustomization<T> where T : MethodNode
    {
        /*************************************/
        /**** 1. Helper Properties        ****/
        /*************************************/

        public virtual string MethodGroup { get; set; } = "";


        /*************************************/
        /**** 2 . Helper Methods          ****/
        /*************************************/

        public virtual IEnumerable<MethodBase> GetRelevantMethods()
        {
            if (MethodGroup != "")
                return Engine.Reflection.Query.BHoMMethodList().Where(x => x.DeclaringType.Name == MethodGroup);
            else
                return Engine.Reflection.Query.BHoMMethodList();
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(T model, NodeView nodeView)
        {
            m_Node = model;
            CreateMethodMenu(m_Node.NickName);

            // Set up the menu for the user to choose the component type
            if (model.Method == null)
            {
                SelectorMenu<MethodBase> selector = new SelectorMenu<MethodBase>(nodeView.MainContextMenu, Item_Click);
                selector.AppendTree(m_MethodTree);
                selector.AppendSearchBox(m_MethodList);
            }
        }

        /*******************************************/

        public void Dispose()
        {
            
        }


        /*******************************************/
        /**** Protected Methods                 ****/
        /*******************************************/

        protected virtual void CreateMethodMenu(string nickname)
        {
            //Create the method tree and method list
            if (m_MethodTreeStore.ContainsKey(nickname) && m_MethodListStore.ContainsKey(nickname))
            {
                m_MethodTree = m_MethodTreeStore[nickname];
                m_MethodList = m_MethodListStore[nickname];
            }
            else
            {
                List<string> ignore = new List<string> { "BH", "oM", "Engine" };
                if (MethodGroup != "")
                    ignore.Add(MethodGroup);

                IEnumerable<MethodBase> methods = GetRelevantMethods();
                IEnumerable<string> paths = methods.Select(x => x.ToText(true));

                m_MethodTree = GroupMethodsByName(Create.Tree(methods, paths.Select(x => x.Split('.').Where(y => !ignore.Contains(y))), "Select " + MethodGroup + " methods").ShortenBranches());
                m_MethodList = paths.Zip(methods, (k, v) => new Tuple<string, MethodBase>(k, v)).ToList();

                m_MethodTreeStore[nickname] = m_MethodTree;
                m_MethodListStore[nickname] = m_MethodList;
            }
        }

        /*******************************************/

        protected Tree<MethodBase> GroupMethodsByName(Tree<MethodBase> tree)
        {

            if (tree.Children.Count > 0)
            {
                if (tree.Children.Values.First().Value != null)
                {
                    var groups = tree.Children.Where(x => x.Key.IndexOf('(') > 0).GroupBy(x => x.Key.Substring(0, x.Key.IndexOf('(')));

                    Dictionary<string, Tree<MethodBase>> children = new Dictionary<string, Tree<MethodBase>>();
                    foreach (var group in groups)
                    {
                        if (group.Count() == 1)
                            children.Add(group.Key, new Tree<MethodBase> { Name = group.Key, Value = group.First().Value.Value });
                        else
                            children.Add(group.Key, new Tree<MethodBase> { Name = group.Key, Children = group.ToDictionary(x => x.Key, x => x.Value) });
                    }
                    tree.Children = children;
                }
                else
                {
                    foreach (var child in tree.Children.Values)
                        GroupMethodsByName(child);
                }
            }

            return tree;
        }

        /*******************************************/

        protected virtual void Item_Click(object sender, MethodBase method)
        {
            m_Node.Method = method;
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected T m_Node = null;

        protected Tree<MethodBase> m_MethodTree = new Tree<MethodBase>();
        protected List<Tuple<string, MethodBase>> m_MethodList = new List<Tuple<string, MethodBase>>();


        /*************************************/
        /**** Static Fields               ****/
        /*************************************/

        private static bool m_AssemblyLoaded = false;
        private static Dictionary<string, Tree<MethodBase>> m_MethodTreeStore = new Dictionary<string, Tree<MethodBase>>();
        private static Dictionary<string, List<Tuple<string, MethodBase>>> m_MethodListStore = new Dictionary<string, List<Tuple<string, MethodBase>>>();


        /*******************************************/
    }
}
