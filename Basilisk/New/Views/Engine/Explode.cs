using BH.oM.Base;
using BH.UI.Basilisk.Components;
using BH.UI.Basilisk.Templates;
using BH.UI.Components;
using BH.UI.Templates;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using ProtoCore.Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BH.UI.Basilisk.Views
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

        private void ExplodeComponent_PortConnected(PortModel arg1, Dynamo.Graph.Connectors.ConnectorModel arg2)
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
            List<object> data = new List<object>();

            if (mirrorData.IsCollection)
            {
                IEnumerable<MirrorData> elements = mirrorData.GetElements();
                if (elements.Count() > 0)
                    data = elements.Select(x => x.Data).ToList();
            }
            else
                data = new List<object> { mirrorData.Data };

            ExplodeCaller caller = m_Node.Caller as ExplodeCaller;
            caller.CollectOutputTypes(data);
            m_Node.RefreshComponent();
        }

        /*******************************************/
    }
}
