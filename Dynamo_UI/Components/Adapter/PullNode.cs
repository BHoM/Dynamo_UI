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
using BH.Adapter;
using BH.UI.Dynamo.Templates;
using Dynamo.Engine;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.UI.Dynamo.Components
{
    [NodeName("Pull (old)")]
    [NodeDescription("Pull objects from the external software")]
    [NodeCategory("BHoM.Adapter")]
    [InPortNames("Adapter", "Query", "Config", "Active")]
    [InPortTypes("object", "object", "object", "bool")]
    [InPortDescriptions("Adapter", "BHoM Query\nDefault: new FilterQuery()", "Pull config\nDefault: null", "Execute the pull\nDefault: false")]
    [OutPortNames("Objects")]
    [OutPortTypes("object[]")]
    [OutPortDescriptions("Objects obtained from the query")]
    [IsDesignScriptCompatible]
    [IsVisibleInDynamoLibrary(false)]
    public class PullNode : ZeroTouchNode
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public EngineController DynamoEngine { get; set; } = null;


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PullNode()
        {
            ClassName = "Methods.CRUD";
            MethodName = "Pull";

            DefaultValues = new Dictionary<int, AssociativeNode> {
                { 1, AstFactory.BuildNullNode() },
                { 2, AstFactory.BuildNullNode() },
                { 3, AstFactory.BuildBooleanNode(false) }
            };

            RegisterAllPorts();
            //AddListeners();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            RegisterAdapter();
            return base.BuildOutputAst(inputAstNodes);
        }

        /*******************************************/
        /**** Event Handling Methods            ****/
        /*******************************************/

        /*protected void AddListeners()
        {
            InPorts[0].Connectors.CollectionChanged += Connectors_CollectionChanged;
        }*/

        /*******************************************/

        public void RegisterAdapter()
        {
            if (DynamoEngine == null || InPorts.Count == 0 || InPorts[0].Connectors.Count == 0) return;

            NodeModel valuesNode = InPorts[0].Connectors[0].Start.Owner;
            int index = InPorts[0].Connectors[0].Start.Index;
            string startId = valuesNode.GetAstIdentifierForOutputIndex(index).Name;
            RuntimeMirror colorsMirror = DynamoEngine.GetMirror(startId);

            if (colorsMirror == null || colorsMirror.GetData() == null) return;

            MirrorData mirrorData = colorsMirror.GetData();
            BHoMAdapter adapter = null;

            if (mirrorData.IsCollection)
            {
                IEnumerable<MirrorData> elements = mirrorData.GetElements();
                if (elements.Count() > 0)
                    adapter = elements.First().Data as BHoMAdapter;
            }
            else
                adapter = mirrorData.Data as BHoMAdapter;

            if (adapter == null || adapter.BHoM_Guid == m_AdapterId) return;

            
            m_AdapterId = adapter.BHoM_Guid;
            adapter.DataUpdated += Obj_DataUpdated;
        }

        /*******************************************/

        private void Obj_DataUpdated(object sender, System.EventArgs e)
        {
            OnNodeModified(true);
        }



        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private Guid m_AdapterId;

        /*******************************************/
    }
}