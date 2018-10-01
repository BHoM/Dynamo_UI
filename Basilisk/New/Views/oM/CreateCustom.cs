using BH.oM.Base;
using BH.UI.Basilisk.Components;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using Dynamo.Controls;
using Dynamo.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace BH.UI.Basilisk.Views
{
    public class CreateCustomView : CallerView<CreateCustomComponent> 
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public override void CustomizeView(CreateCustomComponent nodeModel, NodeView nodeView)
        {
            base.CustomizeView(nodeModel, nodeView);

            m_View = nodeView;
            SetButtons();
            AppendInputNameMenu(true);

            nodeView.MainContextMenu.Closed += MainContextMenu_Closed;
            nodeModel.InPorts.CollectionChanged += InPorts_CollectionChanged;
        }

        /*******************************************/

        protected override void Caller_ItemSelected(object sender, object e)
        {
            SetButtons();
            base.Caller_ItemSelected(sender, e);
        }

        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void SetButtons() //TODO: Consider having one pair of buttons per input to allow more flexible changes
        {
            if (m_ButtonPanel != null)
                m_ButtonPanel.Children.Clear();
            else
            {
                m_ButtonPanel = new WrapPanel
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Orientation = Orientation.Vertical
                };
                m_View.inputGrid.Children.Add(m_ButtonPanel);
            }

            for (int i = 0; i < m_Node.Caller.InputParams.Count; i++)
            {
                var button = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
                button.Click += RemoveButton_Click;
                m_ButtonPanel.Children.Add(button);
            }
            
            var addButton = new DynamoNodeButton() { Content = "+", Width = 26, Height = 26 };
            addButton.Click += AddButton_Click;
            m_ButtonPanel.Children.Add(addButton);
        }

        /*******************************************/

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
            List<string> inputs = m_Node.InPorts.Select(x => x.PortName).ToList();
            
            var button = new DynamoNodeButton() { Content = "-", Width = 26, Height = 26 };
            button.Click += RemoveButton_Click;
            m_ButtonPanel.Children.Insert(inputs.Count, button );

            inputs.Add("item" + inputs.Count);
            caller.SetInputs(inputs);
            m_Node.RefreshComponent();

            var a = m_View.ContentGrid.Children;
        }

        /*******************************************/

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            DynamoNodeButton button = sender as DynamoNodeButton;
            int index = m_ButtonPanel.Children.IndexOf(button);
            m_ButtonPanel.Children.Remove(button);

            CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
            List<string> inputs = m_Node.InPorts.Select(x => x.PortName).ToList();
            inputs.RemoveAt(index);
            caller.SetInputs(inputs);
            m_Node.RefreshComponent();
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

            for (int i = m_Node.InPorts.Count; i < m_InputNameItems.Count; i++)
                m_Menu.Items.Remove(m_InputNameItems[i]);

            for (int i = m_InputNameItems.Count; i < m_Node.InPorts.Count; i++)
            {
                TextBox textBox = new TextBox { Text = m_Node.InPorts[i].PortName };
                textBox.TextChanged += TextBox_TextChanged;
                m_Menu.Items.Add(textBox);
                m_InputNameItems.Add(textBox);
            }
        }

        /*******************************************/

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            m_InputsNeedRefreshing = true;
        }

        /*******************************************/

        private void MainContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            if (m_InputsNeedRefreshing)
            {
                m_InputsNeedRefreshing = false;
                CreateCustomCaller caller = m_Node.Caller as CreateCustomCaller;
                List<string> inputs = m_InputNameItems.Select(x => x.Text).ToList();
                caller.SetInputs(inputs);
                m_Node.RefreshComponent();
            }
        }

        /*******************************************/

        private void InPorts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AppendInputNameMenu();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        protected NodeView m_View = null;
        protected WrapPanel m_ButtonPanel = null;
        protected List<TextBox> m_InputNameItems = new List<TextBox>();
        protected ContextMenu m_Menu;
        protected bool m_InputsNeedRefreshing = false;

        /*******************************************/
    }
}
