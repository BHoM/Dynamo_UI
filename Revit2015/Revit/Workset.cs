using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RevitServices.Persistence;
using RevitServices.Transactions;

using Autodesk.Revit.DB;


namespace Revit
{
    /// <summary>
    /// Revit Workset
    /// </summary>
    public static class Workset
    {
        /// <summary>
        /// Gets workset by name and kind
        /// </summary>
        /// <param name="Name">Workset Name</param>
        /// <param name="WorksetKind">Workset Kind</param>
        /// <returns name="Workset">Workset</returns>
        /// <search>
        /// Revit, Get Workset, Workset, revit, get workset, workset
        /// </search>
        public static Autodesk.Revit.DB.Workset Get(string Name, WorksetKind WorksetKind)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            FilteredWorksetCollector aFilteredWorksetCollector = new FilteredWorksetCollector(aDocument);
            Autodesk.Revit.DB.WorksetKind aWorksetKind = Functions.GetWorksetKind(WorksetKind);

            foreach (Autodesk.Revit.DB.Workset aWorkset in aFilteredWorksetCollector)
            {
                if (aWorkset.Kind == aWorksetKind && aWorkset.Name == Name)
                    return aWorkset;
            }
            return null;
        }

        /// <summary>
        /// Creates new workset with specified name
        /// </summary>
        /// <param name="Name">Workset Name</param>
        /// <returns name="Workset">Workset</returns>
        /// <search>
        /// Revit, Create Workset, Workset, revit, create workset, workset
        /// </search>
        public static Autodesk.Revit.DB.Workset ByName(string Name)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.Workset aWorkset = Autodesk.Revit.DB.Workset.Create(aDocument, Name);
            TransactionManager.Instance.TransactionTaskDone();
            return aWorkset;
        }

        /// <summary>
        /// gets workset Id
        /// </summary>
        /// <param name="Workset">Workset</param>
        /// <returns name="Id">Workset Id</returns>
        /// <search>
        /// Revit, Workset Id, Workset, revit, workset id, workset
        /// </search>
        public static int? Id(Autodesk.Revit.DB.Workset Workset)
        {
            return Workset.Id.IntegerValue;
        }

        /// <summary>
        /// gets workset name
        /// </summary>
        /// <param name="Workset">Workset</param>
        /// <returns name="Name">Name</returns>
        /// <search>
        /// Revit, Workset Name, Workset, revit, workset name, workset
        /// </search>
        public static string Name(Autodesk.Revit.DB.Workset Workset)
        {
            return Workset.Name;
        }
    }
}
