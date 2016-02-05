using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.DesignScript.Runtime;
using DynamoServices;

namespace Structural
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

[IsVisibleInDynamoLibrary(false)]
[Serializable]
public class barID : ISerializable
{
    public int IntID { get; set; }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("intID", IntID, typeof(int));
    }

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

[IsVisibleInDynamoLibrary(false)]
[DynamoServices.RegisterForTrace]
public class TracedBar : IDisposable
{
    internal TracedBar(){}
    //TODO(lukechurch): This really should have been moved into the attribute already
    private const string REVIT_TRACE_ID = "{0459D869-0C72-447F-96D8-08A7FB92214B}-REVIT";

    public double X { get; set; }
    public double Y { get; set; }

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