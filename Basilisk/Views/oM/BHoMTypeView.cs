using BH.Engine.DataStructure;
using BH.Engine.Reflection.Convert;
using BH.oM.Base;
using BH.oM.DataStructure;
using BH.UI.Basilisk.Components;
using BH.UI.Basilisk.Templates;
using Dynamo.Controls;
using Dynamo.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
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
