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