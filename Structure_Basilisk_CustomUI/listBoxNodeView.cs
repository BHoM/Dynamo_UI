using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Controls;
using Dynamo.Wpf;
using Dynamo.ViewModels;
using BH.oM.Base;
using BHER = BH.Engine.Reflection;

namespace Structure_Basilisk_CustomUI
{
    public class listBoxNodeView : VariableInputNodeViewCustomization, INodeViewCustomization<listBoxMe>
    {
        private DynamoViewModel dynamoViewModel;
        private listBoxMe model;
        private NodeView view;


        public void CustomizeView(listBoxMe nodeModel, NodeView nodeView)
        {
            base.CustomizeView(nodeModel, nodeView);

            MenuItem types = new MenuItem { Header = "TYPES", IsCheckable = false };
            types = GetTypes(types);
            nodeView.MainContextMenu.Items.Add(types);
            types.Click += Types_Click;
        }

        private void Types_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //MenuItem item = (MenuItem)sender;
            object myObj = ((MenuItem)sender).Tag;
            
        }

        public MenuItem GetTypes(MenuItem menuItem)
        {
            Type bhomType = typeof(BHoMObject);
            IEnumerable<Type> types = types = BHER.Create.TypeList().Where(x => x.IsSubclassOf(bhomType) && !x.ContainsGenericParameters).OrderBy(x => x.Name);

            foreach (Type type in types)
            {
                MenuItem tempItem = new MenuItem() { Header = type.Name, IsCheckable = false, Tag = type};
                menuItem.Items.Add(tempItem);
            }
            return menuItem;
        }

        public void Dispose()
        {
        }
    }
}
