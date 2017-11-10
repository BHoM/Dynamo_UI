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
using ProtoCore.AST.AssociativeAST;
using Dynamo.Graph;
using System.Xml;

namespace Structure_Basilisk_CustomUI
{

    public class listBoxMeView : INodeViewCustomization<listBoxMe>
    {
        private DynamoViewModel dynamoViewModel;
        private listBoxMe model;
        private NodeView view;


        public void CustomizeView(listBoxMe nodeModel, NodeView nodeView)
        {
            view = nodeView;
            model = nodeModel;
            if (model.runView)
            {
                MenuItem types = new MenuItem { Header = "BHoM Types", IsCheckable = false };
                types = GetTypes(types);
                nodeView.MainContextMenu.Items.Add(types);
                types.Click += Types_Click;
                model.RegisterAllPorts();
            }
            else
            {
                UpdateInputs(model.inputs);
            }
            //model.UpdateInputs(model.inputs);
        }

        public MenuItem GetTypes(MenuItem menuItem)
        {
            Type bhomType = typeof(BHoMObject);
            IEnumerable<Type> types = types = BHER.Create.TypeList().Where(x => x.IsSubclassOf(bhomType) && !x.ContainsGenericParameters).OrderBy(x => x.Name);

            foreach (Type type in types)
            {
                MenuItem tempItem = new MenuItem() { Header = type.Name, IsCheckable = false, Tag = type };
                menuItem.Items.Add(tempItem);
            }
            return menuItem;
        }

        public void Dispose()
        {
        }


        private void Types_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem test1 = e.OriginalSource as MenuItem;
            model.test = (Type)test1.Tag;
            GetInputInfo(model.test);
        }



        public void GetInputInfo(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            model.ConstructorInfo = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > model.ConstructorInfo.GetParameters().Length)
                    model.ConstructorInfo = info;
            }
            model.inputs = model.ConstructorInfo.GetParameters().ToList();
            UpdateInputs(model.inputs);
            int items = view.MainContextMenu.Items.Count;
            view.MainContextMenu.Items.RemoveAt(items - 1);
            //view.UpdateLayout();
            //model.ConstructorInfo = m_Constructor;
        }

        public void UpdateInputs(List<ParameterInfo> inputs)
        {
            foreach (ParameterInfo info in inputs)
                model.InPortData.Add(new PortData(info.Name, "test"));

            model.RegisterAllPorts();
            model.NickName = "BHoM " + model.test.Name;
            model.OverrideNameWithNickName = true;
        }
    }

}