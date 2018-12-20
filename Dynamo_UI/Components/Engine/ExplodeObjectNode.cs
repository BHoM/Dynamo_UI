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

using Autodesk.DesignScript.Runtime;
using Dynamo.Engine;
using Dynamo.Graph;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace BH.UI.Dynamo.Components
{
    [NodeName("ExplodeObject (old)")]
    [NodeDescription("Explode a BHoMObject into its properties")]
    [NodeCategory("BHoM.Engine")]
    [IsDesignScriptCompatible]
    //[IsVisibleInDynamoLibrary(false)]
    public class ExplodeObjectNode : VariableOutputNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public EngineController DynamoEngine { get; set; } = null;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExplodeObjectNode()
        {
            InPortData.Add(new PortData("object", "Object to Explode"));

            RegisterAllPorts();
            AddListeners();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override string GetOutputTooltip(int index)
        {
            return "";
        }

        /*******************************************/

        public override string GetOutputName(int index)
        {
            if (index < m_OutputNames.Count)
                return m_OutputNames[index];
            else
                return "unkown";
        }

        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (m_NeedChange)
                SetOutputs(m_OutputNames);

            List<string> dlls = new List<string>();
            if (DynamoEngine != null)
                dlls = DynamoEngine.LibraryServices.ImportedLibraries.ToList();

            if (OutPortData.Count != OutPorts.Count)
                CreateMissingPorts();

            if (IsPartiallyApplied || OutPortData.Count != m_OutputNames.Count || OutPorts.Count != m_OutputNames.Count)
            {
                return new List<AssociativeNode>();
            }
            else
            {
                var method = new Func<object, string, object>(Methods.Query.GetProperty);

                List<AssociativeNode> outputs = new List<AssociativeNode>();
            
                for (int i = 0; i < m_OutputNames.Count; i++)
                {
                    var functionCall = AstFactory.BuildFunctionCall(method, new List<AssociativeNode> {
                        inputAstNodes[0],
                        AstFactory.BuildStringNode(GetOutputName(i))
                    });

                    outputs.Add(AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(i), functionCall));
                }

                return outputs;
            };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var nameList = xmlDoc.CreateElement("OutputNames");
            element.AppendChild(nameList);
            for (int i = 0; i < m_OutputNames.Count; i++)
            {
                var name = xmlDoc.CreateElement("InputName");
                name.SetAttribute("value", m_OutputNames[i]);
                nameList.AppendChild(name);
            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            m_OutputNames = new List<string>();

            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.Name == "OutputNames")
                {
                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.Attributes != null && child.Attributes["value"] != null)
                            m_OutputNames.Add(child.Attributes["value"].Value);
                    }
                }
            }

            if (m_OutputNames.Count == OutPortData.Count)
            {
                for (int i = 0; i < m_OutputNames.Count; i++)
                    OutPortData[i].NickName = m_OutputNames[i];
            }

            RegisterAllPorts();
        }


        /*******************************************/
        /**** Event Handling Methods            ****/
        /*******************************************/

        protected void AddListeners()
        {
            PropertyChanged += M_Node_PropertyChanged;
            foreach (var port in InPorts)
                port.Connectors.CollectionChanged += Connectors_CollectionChanged;
        }

        /*******************************************/

        private void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnDataChange();
        }

        /*******************************************/

        private void M_Node_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnDataChange();
        }

        /*******************************************/

        protected void OnDataChange()
        {
            if (DynamoEngine == null || InPorts.Count == 0 || InPorts[0].Connectors.Count == 0) return;

            NodeModel valuesNode = InPorts[0].Connectors[0].Start.Owner;
            int index = InPorts[0].Connectors[0].Start.Index;
            string startId = valuesNode.GetAstIdentifierForOutputIndex(index).Name;
            RuntimeMirror colorsMirror = DynamoEngine.GetMirror(startId);

            if (colorsMirror == null || colorsMirror.GetData() == null) return;

            MirrorData mirrorData = colorsMirror.GetData();
            object obj = null;

            if (mirrorData.IsCollection)
            {
                IEnumerable<MirrorData> elements = mirrorData.GetElements();
                if (elements.Count() > 0)
                    obj = elements.First().Data;
            }
            else
                obj = mirrorData.Data;

            if (obj == null) return;

            CheckForChange(Engine.Reflection.Query.PropertyDictionary(obj).Keys.ToList());
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void CheckForChange(List<string> names)
        {
            m_NeedChange = names.Count != OutPortData.Count;

            if (!m_NeedChange)
            {
                for (int i = 0; i < names.Count; i++)
                    m_NeedChange |= names[i] != OutPortData[i].NickName;
            }

            if (m_NeedChange)
            {
                m_OutputNames = names;
                this.OnBuilt();
            }
        }

        /*******************************************/

        private void SetOutputs(List<string> names)
        {
            m_NeedChange = false;

            VariableOutputController.SetNumOutputs(names.Count);
            if (names.Count != OutPortData.Count)
                Console.WriteLine("ouch");

            for (int i = 0; i < names.Count; i++)
                OutPortData[i].NickName = names[i];

            RegisterAllPorts();
        }

        /*******************************************/

        private void CreateMissingPorts()
        {
            for (int i = OutPorts.Count; i < OutPortData.Count; i++)
                AddPort(PortType.Output, OutPortData[i], i);
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private bool m_NeedChange = false;
        List<string> m_OutputNames = new List<string>();


        /*******************************************/
    }
}
