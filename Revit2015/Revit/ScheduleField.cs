using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RevitServices.Persistence;

namespace Revit
{
    /// <summary>
    /// A Schedule Field
    /// </summary>
    public static class ScheduleField
    {
        /// <summary>
        /// Returns name of Schedule Field
        /// </summary>
        /// <param name="SchedulableField">Schedule Field</param>
        /// <returns name="Name">Name</returns>
        /// <search>
        /// Schedule Field name, Schedule Field, schedule field name, schedule field
        /// </search>
        public static string Name(Autodesk.Revit.DB.ScheduleField ScheduleField)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (ScheduleField != null && aDocument != null)
                return ScheduleField.GetName();

            return null;
        }

        /// <summary>
        /// Returns Schedulable Field of Schedule Field
        /// </summary>
        /// <param name="SchedulableField">Schedule Field</param>
        /// <returns name="SchedulableField">Schedulable Field</returns>
        /// <search>
        /// Schedulable Field of Schedule Field, Schedule Field, schedulable field schedule field, schedule field
        /// </search>
        public static Autodesk.Revit.DB.SchedulableField SchedulableField(Autodesk.Revit.DB.ScheduleField ScheduleField)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (ScheduleField != null && aDocument != null)
                return ScheduleField.GetSchedulableField();

            return null;
        }

        /// <summary>
        /// Returns true if Schedule Field is hidden
        /// </summary>
        /// <param name="SchedulableField">Schedule Field</param>
        /// <returns name="IsHidden">true if schedule field is hidden</returns>
        /// <search>
        /// Schedule Field is hidden, Schedule Field, schedule field is hidden, schedule field
        /// </search>
        public static bool? IsHidden(Autodesk.Revit.DB.ScheduleField ScheduleField)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (ScheduleField != null && aDocument != null)
                return ScheduleField.IsHidden;

            return null;
        }
    }
}
