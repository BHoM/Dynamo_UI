//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dynamo.Controls;
//using Dynamo.Wpf;

//namespace Structure_Basilisk_CustomUI
//{
//    public class listBoxNodeView : VariableInputNodeViewCustomization, INodeViewCustomization<listBoxMe>
//    {
//        public void CustomizeView(listBoxMe model, NodeView nodeView)
//        {
//            UserControl1 listbox = new UserControl1()
//            {
//                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
//                VerticalAlignment = System.Windows.VerticalAlignment.Center
//            };
//            nodeView.inputGrid.Children.Add(listbox);
//            listbox.DataContext = model;
//            base.CustomizeView(model, nodeView);
//        }

//        public void Dispose()
//        {
//        }
//    }
//}
