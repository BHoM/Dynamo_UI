using System.Collections.Generic;
using System.Linq;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Reflection;
using Dynamo.Engine;
using System;
using System.Linq.Expressions;
using System.Xml;
using Dynamo.Graph;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using System.Diagnostics;

namespace BH.UI.Basilisk.Templates
{
    public abstract class MethodNode : VariableInputNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public MethodBase Method
        {
            get { return m_Method; }
            set { RegisterMethod(value); }
        }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public MethodNode()
        {
            OutPortData.Add(new PortData("result", "result"));
            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Longest;
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override string GetInputName(int index)
        {
            if (index < m_Inputs.Count())
                return m_Inputs[index];
            else
                return "unknown";
        }

        /*******************************************/

        protected override string GetInputTooltip(int index)
        {
            if (index < m_Descriptions.Count())
                return m_Descriptions[index];
            else
                return "No description";
        }

        /*******************************************/

        public override bool IsConvertible
        {
            get { return true; }
        }

        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (IsPartiallyApplied || Method == null)
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
            else
            {
                // Get the Full name of the method
                string name = Method.DeclaringType.FullName + Method.Name;

                // If method doesn't exist in the global dictionary yet, create one and store it there (not ideal but works for now)
                if (!Methods.Compute.MethodsToExecute.ContainsKey(name))
                    Methods.Compute.MethodsToExecute[name] = m_Method;

                // Create the Build assignment for outpout 0
                List<AssociativeNode> arguments = new List<AssociativeNode> { AstFactory.BuildStringNode(name) }.Concat(inputAstNodes).ToList();
                AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.Basilisk.Methods.Compute", "ExecuteMethod", arguments);
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
            };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            if (Method != null)
            {
                ParameterInfo[] parameters = Method.GetParameters();

                var typeName = xmlDoc.CreateElement("TypeName");
                typeName.SetAttribute("value", Method.DeclaringType.AssemblyQualifiedName);
                element.AppendChild(typeName);

                var methodName = xmlDoc.CreateElement("MethodName");
                methodName.SetAttribute("value", Method.Name);
                element.AppendChild(methodName);

                var paramList = xmlDoc.CreateElement("ParamList");
                element.AppendChild(paramList);
                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = xmlDoc.CreateElement("ParamType");
                    paramType.SetAttribute("value", parameters[i].ParameterType.AssemblyQualifiedName);
                    paramList.AppendChild(paramType);
                }

            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            Type type = null;
            string methodName = "";
            List<Type> paramTypes = new List<Type>();

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "TypeName":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                            type = Type.GetType(node.Attributes["value"].Value);
                        break;
                    case "MethodName":
                        if (node.Attributes != null && node.Attributes["value"] != null)
                            methodName = node.Attributes["value"].Value;
                        break;
                    case "ParamList":
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                if (child.Attributes != null && child.Attributes["value"] != null && child.Attributes["value"].Value != null)
                                    paramTypes.Add(Type.GetType(child.Attributes["value"].Value));
                            }
                            break;
                        }
                }
            }

            if (type != null)
                RegisterMethod(type, methodName, paramTypes);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        protected void RegisterMethod(MethodBase method)
        {
            if (method == null) return;

            m_Method = method;
            NickName = (method is ConstructorInfo) ? m_Method.DeclaringType.Name : m_Method.Name;

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length == 0)
            {
                m_Inputs = new List<string>();
                m_Descriptions = new List<string>();
            }
            else
            {
                m_Inputs = method.GetParameters().Select(x => x.Name).ToList();
                m_Descriptions = method.GetParameters().Select(x => x.ParameterType.FullName + (x.HasDefaultValue ? ("\nDefault value: " + (x.DefaultValue != null ? x.DefaultValue.ToString() : "null")) : "")).ToList();
            }

            for (int i = InPortData.Count; i < m_Inputs.Count; i++)
                AddInput();

            for (int i = m_Inputs.Count; i < InPortData.Count; i++)
                RemoveInput();

            for (int i = 0; i < InPortData.Count; i++)
                InPortData[i].NickName = m_Inputs[i];

            RegisterAllPorts();
        }

        /*******************************************/

        protected void RegisterMethod(Type type, string methodName, List<Type> paramTypes)
        {
            m_Method = null;

            List<MethodBase> methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList<MethodBase>();

            foreach (MethodBase method in methods)
            {
                if (method.Name == methodName)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == paramTypes.Count)
                    {
                        bool matching = true;
                        for (int i = 0; i < paramTypes.Count; i++)
                        {
                            matching &= (parameters[i].ParameterType == paramTypes[i]);
                        }
                        if (matching)
                        {
                            m_Method = method;
                            break;
                        }
                    }
                }
            }

            if (m_Method != null)
                RegisterMethod(m_Method);
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private MethodBase m_Method = null;
        private List<string> m_Inputs = new List<string>();
        private List<string> m_Descriptions = new List<string>();


        /***************************************************/
    }
}
