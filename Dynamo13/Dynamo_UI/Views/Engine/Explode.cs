/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using BH.oM.Base;
using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using BH.UI.Base.Components;
using BH.UI.Base;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Dynamo.Views
{
    public class ExplodeView : CallerView<ExplodeComponent> 
    {
        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public override void CustomizeView(ExplodeComponent component, NodeView nodeView)
        {
            base.CustomizeView(component, nodeView);

            component.PortConnected += ExplodeComponent_PortConnected;
            component.PortDisconnected += ExplodeComponent_PortDisconnected;
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void ExplodeComponent_PortDisconnected(PortModel obj)
        {
            if (obj.PortType == PortType.Input)
                UpdateOutputs();
        }

        /*******************************************/

        private void ExplodeComponent_PortConnected(PortModel arg1, global::Dynamo.Graph.Connectors.ConnectorModel arg2)
        {
            if (arg1.PortType == PortType.Input)
                UpdateOutputs();
        }

        /*******************************************/

        private void UpdateOutputs()
        {
            if (m_DynamoEngine == null || m_Node.InPorts.Count == 0 || m_Node.InPorts[0].Connectors.Count == 0) return;

            NodeModel valuesNode = m_Node.InPorts[0].Connectors[0].Start.Owner;
            int index = m_Node.InPorts[0].Connectors[0].Start.Index;
            string startId = valuesNode.GetAstIdentifierForOutputIndex(index).Name;
            RuntimeMirror colorsMirror = m_DynamoEngine.GetMirror(startId);

            if (colorsMirror == null || colorsMirror.GetData() == null) return;

            MirrorData mirrorData = colorsMirror.GetData();
            List<object> data = CollectData(mirrorData);

            ExplodeCaller caller = m_Node.Caller as ExplodeCaller;
            caller.CollectOutputTypes(data);
            m_Node.RefreshComponent();
        }

        /*******************************************/

        private List<object> CollectData(MirrorData mirrorData)
        {
            if (mirrorData.IsCollection)
            {
                IEnumerable<MirrorData> elements = mirrorData.GetElements();
                if (elements.Count() > 0)
                    return elements.SelectMany(x => CollectData(x)).ToList();
                else
                    return new List<object>();
            }
            else
                return new List<object> { mirrorData.Data };
        }

        /*******************************************/
    }
}

