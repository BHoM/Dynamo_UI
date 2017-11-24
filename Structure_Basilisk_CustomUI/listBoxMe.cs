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
        public ConstructorInfo ConstructorInfo { get; set; } = null;
        public Type test = null;
        public bool runView = true;



        public listBoxMe()
        {
            OutPortData.Add(new PortData("BHoM Object", "Your custom created BHoM object"));
        }



        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (ConstructorInfo != null)
            {
                if (!HasConnectedInput(0) || !HasConnectedInput(1))
                {
                    return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
                }

                object a = SetOutput(inputAstNodes);
                var functionCall =
                AstFactory.BuildFunctionCall(
                new Func<List<AssociativeNode>, object>(this.SetOutput),
                inputAstNodes);

                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            }

            else
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

        }

        //public AssociativeNode SetOutput(List<AssociativeNode> inputAstNodes)
        //{

        //    ConstructorInfo constructor = null;
        //    Type delegateType = null;
        //    try
        //    {
        //        delegateType = Type.GetType("System.Func`3").MakeGenericType(inputs2[0], inputs2[1], test);
        //        constructor = delegateType.GetConstructors().First();
        //    }
        //    catch (Exception)
        //    {
        //        return AstFactory.BuildNullNode();
        //    }

        //    //return AstFactory.BuildFunctionCall(Delegate.CreateDelegate(delegateType, ConstructorInfo) ,inputAstNodes);
        //    return AstFactory.BuildFunctionCall(ConstructorInfo.DeclaringType.FullName, "ctor", new List<AssociativeNode>());
        //}

        public List<object> SetOutput(List<AssociativeNode> inputAstNodes)
        {
            List<object> inputs3 = new List<object>();
            for (int i = 0; i < inputAstNodes.Count; i++)
            {
                Tuple<int, NodeModel> data;
                TryGetInput(i, out data);
                inputs3.Add(data.Item2.CachedValue.Data);
            }

            return inputs3;
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
