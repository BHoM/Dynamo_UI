using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Wpf;
using Dynamo.ViewModels;
using BH.oM.Base;
using BHER = BH.Engine.Reflection;
using System.Reflection;

namespace Structure_Basilisk_CustomUI
{
    public class listBoxNodeView :  INodeViewCustomization<listBoxMe>
    {
        private DynamoViewModel dynamoViewModel;
        private listBoxMe model;
        private NodeView view;


        public void CustomizeView(listBoxMe nodeModel, NodeView nodeView)
        {
            view = nodeView;
            model = nodeModel;
            MenuItem types = new MenuItem { Header = "TYPES", IsCheckable = false };
            types = GetTypes(types);
            nodeView.MainContextMenu.Items.Add(types);
            types.Click += Types_Click;
            nodeModel.InPorts.Clear();
        }

        private void Types_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem test1 = e.OriginalSource as MenuItem;
            Type test = (Type)test1.Tag;
            GetInputInfo(test);

        }

        public void GetInputInfo(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            ConstructorInfo Constructor = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > Constructor.GetParameters().Length)
                    Constructor = info;
            }
            List<ParameterInfo> inputs = Constructor.GetParameters().ToList();
            UpdateInputs(inputs);
            view.UpdateLayout();
        }

        public void UpdateInputs(List<ParameterInfo> inputs)
        {
            foreach (ParameterInfo info in inputs)
                model.InPortData.Add(new PortData(info.Name, "test"));
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
