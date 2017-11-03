using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Controls;
using Dynamo.Wpf;

namespace Structure_Basilisk_CustomUI
{
    public class listBoxNodeView : INodeViewCustomization<listBoxMe>
    {
        public void CustomizeView(listBoxMe model, NodeView nodeView)
        {
            listBoxTest listbox = new listBoxTest();
            nodeView.inputGrid.Children.Add(listbox);
            listbox.DataContext = model;
        }

        public void Dispose()
        {
        }
    }
}
