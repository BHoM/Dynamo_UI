using BH.UI.Basilisk.Components;
using Dynamo.Controls;
using Dynamo.Nodes;
using Dynamo.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BH.UI.Basilisk.Views
{
    public class CustomObjectView : INodeViewCustomization<CustomObjectNode>
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public virtual void CustomizeView(CustomObjectNode nodeModel, NodeView nodeView)
        {
            m_Node = nodeModel;
            m_View = nodeView;
            SetButtons();
            AppendInputNameMenu(true);

            m_View.MainContextMenu.Closed += MainContextMenu_Closed;
            m_Node.InPortData.CollectionChanged += InPortData_CollectionChanged;
        }

        /*******************************************/

        public void Dispose()
        {

        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void SetButtons() //TODO: Consider having one pair of buttons per input to allow more flexible changes
        {
            var addButton = new DynamoNodeButton(m_View.ViewModel.NodeModel, "AddInPort") { Content = "+", Width = 20 };
            var subButton = new DynamoNodeButton(m_View.ViewModel.NodeModel, "RemoveInPort") { Content = "-", Width = 20 };

            var wp = new WrapPanel
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            wp.Children.Add(addButton);
            wp.Children.Add(subButton);

            m_View.inputGrid.Children.Add(wp);
        }


        /*******************************************/

        protected void AppendInputNameMenu(bool firstTime = false)
        {
            if (firstTime)
            {
                m_Menu = m_View.MainContextMenu;
                m_Menu.Items.Add(new Separator());

                MenuItem label = new MenuItem { Header = "Input Names", IsCheckable = false };
                m_Menu.Items.Add(label);
            }
            
            for (int i = m_Node.InPortData.Count; i < m_InputNameItems.Count; i++)
                m_Menu.Items.Remove(m_InputNameItems[i]);

            for (int i = m_InputNameItems.Count; i < m_Node.InPortData.Count; i++)
            {
                TextBox textBox = new TextBox { Text = m_Node.InPortData[i].NickName };
                textBox.TextChanged += TextBox_TextChanged;
                m_Menu.Items.Add(textBox);
                m_InputNameItems.Add(textBox);
            }
        }

        /*******************************************/

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box == null) return;

            int index = m_InputNameItems.IndexOf(box);
            if (index < 0 || index >= m_Node.InPortData.Count) return;

            m_Node.InPortData[index].NickName = box.Text;
            m_Node.InputNames = m_InputNameItems.Select(x => x.Text).ToList();
            m_InputsNeedRefreshing = true;
        }

        /*******************************************/

        private void InPortData_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AppendInputNameMenu();
        }

        /*******************************************/

        private void MainContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (m_InputsNeedRefreshing)
            {
                m_InputsNeedRefreshing = false;
                m_Node.RegisterAllPorts();
            }
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected CustomObjectNode m_Node = null;
        protected NodeView m_View = null;
        protected List<TextBox> m_InputNameItems = new List<TextBox>();
        protected ContextMenu m_Menu;
        protected bool m_InputsNeedRefreshing = false;


        /*******************************************/
    }
}
