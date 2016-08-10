using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.Elements;

using RevitServices.Persistence;
using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Panel Schedule View
    /// </summary>
    public static class PanelScheduleView
    {
        /// <summary>
        /// Change template for panel schedule view.
        /// </summary>
        /// <param name="PanelScheduleView">Panel Schedule View</param>
        /// <param name="PanelScheduleTemplate">Panel Schedule Template</param>
        /// <returns name="ChangeTemplate">Panel Schedule View</returns>
        /// <search>
        /// Change panel schedule template, Panel schedule view, change panel schedule template, panel schedule view
        /// </search>
        public static Elements.Element ChangeTemplate(Revit.Elements.Element PanelScheduleView, Elements.Element PanelScheduleTemplate)
        {
            if (PanelScheduleView != null && PanelScheduleView.InternalElement != null && PanelScheduleTemplate.InternalElement is Autodesk.Revit.DB.Electrical.PanelScheduleView)
            {
                Autodesk.Revit.DB.Electrical.PanelScheduleView aPanelScheduleView = PanelScheduleTemplate.InternalElement as Autodesk.Revit.DB.Electrical.PanelScheduleView;
                if (PanelScheduleTemplate != null && PanelScheduleTemplate.InternalElement != null && PanelScheduleTemplate.InternalElement is Autodesk.Revit.DB.Electrical.PanelScheduleTemplate)
                {
                    Autodesk.Revit.DB.Electrical.PanelScheduleTemplate aPanelScheduleTemplate = PanelScheduleTemplate.InternalElement as Autodesk.Revit.DB.Electrical.PanelScheduleTemplate;
                    TransactionManager.Instance.EnsureInTransaction(aPanelScheduleTemplate.Document);
                    aPanelScheduleView.GenerateInstanceFromTemplate(aPanelScheduleTemplate.Id);
                    TransactionManager.Instance.TransactionTaskDone();
                    return aPanelScheduleView.ToDSType(true);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates panel schedule view.
        /// </summary>
        /// <param name="Name">Panel Schedule Name</param>
        /// <param name="PanelScheduleTemplate">Panel Schedule Template</param>
        /// <param name="Element">Panel</param>
        /// <returns name="Element">Panel Schedule View</returns>
        /// <search>
        /// Panel Schedule View, PanelScheduleView, panelscheduleview, bynameandtemplate, ByNameAndTemplate
        /// </search>
        public static Elements.Element ByNameAndTemplate(string Name, Elements.Element Element, Elements.Element PanelScheduleTemplate)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Electrical.PanelScheduleView aPanelScheduleView = Autodesk.Revit.DB.Electrical.PanelScheduleView.CreateInstanceView(aDocument, Element.InternalElement.Id);
            TransactionManager.Instance.TransactionTaskDone();
            return aPanelScheduleView.ToDSType(true);
        }
    }
}
