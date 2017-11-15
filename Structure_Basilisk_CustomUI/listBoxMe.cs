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
using DynamoUnits;
using System.Linq.Expressions;

namespace Structure_Basilisk_CustomUI
{
    [NodeName("Create BHoM Object")]
    [NodeDescription("Example Node Model, multiplies A x the value of the slider")]
    [NodeCategory("Basilisk.Base.BHoMObject")]
    [IsDesignScriptCompatible]
    [Serializable]

    
    public class listBoxMe : NodeModel
    {
        private listBoxMeView _thisView;
        public List<ParameterInfo> inputs { get; set; }
        public List<Type> inputs2 { get; set; }
        public MethodBase ConstructorInfo { get; set; } = null;
        public Type test = null;
        public bool runView = true;



        public listBoxMe()
        {
            OutPortData.Add(new PortData("BHoM Object", "Your custom created BHoM object"));
        }



        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), SetOutput(inputAstNodes)) };
        }


        //public static Func<T,T,T> GetCtor<T>()
        //{
        //    Type type = typeof(T);
        //    Expression body = Expression.New(type);
        //    return Expression.Lambda<Func<T,T,T>>(body).Compile();
        //}

        //public Type create_type(Type[] types)
        //{
        //    return typeof(Func<>).MakeGenericType(types);
        //    return typeof(Func<,>).MakeGenericType(types); 
        //    return typeof(Func<,,>).MakeGenericType(types); 
        //    return typeof(Func<,,,>).MakeGenericType(types);                                                            
        //}



        public AssociativeNode SetOutput(List<AssociativeNode> inputAstNodes)
        {
            var delegateType = typeof(Func<,>).MakeGenericType(test);
            return AstFactory.BuildNullNode();
        }



        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);

            if (test != null && ConstructorInfo != null)
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
            ConstructorInfo = constructors[0];

            if (ConstructorInfo != null)
                runView = false;

            foreach (ConstructorInfo info in constructors)
            {
                ParameterInfo[] param = info.GetParameters();
                if (info.GetParameters().Length > ConstructorInfo.GetParameters().Length)
                    ConstructorInfo = info;
            }
            inputs = ConstructorInfo.GetParameters().ToList();
        }
    }
}
