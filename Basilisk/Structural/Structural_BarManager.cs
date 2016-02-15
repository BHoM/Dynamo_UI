using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.DesignScript.Runtime;
using DynamoServices;

namespace Basilisk.Structural
{

class BarManager
{
    internal BarManager() { }
    private static int barID = 1;
    
    public static int GetNextUnusedID()
    {
        int next = barID;
        barID++;
        return next;
    }

    private static Dictionary<int, TracedBar> barDictionary = new Dictionary<int, TracedBar>();

    public static TracedBar GetbarByID(int id)
    {
        TracedBar ret;
        barDictionary.TryGetValue(id, out ret);
        return ret;
    }

    public static void RegisterbarForID(int id, TracedBar bar)
    {
        if (barDictionary.ContainsKey(id))
        {
            barDictionary[id] = bar;
        }
        else
        {
            barDictionary.Add(id, bar);
        }

    }

}

/// <summary>
/// Serialisable bar ID object
/// </summary>
[IsVisibleInDynamoLibrary(false)]
[Serializable]
public class barID : ISerializable
{
        /// <summary>ID number as integer</summary>
        public int IntID { get; set; }

        /// <summary>
        /// Get serialisation data
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("intID", IntID, typeof(int));
    }

        /// <summary>
        /// Construct empty bar ID
        /// </summary>
    public barID()
    {
        IntID = int.MinValue;

    }

    /// <summary>
    /// Ctor used by the serialisation engine
    /// </summary>
    /// <param name="info"></param>
    /// <param name="context"></param>
    public barID(SerializationInfo info, StreamingContext context)
    {
        IntID = (int)info.GetValue("intID", typeof(int));

    }
}

    /// <summary>
    /// Traced bar
    /// </summary>
[IsVisibleInDynamoLibrary(false)]
[DynamoServices.RegisterForTrace]
public class TracedBar : IDisposable
{
    internal TracedBar(){}
    //TODO(lukechurch): This really should have been moved into the attribute already
    private const string REVIT_TRACE_ID = "{0459D869-0C72-447F-96D8-08A7FB92214B}-REVIT";

        /// <summary>X</summary>
        public double X { get; set; }
        /// <summary>Y</summary>
        public double Y { get; set; }

        /// <summary>ID</summary>
        public int ID { get; private set; }

    private TracedBar(double x, double y)
        : this(x, y, BarManager.GetNextUnusedID())
    {
    }

    private TracedBar(double x, double y, int id)
    {

        this.X = x;
        this.Y = y;
        this.ID = id;

        BarManager.RegisterbarForID(id, this);

        WorkspaceEvents.WorkspaceAdded += WorkspaceEventsWorkspaceAdded;
    }

    void WorkspaceEventsWorkspaceAdded(WorkspacesModificationEventArgs args)
    {
        // What does a bar do when a workspace is opened?
    }

        /// <summary>
        /// Construct a traced bar by point
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
    public static TracedBar ByPoint(double x, double y)
    {
        TracedBar tbar;

        barID hid = TraceUtils.GetTraceData(REVIT_TRACE_ID) as barID;

        if (hid == null)
        {
            // Trace didn't give us a bar, it's a new one.
            tbar = new TracedBar(x, y);
        }
        else
        {
            tbar = BarManager.GetbarByID(hid.IntID);
        }

        // Set the trace data on the return to be this bar.
        TraceUtils.SetTraceData(REVIT_TRACE_ID, new barID { IntID = tbar.ID });
        
        return tbar;
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
        /// 
        /// </summary>
    public void Dispose()
    {
        // Unhook the workspace event handler.
        WorkspaceEvents.WorkspaceAdded -= WorkspaceEventsWorkspaceAdded;
    }
}
}