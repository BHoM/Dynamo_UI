using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.DesignScript.Runtime;
using DynamoServices;

namespace Basilisk.Project
{

    class ProjectManager
    {
        internal ProjectManager() { }
        private static int ProjectID = 1;

        public BHoM.Global.Project project { get; set; }
    }

    /// <summary>
    /// Serialisable Project ID object
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [Serializable]
    public class ProjectID : ISerializable
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
        /// Construct empty Project ID
        /// </summary>
        public ProjectID()
        {
            IntID = int.MinValue;

        }

        /// <summary>
        /// Ctor used by the serialisation engine
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ProjectID(SerializationInfo info, StreamingContext context)
        {
            IntID = (int)info.GetValue("intID", typeof(int));

        }
    }

    /// <summary>
    /// Traced Project
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [DynamoServices.RegisterForTrace]
    public class TracedProject : IDisposable
    {
        internal TracedProject() { }

        /// <summary>ID</summary>
        public int ID { get; private set; }

        void WorkspaceEventsWorkspaceAdded(WorkspacesModificationEventArgs args)
        {
           
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