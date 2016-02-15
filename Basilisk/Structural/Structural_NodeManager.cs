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

    /// <summary>
    /// Node ID
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [Serializable]
    public class nodeID : ISerializable
    {
        /// <summary>ID number as integer</summary>
        public int IntID { get; set; }

        /// <summary>
        /// Get object serialisation data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("intID", IntID, typeof(int));
        }

        /// <summary>
        /// Node ID
        /// </summary>
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

    /// <summary>
    /// Traced node object
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [DynamoServices.RegisterForTrace]
    public class TracedNode : IDisposable
    {
        internal TracedNode() { }
        //TODO(lukechurch): This really should have been moved into the attribute already
        private const string REVIT_TRACE_ID = "{0459D869-0C72-447F-96D8-08A7FB92214B}-REVIT";

        /// <summary>X</summary>
        public double X { get; set; }
        /// <summary>Y</summary>
        public double Y { get; set; }
        /// <summary>ID</summary>
        public int ID { get; private set; }

        /// <summary>
        /// Traced node by X Y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private TracedNode(double x, double y)
            : this(x, y, NodeManager.GetNextUnusedID())
        {
        }

        /// <summary>
        /// Traced node by X Y ID
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="id"></param>
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

        /// <summary>
        /// Traced node by point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}: ({1}, {2})", ID, X, Y);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // Unhook the workspace event handler.
            WorkspaceEvents.WorkspaceAdded -= WorkspaceEventsWorkspaceAdded;
        }
    }
}