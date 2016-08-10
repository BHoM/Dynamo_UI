using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RevitServices.Persistence;
using Autodesk.Revit.DB;

namespace Revit
{
    /// <summary>
    /// A schedulable field of Revit Schedule
    /// </summary>
    public static class SchedulableField
    {
        /// <summary>
        /// Returns name of Schedulable Field
        /// </summary>
        /// <param name="SchedulableField">Schedulable Field</param>
        /// <returns name="Name">Name</returns>
        /// <search>
        /// Schedulable Field name, Schedulable Field, schedulable field name, schedulable field
        /// </search>
        public static string Name(Autodesk.Revit.DB.SchedulableField SchedulableField)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (SchedulableField != null && aDocument != null)
                return SchedulableField.GetName(aDocument);

            return null;
        }

        /// <summary>
        /// Returns Id of displayed parameter
        /// </summary>
        /// <param name="SchedulableField">Schedulable Field</param>
        /// <returns name="ParameterId">Id of displayed parameter</returns>
        /// <search>
        /// Parameter Id, Schedulable Field, schedulable field name, parameter id
        /// </search>
        public static int? ParameterId(Autodesk.Revit.DB.SchedulableField SchedulableField)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (SchedulableField != null && aDocument != null)
                return SchedulableField.ParameterId.IntegerValue;
            return null;
        }
    }
}
