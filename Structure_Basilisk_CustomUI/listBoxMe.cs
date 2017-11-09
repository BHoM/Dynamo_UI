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
    [NodeName("Create BHoM Object")]
    [NodeDescription("Example Node Model, multiplies A x the value of the slider")]
    [NodeCategory("Basilisk.Base.BHoMObject")]
    [IsDesignScriptCompatible]
    [Serializable]
    

    public class listBoxMe : NodeModel, INodeViewCustomization<listBoxMe>
    {
        private double _sliderValue;

        public listBoxMe()
        {
            OutPortData.Add(new PortData("BHoM Object", "Your custom created BHoM object"));
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), SetOutput(inputAstNodes)) };
        }

        private DynamoViewModel dynamoViewModel;
        private listBoxMe model;
        private NodeView view;
        protected Type test = null;
        protected ConstructorInfo m_Constructor = null;
        protected List<MethodInfo> m_DaGets = new List<MethodInfo>();


        public void CustomizeView(listBoxMe nodeModel, NodeView nodeView)
        {
            view = nodeView;
            model = nodeModel;
            MenuItem types = new MenuItem { Header = "BHoM Types", IsCheckable = false };
            types = GetTypes(types);
            nodeView.MainContextMenu.Items.Add(types);
            types.Click += Types_Click;
            nodeModel.InPorts.Clear();
        }

        private void Types_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuItem test1 = e.OriginalSource as MenuItem;
            test = (Type)test1.Tag;
            GetInputInfo(test);
        }

        public void GetInputInfo(Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();
            m_Constructor = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > m_Constructor.GetParameters().Length)
                    m_Constructor = info;
            }
            List<ParameterInfo> inputs = m_Constructor.GetParameters().ToList();
            UpdateInputs(inputs);
            int items = view.MainContextMenu.Items.Count;
            view.MainContextMenu.Items.RemoveAt(items - 1);
            //view.UpdateLayout();

        }

        public void UpdateInputs(List<ParameterInfo> inputs)
        {
            foreach (ParameterInfo info in inputs)
                model.InPortData.Add(new PortData(info.Name, "test"));

            model.RegisterAllPorts();
            model.NickName = "BHoM " + test.Name;
            model.OverrideNameWithNickName = true;
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

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);

            if (test != null && m_Constructor != null)
            {
                var serNode = element.OwnerDocument.CreateElement("TypeName");
                serNode.InnerText = test.AssemblyQualifiedName;
                element.AppendChild(serNode);
            }           
        }

        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);
            var deSer = nodeElement.ChildNodes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "TypeName");
            test = Type.GetType(deSer.InnerText);

            ConstructorInfo[] constructors = test.GetConstructors();
            m_Constructor = constructors[0];
            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > m_Constructor.GetParameters().Length)
                    m_Constructor = info;
            }
            List<ParameterInfo> inputs = m_Constructor.GetParameters().ToList();
        }

        public AssociativeNode SetOutput(List<AssociativeNode> inputAstNodes)
        {

            //if (!check)
            //    return AstFactory.BuildNullNode();

            try
            {
                var function = AstFactory.BuildPrimitiveNodeFromObject(m_Constructor.Invoke(inputAstNodes.ToArray()));
                return function;
            }
            catch (Exception)
            {

            }

            return AstFactory.BuildNullNode();

        }
    }
}
