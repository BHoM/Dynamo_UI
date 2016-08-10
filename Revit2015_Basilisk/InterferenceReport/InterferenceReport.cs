using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Autodesk.DesignScript.Runtime;

namespace InterferenceReport
{
    /// <summary>
    /// InterferenceReport generated from Revit
    /// </summary>
    public class InterferenceReport
    {
        private List<InterferenceReportRow> pInterferenceReportList = new List<InterferenceReportRow>();

        /// <summary>
        /// Creates Interference Report from file path
        /// </summary>
        /// <param name="FilePath">File Path</param>
        /// <search>
        /// Interference Report, InterferenceReport, File Path, filepath, InterferenceReport, file path, interference report
        /// </search>
        private InterferenceReport(object FilePath)
        {
            HtmlDocument aHtmlDocument = new HtmlDocument();
            aHtmlDocument.Load(FilePath.ToString());
            HtmlNodeCollection aHtmlNodeCollection = aHtmlDocument.DocumentNode.SelectNodes(".//table");
            if (aHtmlNodeCollection != null && aHtmlNodeCollection.Count > 0)
            {
                HtmlNode aTableHtmlNode = aHtmlNodeCollection.First();
                aHtmlNodeCollection = aTableHtmlNode.SelectNodes(".//tr");
                if (aHtmlNodeCollection != null && aHtmlNodeCollection.Count > 1)
                {
                    List<HtmlNode> aTdHtmlNodeList = aHtmlNodeCollection.Cast<HtmlNode>().ToList();
                    aTdHtmlNodeList.RemoveAt(0);
                    foreach (HtmlNode aTdHtmlNode in aTdHtmlNodeList)
                    {
                        InterferenceReportRow aReportRow = new InterferenceReportRow(aTdHtmlNode);
                        pInterferenceReportList.Add(aReportRow);
                    }
                }
            }

            aHtmlNodeCollection = aHtmlDocument.DocumentNode.SelectNodes("//p");

        }

        /// <summary>
        /// Creates Interference Report from file path
        /// </summary>
        /// <param name="FilePath">File Path</param>
        /// <returns name="InterferenceReport">Interference Report</returns>
        /// <search>
        /// Interference Report, InterferenceReport, File Path, filepath, InterferenceReport, file path, interference Report
        /// </search>
        public static InterferenceReport ByFilePath(object FilePath)
        {
            return new InterferenceReport(FilePath);
        }

        /// <summary>
        /// Gets all InterferenceReport Rows from InterferenceReport
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <returns name="ReportRows">Report Rows List</returns>
        /// <search>
        /// Get InterferenceReport Rows, InterferenceReport, get InterferenceReport rows, InterferenceReport
        /// </search>
        public static List<InterferenceReportRow> Get(InterferenceReport InterferenceReport)
        {
            return InterferenceReport.pInterferenceReportList;
        }

        /// <summary>
        /// Gets InterferenceReport Row by row index
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <param name="Index">InterferenceReport Row Index</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get InterferenceReport Row, InterferenceReport, get InterferenceReport row, InterferenceReport, GetByIndex, getbyindex
        /// </search>
        public static InterferenceReportRow GetByIndex(InterferenceReport InterferenceReport, int Index)
        {
            return InterferenceReport.pInterferenceReportList.Find(x => InterferenceReportRow.Index(x) == Index);
        }

        /// <summary>
        /// Gets Interference Report Row by category
        /// </summary>
        /// <param name="InterferenceReport">Interference Report</param>
        /// <param name="Category">Category</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Report rows, interference Report, GetByCategory, getbycategory
        /// </search>
        public static List<InterferenceReportRow> GetByCategory(InterferenceReport InterferenceReport, Revit.Elements.Category Category)
        {
            string aCategoryName = Category.Name.ToUpper();
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, 0).ToUpper() == aCategoryName || InterferenceReportRow.Category(x, 1).ToUpper() == aCategoryName);
        }

        /// <summary>
        /// Gets Interference Report Row by category
        /// </summary>
        /// <param name="InterferenceReport">Interference Report</param>
        /// <param name="Category">Category</param>
        /// <param name="Index">Interference Report Column Index (A = 0, B = 1)</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Report Rows, Interference Report, get Report rows, interference Report, GetByCategory, getbycategory
        /// </search>
        public static List<InterferenceReportRow> GetByCategory(InterferenceReport InterferenceReport, Revit.Elements.Category Category, int Index)
        {
            string aCategoryName = Category.Name.ToUpper();
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, Index).ToUpper() == aCategoryName);
        }

        /// <summary>
        /// Gets Interference Report Row by element Id
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <param name="Id">Element Id</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, interference Report, GetById, getbyid
        /// </search>
        public static List<InterferenceReportRow> GetById(InterferenceReport InterferenceReport, int Id)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Id(x, 0) == Id || InterferenceReportRow.Id(x, 1) == Id);
        }

        /// <summary>
        /// Gets Interference Report Row by element Id
        /// </summary>
        /// <param name="InterferenceReport">Interference Report</param>
        /// <param name="Id">Element Id</param>
        /// <param name="Index">Interference Report Column Index (A = 0, B = 1)</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, interference Report, GetById, getbyid
        /// </search>
        public static List<InterferenceReportRow> GetById(InterferenceReport InterferenceReport, int Id, int Index)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Id(x, Index) == Id);
        }

        /// <summary>
        /// Gets Interference Report Row by Family Name
        /// </summary>
        /// <param name="InterferenceReport">Interference Report</param>
        /// <param name="FamilyName">Family Name</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, interference Report, GetByFamilyName, getbyfamilyname
        /// </search>
        public static List<InterferenceReportRow> GetByFamilyName(InterferenceReport InterferenceReport, string FamilyName)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, 0).ToUpper() == FamilyName || InterferenceReportRow.Category(x, 1).ToUpper() == FamilyName);
        }

        /// <summary>
        /// Gets Interference Report Row by Family Name
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <param name="FamilyName">Family Name</param>
        /// <param name="Index">InterferenceReport Column Index (A = 0, B = 1)</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, InterferenceReport, GetByFamilyName, getbyfamilyname
        /// </search>
        public static List<InterferenceReportRow> GetByFamilyName(InterferenceReport InterferenceReport, string FamilyName, int Index)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, Index).ToUpper() == FamilyName);
        }

        /// <summary>
        /// Gets InterferenceReport Row by Document Name
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <param name="DocumentName">Document Name</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, Interference Report, GetByDocumentName, getbydocumentname
        /// </search>
        public static List<InterferenceReportRow> GetByDocumentName(InterferenceReport InterferenceReport, string DocumentName)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, 0).ToUpper() == DocumentName || InterferenceReportRow.Category(x, 1).ToUpper() == DocumentName);
        }

        /// <summary>
        /// Gets Interference Report Row by Document Name
        /// </summary>
        /// <param name="InterferenceReport">InterferenceReport</param>
        /// <param name="DocumentName">Document Name</param>
        /// <param name="Index">InterferenceReport Column Index (A = 0, B = 1)</param>
        /// <returns name="ReportRow">Report Row</returns>
        /// <search>
        /// Get Interference Report Rows, Interference Report, get Interference Report rows, InterferenceReport, GetByDocumentName, getbydocumentname
        /// </search>
        public static List<InterferenceReportRow> GetByDocumentName(InterferenceReport InterferenceReport, string DocumentName, int Index)
        {
            return InterferenceReport.pInterferenceReportList.FindAll(x => InterferenceReportRow.Category(x, Index).ToUpper() == DocumentName);
        }
    }

    /// <summary>
    /// Interference Report Row
    /// </summary>
    public class InterferenceReportRow
    {
        private int pIndex = -1;
        private string[] pDocuments = new string[2];
        private string[] pCategories = new string[2];
        private string[] pFamilies = new string[2];
        private string[] pTypes = new string[2];
        private int[] pIds = new int[2];

        internal InterferenceReportRow(HtmlNode HtmlNode)
        { 
            HtmlNodeCollection aHtmlNodeCollection = HtmlNode.SelectNodes(".//td");
            if (aHtmlNodeCollection != null && aHtmlNodeCollection.Count > 2)
            {
                if (aHtmlNodeCollection[0].InnerText != null)
                    int.TryParse(aHtmlNodeCollection[0].InnerText.Trim(), out pIndex);

                ExtractData(aHtmlNodeCollection[1], 0);
                ExtractData(aHtmlNodeCollection[2], 1);
            }
        }

        private void ExtractData(HtmlNode HtmlNode, int Index)
        {
            if (HtmlNode.InnerText != null)
            {
                string[] aValues = HtmlNode.InnerText.Split(':');
                if(aValues.Length > 3)
                {
                    int aIndex = 0;
                    if (aValues.Length > 4)
                    {
                        pDocuments[Index] = aValues[0].Trim();
                        aIndex++;
                    }
                    else
                    {
                        pDocuments[Index] = string.Empty;
                    }

                    pCategories[Index] = aValues[aIndex].Trim();
                    pFamilies[Index] = aValues[aIndex + 1].Trim();
                    pTypes[Index] = aValues[aIndex + 2].Trim();

                    string[] aIdValues = aValues[aIndex + 3].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if(aIdValues != null && aIdValues.Length > 0)
                    {
                        if (!(int.TryParse(aIdValues.Last(), out pIds[Index])))
                            pIds[Index] = -1;
                    }
                }
            }

        }

        /// <summary>
        /// Gets Interference Report Row Document at specified column Index (A = 0, B = 1)
        /// </summary>
        /// <param name="InterferenceReportRow">Interference Report Row</param>
        /// <param name="Index">InterferenceReport Column index (A = 0, B = 1)</param>
        /// <returns name="Document">Document</returns>
        /// <search>
        /// Get Interference Report Row Document, Interference Report, get Interference Report row document, InterferenceReport, Document
        /// </search>
        public static string Document(InterferenceReportRow InterferenceReportRow, int Index)
        {
            return InterferenceReportRow.pDocuments[Index];
        }

        /// <summary>
        /// Gets Interference Report Row Family at specified column Index (A = 0, B = 1)
        /// </summary>
        /// <param name="InterferenceReportRow">Interference Report Row</param>
        /// <param name="Index">Interference Report Column index (A = 0, B = 1)</param>
        /// <returns name="Family">Family</returns>
        /// <search>
        /// Get Interference Report Row Family, Interference Report, get Interference Report row document, Interference Report, Family
        /// </search>
        public static string Family(InterferenceReportRow InterferenceReportRow, int Index)
        {
            return InterferenceReportRow.pFamilies[Index];
        }

        /// <summary>
        /// Gets Interference Report Row Family Type at specified column Index (A = 0, B = 1)
        /// </summary>
        /// <param name="InterferenceReportRow">Interference Report Row</param>
        /// <param name="Index">InterferenceReport Column index (A = 0, B = 1)</param>
        /// <returns name="Type">Type</returns>
        /// <search>
        /// Get Interference Report Row Family Type, Interference Report, get Interference Report row document, Interference Report, Family Type
        /// </search>
        public static string Type(InterferenceReportRow InterferenceReportRow, int Index)
        {
            return InterferenceReportRow.pTypes[Index];
        }

        /// <summary>
        /// Gets Interference Report Row Element Id at specified column Index (A = 0, B = 1)
        /// </summary>
        /// <param name="InterferenceReportRow">Interference Report Row</param>
        /// <param name="Index">Interference Report Column index (A = 0, B = 1)</param>
        /// <returns name="Id">Id</returns>
        /// <search>
        /// Get Interference Report Row Element Id, Interference Report, get Interference Report row document, InterferenceReport, Element Id
        /// </search>
        public static int Id(InterferenceReportRow InterferenceReportRow, int Index)
        {
            return InterferenceReportRow.pIds[Index];
        }

        /// <summary>
        /// Gets Interference Report Row Element Category at specified column Index (A = 0, B = 1)
        /// </summary>
        /// <param name="InterferenceReportRow">InterferenceReport Row</param>
        /// <param name="Index">InterferenceReport Column index (A = 0, B = 1)</param>
        /// <returns name="Category">Category</returns>
        /// <search>
        /// Get Interference Report Row Element Category, Interference Report, get Interference Report row document, InterferenceReport, Element Category
        /// </search>
        public static string Category(InterferenceReportRow InterferenceReportRow, int Index)
        {
            return InterferenceReportRow.pCategories[Index];
        }

        /// <summary>
        /// Gets Interference Report Row Index
        /// </summary>
        /// <param name="InterferenceReportRow">Interference Report Row</param>
        /// <returns name="Index">Index</returns>
        /// <search>
        /// Get Interference Report Row Index, Interference Report, get Interference Report row document, InterferenceReport
        /// </search>
        public static int Index(InterferenceReportRow InterferenceReportRow)
        {
            return InterferenceReportRow.pIndex;
        }
    }

}
