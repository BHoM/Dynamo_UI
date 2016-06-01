using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using RevitServices.Persistence;

using System.Drawing.Printing;
using RevitServices.Transactions;
using Autodesk.DesignScript.Runtime;


namespace Revit
{
    /// <summary>
    /// Revit Printing
    /// </summary>
    public static class PrintManager
    {
        /// <summary>
        /// Prints with current printing settings
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="Success">true if printing ended with success</returns>
        /// <search>
        /// Revit, Print, revit print, Print Manager, print manager
        /// </search>
        public static bool Print(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (aDocument != null)
            {
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                PrintManager.Apply();
                PrintManager.SubmitPrint();
                TransactionManager.Instance.TransactionTaskDone();

                return true;
            }
            return false;
        }

        /// <summary>
        /// Prints set of views
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="Views">Views</param>
        /// <returns name="Success">true if printing ended with success</returns>
        /// <search>
        /// Revit, Print, revit print, Print Manager, print manager
        /// </search>
        public static bool Print(Autodesk.Revit.DB.PrintManager PrintManager, List<Revit.Elements.Element> Views)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (aDocument != null && Views != null)
            {
                ViewSet aViewSet = new ViewSet();
                TransactionManager.Instance.EnsureInTransaction(aDocument);
                ViewSheetSetting aViewSheetSetting = PrintManager.ViewSheetSetting;
                foreach (Revit.Elements.Element aElement in Views)
                    if (aElement.InternalElement != null && aElement.InternalElement is Autodesk.Revit.DB.View)
                        aViewSet.Insert(aElement.InternalElement as Autodesk.Revit.DB.View);

                aViewSheetSetting.CurrentViewSheetSet.Views = aViewSet;

                aViewSheetSetting.SaveAs(Guid.NewGuid().ToString());
                PrintManager.Apply();
                PrintManager.SubmitPrint();
                aViewSheetSetting.Delete();

                TransactionManager.Instance.TransactionTaskDone();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves Sheet Set for printing
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="Views">Views</param>
        /// <param name="Name">Name</param>
        /// <returns name="Success">true if printing saved with success</returns>
        /// <search>
        /// Revit, Print, revit print, Print Manager, print manager, SaveSheetSet, Save Sheet Set
        /// </search>
        public static bool SaveSheetSet(Autodesk.Revit.DB.PrintManager PrintManager, List<Elements.Element> Views, string Name)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            ViewSet aViewSet = new ViewSet();
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.PrintRange aPrintRange = PrintManager.PrintRange;
            PrintManager.PrintRange = Autodesk.Revit.DB.PrintRange.Select;
            PrintManager.Apply();
            ViewSheetSetting aViewSheetSetting = PrintManager.ViewSheetSetting;
            foreach (Elements.Element aElement in Views)
                if (aElement.InternalElement != null && aElement.InternalElement is Autodesk.Revit.DB.View)
                    aViewSet.Insert(aElement.InternalElement as Autodesk.Revit.DB.View);
            aViewSheetSetting.CurrentViewSheetSet.Views = aViewSet;
            aViewSheetSetting.SaveAs(Name);
            PrintManager.PrintRange = aPrintRange;
            PrintManager.Apply();
            TransactionManager.Instance.TransactionTaskDone();
            return true;
        }

        /// <summary>
        /// gets names of installed printers
        /// </summary>
        /// <returns name="Printers">List of instaled printers came</returns>
        /// <search>
        /// Revit, Installed Printers, revit installed printers, Print Manager, print manager
        /// </search>
        public static List<string> InstalledPrinters()
        {
            List<string> aResult = new List<string>();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                aResult.Add(PrinterSettings.InstalledPrinters[i]);
            return aResult;
        }

        /// <summary>
        /// gets paper sizes names for current settings of print manager
        /// </summary>
        /// <returns name="PaperSizes">List of paper size names</returns>
        /// <search>
        /// Revit, Paper Sizes, revit paper sizes, Print Manager, print manager
        /// </search>
        public static List<string> PaperSizes(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            List<string> aResult = new List<string>();
            foreach (Autodesk.Revit.DB.PaperSize aPaperSize in PrintManager.PaperSizes)
            {
                aResult.Add(aPaperSize.Name);
            }
            return aResult;
        }

        /// <summary>
        /// gets printer manager for current document
        /// </summary>
        /// <returns name="PrinterManager">Printer Manager</returns>
        /// <search>
        /// Revit, Get Printer Manager, revit, get printer manager, Print Manager, print manager
        /// </search>
        public static Autodesk.Revit.DB.PrintManager Get()
        {
            return DocumentManager.Instance.CurrentDBDocument.PrintManager;
        }

        /// <summary>
        /// Sets printer name
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="PrinterName">Printer Name</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, Printer Name, Printer Manager, printer name, set printer name
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetPrinterName(Autodesk.Revit.DB.PrintManager PrintManager, string PrinterName)
        {
            PrintManager.SelectNewPrintDriver(PrinterName);
            PrintManager.Apply();
            if (PrintManager.PrinterName == PrinterName)
                return PrintManager;
            return null;
        }

        /// <summary>
        /// Gets printer name
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="PrinterName">Printer Name</returns>
        /// <search>
        /// Revit, Printer Name, Printer Manager, printer name, get printer name
        /// </search>
        public static string GetPrinterName(Autodesk.Revit.DB.PrintManager PrintManager)
        {
                return PrintManager.PrinterName;
        }

        /// <summary>
        /// Sets peper size
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="PaperSize">Paper Size Name</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, Printer Name, Printer Manager, printer name, get printer name
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetPaperSize(Autodesk.Revit.DB.PrintManager PrintManager, string PaperSize)
        {
            foreach (Autodesk.Revit.DB.PaperSize aPaperSize in PrintManager.PaperSizes)
            {
                if(aPaperSize.Name == PaperSize)
                {
                    PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize = aPaperSize;
                    PrintManager.Apply();
                    return PrintManager;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets paper size
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="PaperSize">Paper Size Name</returns>
        /// <search>
        /// Revit, Papaer Size, Printer Manager, paper size, get paper size
        /// </search>
        public static string GetPaperSize(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            return PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PaperSize.Name;
        }

        /// <summary>
        /// Sets Page Orientation
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="IsPortrait">if true then Portrait orientation else Landscape</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, set page orientation, Set Page Orientation, Print Manager, print manager 
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetPageOrientation(Autodesk.Revit.DB.PrintManager PrintManager, bool IsPortrait)
        {
            if (IsPortrait)
                PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation = PageOrientationType.Portrait;
            else
                PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation = PageOrientationType.Landscape;

            PrintManager.Apply();

            return PrintManager;
        }

        /// <summary>
        /// Gets Page Orientation
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="IsPortrait">If Portrait orientation then true else false</returns>
        /// <search>
        /// Revit, get page orientation, Get Page Orientation, Print Manager, print manager 
        /// </search>
        public static bool IsPortrait(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            if (PrintManager.PrintSetup.CurrentPrintSetting.PrintParameters.PageOrientation == PageOrientationType.Portrait)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets Collate for Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="Collate">Collate</returns>
        /// <search>
        /// Revit, get collate, Get collate, Print Manager, print manager 
        /// </search>
        public static bool GetCollate(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            return PrintManager.Collate;
        }

        /// <summary>
        /// Sets Collate for Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="Collate">Collate</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, set collate, Set collate, Print Manager, print manager 
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetCollate(Autodesk.Revit.DB.PrintManager PrintManager, bool Collate)
        {
            PrintManager.Collate = Collate;
            PrintManager.Apply();

            if (PrintManager.Collate == Collate)
                return PrintManager;
            else
                return null;
        }

        /// <summary>
        /// Gets Print to file name for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="PrintToFileName">Print To File Name</returns>
        /// <search>
        /// Revit, get print to file name, Get print to file name, Print Manager, print manager 
        /// </search>
        public static string GetPrintToFileName(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            return PrintManager.PrintToFileName;
        }

        /// <summary>
        /// Sets Print to file name for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="FileName">FileName</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, set print to file name, Set print to file name, Print Manager, print manager 
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetPrintToFileName(Autodesk.Revit.DB.PrintManager PrintManager, string FileName)
        {
            PrintManager.PrintToFileName = FileName;
            PrintManager.Apply();

            if (PrintManager.PrintToFileName == FileName)
                return PrintManager;
            else
                return null;
        }

        /// <summary>
        /// Gets Combined File option value for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="CombinedFile">Combined File</returns>
        /// <search>
        /// Revit, Get combined file, Get combined file, Print Manager, print manager 
        /// </search>
        public static bool GetCombinedFile(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            return PrintManager.CombinedFile;
        }

        /// <summary>
        /// Sets CombinedFile for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="CombinedFile">Combined File</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, set combined file, Set combined file, Print Manager, print manager 
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetCombinedFile(Autodesk.Revit.DB.PrintManager PrintManager, bool CombinedFile)
        {
            PrintManager.CombinedFile = CombinedFile;
            PrintManager.Apply();

            if (PrintManager.CombinedFile == CombinedFile)
                return PrintManager;
            else
                return null;
        }

        /// <summary>
        /// Sets PrintRange for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <param name="PrintRange">Print Range</param>
        /// <returns name="PrintManager">Print Manager</returns>
        /// <search>
        /// Revit, set print range, Set print range, Print Manager, print manager 
        /// </search>
        public static Autodesk.Revit.DB.PrintManager SetPrintRange(Autodesk.Revit.DB.PrintManager PrintManager, PrintRange PrintRange)
        {
            Autodesk.Revit.DB.PrintRange aPrintRange = Functions.GetPrintRange(PrintRange);
            PrintManager.PrintRange = aPrintRange;
            PrintManager.Apply();
            if (PrintManager.PrintRange == aPrintRange)
                return PrintManager;
            else
                return null;
        }

        /// <summary>
        /// Gets PrintRange for Revit Print Manager
        /// </summary>
        /// <param name="PrintManager">Print Manager</param>
        /// <returns name="PrintRange">Print Range</returns>
        /// <search>
        /// Revit, get print range, Get print range, Print Manager, print manager 
        /// </search>
        public static PrintRange? GetPrintRange(Autodesk.Revit.DB.PrintManager PrintManager)
        {
            Array aArray = Enum.GetValues(typeof(PrintRange));
            foreach(PrintRange aPrintRange in aArray)
            {
                Autodesk.Revit.DB.PrintRange aDBPrintRange = Functions.GetPrintRange(aPrintRange);
                if (PrintManager.PrintRange == aDBPrintRange)
                    return aPrintRange;
            }
            return null;
        }
    }
}
