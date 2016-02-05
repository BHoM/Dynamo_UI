using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.DesignScript.Runtime;

using DynamoServices;

namespace Structural
{

    class NodeManager
    {
        internal NodeManager() { }
        private static int nodeID = 1;

        public static int GetNextUnusedID()
        {
            int next = nodeID;
            nodeID++;
            return next;
        }

        private static Dictionary<int, TracedNode> nodeDictionary = new Dictionary<int, TracedNode>();

        public static TracedNode GetnodeByID(int id)
        {
            TracedNode ret;
            nodeDictionary.TryGetValue(id, out ret);
            return ret;
        }

        public static void RegisternodeForID(int id, TracedNode node)
        {
            if (nodeDictionary.ContainsKey(id))
            {
                nodeDictionary[id] = node;
            }
            else
            {
                nodeDictionary.Add(id, node);
            }

        }

    }
    [IsVisibleInDynamoLibrary(false)]
    [Serializable]
    public class nodeID : ISerializable
    {
        public int IntID { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("intID", IntID, typeof(int));
        }

        public nodeID()
        {
            IntID = int.MinValue;

        }

        /// <summary>
        /// Ctor used by the serialisation engine
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public nodeID(SerializationInfo info, StreamingContext context)
        {
            IntID = (int)info.GetValue("intID", typeof(int));

        }
    }

    [IsVisibleInDynamoLibrary(false)]
    [DynamoServices.RegisterForTrace]
    public class TracedNode : IDisposable
    {
        internal TracedNode() { }
        //TODO(lukechurch): This really should have been moved into the attribute already
        private const string REVIT_TRACE_ID = "{0459D869-0C72-447F-96D8-08A7FB92214B}-REVIT";

        public double X { get; set; }
        public double Y { get; set; }

        public int ID { get; private set; }

        private TracedNode(double x, double y)
            : this(x, y, NodeManager.GetNextUnusedID())
        {
        }

        private TracedNode(double x, double y, int id)
        {

            this.X = x;
            this.Y = y;
            this.ID = id;

            NodeManager.RegisternodeForID(id, this);

            WorkspaceEvents.WorkspaceAdded += WorkspaceEventsWorkspaceAdded;
        }

        void WorkspaceEventsWorkspaceAdded(WorkspacesModificationEventArgs args)
        {
            // What does a node do when a workspace is opened?
        }

        public static TracedNode ByPoint(double x, double y)
        {
            TracedNode tnode;

            nodeID hid = TraceUtils.GetTraceData(REVIT_TRACE_ID) as nodeID;

            if (hid == null)
            {
                // Trace didn't give us a node, it's a new one.
                tnode = new TracedNode(x, y);
            }
            else
            {
                tnode = NodeManager.GetnodeByID(hid.IntID);
            }

            // Set the trace data on the return to be this node.
            TraceUtils.SetTraceData(REVIT_TRACE_ID, new nodeID { IntID = tnode.ID });

            return tnode;
        }

        public override string ToString()
        {
            return String.Format("{0}: ({1}, {2})", ID, X, Y);
        }

        public void Dispose()
        {
            // Unhook the workspace event handler.
            WorkspaceEvents.WorkspaceAdded -= WorkspaceEventsWorkspaceAdded;
        }
    }
}