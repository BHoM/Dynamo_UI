/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
using BH.Engine.Reflection;
using System.Collections;

namespace BH.UI.Dynamo.Templates
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
            OutPortData.Add(new PortData("  ", "result obtained from the method"));
            RegisterAllPorts();

            ArgumentLacing = LacingStrategy.Shortest;
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
            // Make sure the method has been assigned
            if (m_Method == null)
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

            // Check if the component has all the inputs it needs
            ParameterInfo[] parameters = m_Method.GetParameters();
            bool IsReady = inputAstNodes != null && inputAstNodes.Count == parameters.Count();
            if (IsReady)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (inputAstNodes[i].Kind == AstKind.Null && !parameters[i].HasDefaultValue)
                    {
                        IsReady = false;
                        break;
                    }
                }
            }
            if (!IsReady)
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };

            // Apppend replication guide to the input parameter based on lacing strategy
            UseLevelAndReplicationGuide(inputAstNodes);

            // Get the Full name of the method
            string name = Method.ToText(true, "(", ",", ")", false);

            // If method doesn't exist in the global dictionary yet, create one and store it there (not ideal but works for now)
            if (!Methods.Compute.MethodsToExecute.ContainsKey(name))
                Methods.Compute.MethodsToExecute[name] = m_Method;

            Type enumerableType = typeof(IEnumerable);
            string prefix = Guid.NewGuid().ToString() + "_";
            List<AssociativeNode> toDo = new List<AssociativeNode>();
            List<AssociativeNode> arguments = new List<AssociativeNode>() { AstFactory.BuildStringNode(name) };
            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = parameters[i].ParameterType;
                if (enumerableType.IsAssignableFrom(paramType) && paramType != typeof(string))
                {
                    AssociativeNode localCall;
                    Type[] argTypes = paramType.GetGenericArguments();
                    if (argTypes.Length > 0 && enumerableType.IsAssignableFrom(argTypes[0]) && argTypes[0] != typeof(string))
                        localCall = AstFactory.BuildFunctionCall("BH.UI.Dynamo.Methods.Create", "TreeWrapper", new List<AssociativeNode> { inputAstNodes[i] });
                    else
                        localCall = AstFactory.BuildFunctionCall("BH.UI.Dynamo.Methods.Create", "ListWrapper", new List<AssociativeNode> { inputAstNodes[i] });
                    AssociativeNode newVar = AstFactory.BuildIdentifier(prefix + i.ToString());
                    toDo.Add(AstFactory.BuildAssignment(newVar, localCall));
                    arguments.Add(newVar);
                }
                else
                    arguments.Add(inputAstNodes[i]);
            }

            // Create the Build assignment for outpout 0
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.Dynamo.Methods.Compute", "ExecuteMethod", arguments);
            toDo.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall));
            return toDo;
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
                                {
                                    string paramTypeString = child.Attributes["value"].Value;

                                    //Fix for namespace change in structure
                                    if (paramTypeString.Contains("oM.Structural"))
                                    {
                                        paramTypeString = paramTypeString.Replace("oM.Structural", "oM.Structure");
                                    }

                                    paramTypes.Add(Type.GetType(paramTypeString));
                                }
                            }
                            break;
                        }
                }
            }

            if (type != null)
                RegisterMethod(type, methodName, paramTypes);

            base.DeserializeCore(element, context);
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
                m_Descriptions = method.GetParameters().Select(x => x.ParameterType.ToText() + (x.HasDefaultValue ? ("\nDefault value: " + (x.DefaultValue != null ? x.DefaultValue.ToString() : "null")) : "")).ToList();
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

            List<MethodBase> methods;
            if (methodName == ".ctor")
                methods = type.GetConstructors().ToList<MethodBase>();
            else
                methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList<MethodBase>();

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
