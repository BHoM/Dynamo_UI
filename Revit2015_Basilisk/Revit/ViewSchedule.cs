using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using Revit.Elements;
using RevitServices.Persistence;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Schedule
    /// </summary>
    public static class Schedule
    {
        /// <summary>
        /// Returns all elements from view schedule
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <returns name="Elements">Elements</returns>
        /// <search>
        /// Schedule elements, view shedule elements
        /// </search>
        public static List<Elements.Element> Elements(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null)
            {
                ViewSchedule aViewSchedule = Element.InternalElement as ViewSchedule;
                if (aViewSchedule != null)
                    return new FilteredElementCollector(aViewSchedule.Document, aViewSchedule.Id).ToElements().ToList().ConvertAll(x => ElementWrapper.ToDSType(x, true));
            }
            return null;
        }

        /// <summary>
        /// Creates schedule by name and category
        /// </summary>
        /// <param name="Name">Schedule name</param>
        /// <param name="Category">Schedule category</param>
        /// <returns name="ViewSchedule">View Schedule</returns>
        /// <search>
        /// Create schedule, create schedule
        /// </search>
        public static Elements.Element ByNameAndCategory(string Name, Revit.Elements.Category Category)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (aDocument != null && Category != null)
            {
                Autodesk.Revit.DB.Category aCategory = aDocument.Settings.Categories.get_Item(Category.Name);
                
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                ViewSchedule aViewSchedule = ViewSchedule.CreateSchedule(aDocument, aCategory.Id);
                TransactionManager.Instance.TransactionTaskDone();
                if(aViewSchedule != null)
                {
                    aViewSchedule.Name = Name;
                    return aViewSchedule.ToDSType(true);
                }

            }
            return null;
        }

        /// <summary>
        /// Gets Schedulable fields for schedule. This is list of fields may be added to schedule
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <returns name="SchedulableFields">List of Schedulable fields</returns>
        /// <search>
        /// Schedulable fields, ViewSchedule, schedulable fields, schedule, view schedule
        /// </search>
        public static List<Autodesk.Revit.DB.SchedulableField> SchedulableFields(Revit.Elements.Element Element)
        {
            if(Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.ViewSchedule)
            {
                ViewSchedule aViewSchedule = Element.InternalElement as Autodesk.Revit.DB.ViewSchedule;
                return aViewSchedule.Definition.GetSchedulableFields().ToList();
            }
            return null;
        }

        /// <summary>
        /// Gets Schedule fields. This is list of existing fields in schedule
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <returns name="ScheduleFields">List of Schedule fields</returns>
        /// <search>
        /// Schedule fields, Schedule, schedule fields, schedule
        /// </search>
        public static List<Autodesk.Revit.DB.ScheduleField> ScheduleFields(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.ViewSchedule)
            {
                List<Autodesk.Revit.DB.ScheduleField> aResult = new List<Autodesk.Revit.DB.ScheduleField>();
                Autodesk.Revit.DB.ViewSchedule aViewSchedule = Element.InternalElement as Autodesk.Revit.DB.ViewSchedule;
                int aCount = aViewSchedule.Definition.GetFieldCount();
                for (int i = 0; i < aCount; i++)
                    aResult.Add(aViewSchedule.Definition.GetField(i));
                return aResult;
            }
            return null;
        }

        /// <summary>
        /// Removes Schedule fields from schedule.
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <param name="ScheduleField">Schedule Field</param>
        /// <returns name="RemoveField">true if schedule field was removed</returns>
        /// <search>
        /// Remove schedule field, Schedule, remove schedule field, schedule
        /// </search>
        public static bool RemoveField(Revit.Elements.Element Element, Autodesk.Revit.DB.ScheduleField ScheduleField)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.ViewSchedule)
            {
                Autodesk.Revit.DB.ViewSchedule aViewSchedule = Element.InternalElement as Autodesk.Revit.DB.ViewSchedule;
                if (aViewSchedule != null)
                {
                    TransactionManager.Instance.EnsureInTransaction(aViewSchedule.Document);
                    aViewSchedule.Definition.RemoveField(ScheduleField.FieldId);
                    TransactionManager.Instance.TransactionTaskDone();

                    if (aViewSchedule.Definition.GetField(ScheduleField.FieldId) == null)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds field to schedule.
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <param name="SchedulableField">SchedulableField</param>
        /// <returns name="AddField">Added Schedule Field</returns>
        /// <search>
        /// Add schedule field, Schedule, add schedule field, schedule
        /// </search>
        public static Autodesk.Revit.DB.ScheduleField AddField(Revit.Elements.Element Element, Autodesk.Revit.DB.SchedulableField SchedulableField)
        {
            if (SchedulableField != null && Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.ViewSchedule)
            {
                
                ViewSchedule aViewSchedule = Element.InternalElement as Autodesk.Revit.DB.ViewSchedule;
                List<Autodesk.Revit.DB.SchedulableField> aSchedulableFieldList = aViewSchedule.Definition.GetSchedulableFields().ToList();
                foreach(Autodesk.Revit.DB.SchedulableField aSchedulableField in aSchedulableFieldList)
                {
                    IList<ScheduleFieldId> aScheduleFieldIdList = aViewSchedule.Definition.GetFieldOrder();
                    foreach (ScheduleFieldId aScheduleFieldId in aScheduleFieldIdList)
                        if (aViewSchedule.Definition.GetField(aScheduleFieldId).GetSchedulableField() == SchedulableField)
                            return aViewSchedule.Definition.GetField(aScheduleFieldId);
                }

                if (aViewSchedule.Document != null)
                {
                    TransactionManager.Instance.EnsureInTransaction(aViewSchedule.Document);
                    Autodesk.Revit.DB.ScheduleField aScheduleField = aViewSchedule.Definition.AddField(SchedulableField);
                    TransactionManager.Instance.TransactionTaskDone();
                    return aScheduleField;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets field from schedule.
        /// </summary>
        /// <param name="Element">Schedule</param>
        /// <param name="Index">Schedulable Field Index</param>
        /// <returns name="AddField">Added Schedule Field</returns>
        /// <search>
        /// Get field, Schedule, get field, schedule
        /// </search>
        public static Autodesk.Revit.DB.ScheduleField GetField(Revit.Elements.Element Element, int Index)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.ViewSchedule)
            {
                Autodesk.Revit.DB.ViewSchedule aViewSchedule = Element.InternalElement as Autodesk.Revit.DB.ViewSchedule;
                if(aViewSchedule != null)
                    return aViewSchedule.Definition.GetField(Index);
            }
            return null;
        }
        
        /// <summary>
        /// Returns number of fields in schedule.
        /// </summary>
        /// <param name="Element">View Schedule</param>
        /// <returns name="FieldCount">Number of Fields</returns>
        /// <search>
        /// Field count, Schedule, field count, schedule
        /// </search>
        public static int? FieldCount(Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is ViewSchedule)
            {

                ViewSchedule aViewSchedule = Element.InternalElement as ViewSchedule;
                if (aViewSchedule != null)
                    return aViewSchedule.Definition.GetFieldCount();
            }
            return null;
        }
    }
}
