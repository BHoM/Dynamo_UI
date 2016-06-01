using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Autodesk.DesignScript.Runtime;

namespace ClashReport
{
    /// <summary>
    /// Clash Report generated from Nawiswork
    /// </summary>
    public class ClashReport
    {
        private List<ClashItem> pClashItemList = new List<ClashItem>();
        private string pName = null;
        private string pTolerance = null;
        private int pSelfIntersect = -1;
        private int pTotal = -1;
        private int pNew = -1;
        private int pActive = -1;
        private int pReviewed = -1;
        private int pApproved = -1;
        private int pResolved = -1;
        private string pType = null;
        private string pStatus = null;
        

        private ClashReport(HtmlNode HtmlNode)
        {
            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("h3").Where(x => x.ParentNode == HtmlNode).ToList();
            if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                pName = aHtmlNodeList.First().InnerText;

            Dictionary<string, string> aNameValueDictionary = HTML.Functions.GetNameValuePairs(HtmlNode);

            foreach (KeyValuePair<string, string> NameValuePair in aNameValueDictionary)
            {
                if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Tolerance))
                    pTolerance = NameValuePair.Value;
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Total))
                    int.TryParse(NameValuePair.Value, out pTotal);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.SelfIntersect))
                    int.TryParse(NameValuePair.Value, out pSelfIntersect);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.New))
                    int.TryParse(NameValuePair.Value, out pNew);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Active))
                    int.TryParse(NameValuePair.Value, out pActive);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Reviewed))
                    int.TryParse(NameValuePair.Value, out pReviewed);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Approved))
                    int.TryParse(NameValuePair.Value, out pApproved);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Resolved))
                    int.TryParse(NameValuePair.Value, out pResolved);
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Type))
                    pType = NameValuePair.Value;
                else if (NameValuePair.Key.StartsWith(HTML.Names.ClashReport.Status))
                    pStatus = NameValuePair.Value;
            }

            pClashItemList = new List<ClashItem>();
            aHtmlNodeList = HtmlNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML.Names.Class.ClashGroup && x.ParentNode == HtmlNode).ToList();

            if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
            {
                foreach (HtmlNode aHtmlNode in aHtmlNodeList)
                    pClashItemList.Add(new ClashGroup(aHtmlNode));
            }

            aHtmlNodeList = HtmlNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML.Names.Class.ViewPoint && x.ParentNode == HtmlNode).ToList();
            if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                foreach (HtmlNode aHtmlNode in aHtmlNodeList)
                {
                    pClashItemList.Add(new Clash(aHtmlNode));
                }
        }

        private ClashReport(HtmlNode SummaryHtmlNode, HtmlNode MainHtmlNode)
        {
            if (SummaryHtmlNode != null)
            {
                List<HtmlNode> aTrHtmlNodeList = SummaryHtmlNode.Descendants("tr").Where(x => x.ParentNode == SummaryHtmlNode).ToList();
                if (aTrHtmlNodeList != null && aTrHtmlNodeList.Count > 1)
                {
                    List<HtmlNode> aTdHtmlNodeList = aTrHtmlNodeList[0].Descendants("td").Where(x => x.ParentNode == aTrHtmlNodeList[0]).ToList();
                    if (aTdHtmlNodeList != null && aTdHtmlNodeList.Count > 0)
                        pName = aTdHtmlNodeList[0].InnerText;

                    aTdHtmlNodeList = aTrHtmlNodeList[1].Descendants("td").Where(x => x.ParentNode == aTrHtmlNodeList[1]).ToList();
                    if (aTdHtmlNodeList != null && aTdHtmlNodeList.Count > 8)
                    {
                        pTolerance = aTdHtmlNodeList[0].InnerText;
                        int.TryParse(aTdHtmlNodeList[1].InnerText, out pTotal);
                        int.TryParse(aTdHtmlNodeList[2].InnerText, out pNew);
                        int.TryParse(aTdHtmlNodeList[3].InnerText, out pActive);
                        int.TryParse(aTdHtmlNodeList[4].InnerText, out pReviewed);
                        int.TryParse(aTdHtmlNodeList[5].InnerText, out pApproved);
                        int.TryParse(aTdHtmlNodeList[6].InnerText, out pResolved);
                        pType = aTdHtmlNodeList[7].InnerText;
                        pStatus = aTdHtmlNodeList[8].InnerText;
                    }
                }

                pClashItemList = new List<ClashItem>();
                if (MainHtmlNode != null)
                {
                    List<HtmlNode> aHtmlNodeList = MainHtmlNode.Descendants("tr").Where(x => x.ParentNode == MainHtmlNode).ToList();
                    List<HtmlNode> aTempHtmlNodeList = aHtmlNodeList.FindAll(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML_Tabular.Names.Class.HeaderRow);

                    if (aTempHtmlNodeList != null && aTempHtmlNodeList.Count > 1)
                    {
                        Header aHeader = new Header(aTempHtmlNodeList[1]);

                        pClashItemList = new List<ClashItem>();
                        aTempHtmlNodeList = aHtmlNodeList.Where(x => x.ParentNode == MainHtmlNode && x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML_Tabular.Names.Class.ContentRow).ToList();
                        foreach (HtmlNode aHtmlNode in aTempHtmlNodeList)
                        {
                            Clash aClash = aHeader.GetClash(aHtmlNode);
                            pClashItemList.Add(aClash);
                        }
                        aTempHtmlNodeList = aHtmlNodeList.Where(x => x.ParentNode == MainHtmlNode && x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML_Tabular.Names.Class.GroupRow).ToList();
                        foreach (HtmlNode aHtmlNode in aTempHtmlNodeList)
                        {
                            ClashGroup aClashGroup = aHeader.GetClashGroup(aHtmlNode);
                            pClashItemList.Add(aClashGroup);
                        }
                    }
                }
            }
        }

        private static List<ClashReport> FromHTML(List<HtmlNode> HtmlNodeList)
        {
            List<ClashReport> aResult = new List<ClashReport>();

            foreach (HtmlNode aHtmlNode in HtmlNodeList)
                aResult.Add(new ClashReport(aHtmlNode));
            return aResult;
        }

        private static List<ClashReport> FromHTML_Tabular(HtmlNode HtmlNode)
        {
            List<ClashReport> aResult = new List<ClashReport>();

            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("table").ToList();

            List<HtmlNode> aSummaryTableList = aHtmlNodeList.FindAll(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML_Tabular.Names.Class.TestSummaryTable);
            List<HtmlNode> aMainTableList = aHtmlNodeList.FindAll(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML_Tabular.Names.Class.MainTable);
            if (aSummaryTableList != null)
                for (int i = 0; i < aSummaryTableList.Count; i++)
                {
                    HtmlNode aSummaryTable = aSummaryTableList[i];

                    HtmlNode aMainTable = null;
                    if (aMainTableList.Count > i)
                        aMainTable = aMainTableList[i];

                    aResult.Add(new ClashReport(aSummaryTable, aMainTable));
                }

            return aResult;
        }

        /// <summary>
        /// Creates Clash Report from file path
        /// </summary>
        /// <param name="FilePath">File Path</param>
        /// <returns name="ClashReports">Clash Report List</returns>
        /// <search>
        /// Clash Report, ClashReport, File Path, filepath, ClashReport, file path, clash Report
        /// </search>
        public static List<ClashReport> ByFilePath(object FilePath)
        {
            string aExtension = System.IO.Path.GetExtension(FilePath.ToString()).ToUpper();
            if (aExtension.Contains("HTML"))
            {
                List<ClashReport> aResult = new List<ClashReport>();
                HtmlDocument aHtmlDocument = new HtmlDocument();
                aHtmlDocument.Load(FilePath.ToString(), Encoding.UTF8);

                List<HtmlNode> aHtmlNodeList = aHtmlDocument.DocumentNode.Descendants("div").Where(x => x.ParentNode == aHtmlDocument.DocumentNode || x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML.Names.Class.ClashReport).ToList();
                if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                { 
                    aResult = FromHTML(aHtmlNodeList);
                }
                else
                {
                    aResult = FromHTML_Tabular(aHtmlDocument.DocumentNode);
                }
                return aResult;
            }
            return null;
        }

        /// <summary>
        /// Gets Clash Report Name
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Name">Clash Report Name</returns>
        /// <search>
        /// Clash Report, Clash Report Name, Navisworks, Name, clash report, clash report name, navisworks, name
        /// </search>
        public static string Name(ClashReport ClashReport)
        {
            return ClashReport.pName;
        }

        /// <summary>
        /// Gets Clash Report Tolerance
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Tolerance">Interference Report Tolerance</returns>
        /// <search>
        /// Clash Report, Clash Report Tolerance, Navisworks, Tolerance, clash report, clash report tolerance, navisworks, tolerance
        /// </search>
        public static string Tolerance(ClashReport ClashReport)
        {
            return ClashReport.pTolerance;
        }

        /// <summary>
        /// Gets total number of clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Total">Clash Report Total number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Total, Navisworks, Total, clash report, clash report total, navisworks, total
        /// </search>
        public static int Total(ClashReport ClashReport)
        {
            return ClashReport.pTotal;
        }

        /// <summary>
        /// Gets number of new clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="New">Clash Report New number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report New, Navisworks, Total, clash report, clash report new, navisworks, new
        /// </search>
        public static int New(ClashReport ClashReport)
        {
            return ClashReport.pNew;
        }

        /// <summary>
        /// Gets number of active clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Active">Interference Report Active number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report New, Navisworks, Active, clash report, clash report active, navisworks, active
        /// </search>
        public static int Active(ClashReport ClashReport)
        {
            return ClashReport.pActive;
        }

        /// <summary>
        /// Gets number of reviewed clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Reviewed">Clash Report Reviewed number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Reviewed, Navisworks, Reviewed, clash report, clash report reviewed, navisworks, reviewed
        /// </search>
        public static int Reviewed(ClashReport ClashReport)
        {
            return ClashReport.pReviewed;
        }

        /// <summary>
        /// Gets number of approved clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Approved">Clash Report Approved number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Approved, Navisworks, Approved, clash report, clash report approved, navisworks, approved
        /// </search>
        public static int Approved(ClashReport ClashReport)
        {
            return ClashReport.pApproved;
        }

        /// <summary>
        /// Gets number of resolved clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Resolved">Clash Report Resolved number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Resolved, Navisworks, Resolved, clash report, clash report resolved, navisworks, resolved
        /// </search>
        public static int Resolved(ClashReport ClashReport)
        {
            return ClashReport.pResolved;
        }

        /// <summary>
        /// Gets number of delf intersect clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="SelfIntersect">Clash Report Self Intersect number of clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Self Intersect, Navisworks, Self Intersect, clash report, clash report self intersect, navisworks, self intersect
        /// </search>
        public static int SelfIntersect(ClashReport ClashReport)
        {
            return ClashReport.pSelfIntersect;
        }

        /// <summary>
        /// Gets type of Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Type">Clash Report Type</returns>
        /// <search>
        /// Clash Report, Clash Report Type, Navisworks, Type, clash report, clash report type, navisworks, type
        /// </search>
        public static string Type(ClashReport ClashReport)
        {
            return ClashReport.pType;
        }

        /// <summary>
        /// Gets status of Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Status">Clash Report Status</returns>
        /// <search>
        /// Clash Report, Clash Report Status, Navisworks, Status, clash report, clash report status, navisworks, status
        /// </search>
        public static string Status(ClashReport ClashReport)
        {
            return ClashReport.pStatus;
        }

        /// <summary>
        /// Gets clash groups in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Groups">Clash Report Groups</returns>
        /// <search>
        /// Clash Report, Clash Report Clash Groups, Navisworks, Clash Groups, clash report, clash report clash groups, navisworks, clash groups
        /// </search>
        public static List<ClashGroup> ClashGroups(ClashReport ClashReport)
        {
            return ClashReport.pClashItemList.FindAll(x => x is ClashGroup).Cast<ClashGroup>().ToList();
        }

        /// <summary>
        /// Gets clashes in Clash Report 
        /// </summary>
        /// <param name="ClashReport">Clash Report</param>
        /// <returns name="Clashes">Clashes</returns>
        /// <search>
        /// Clash Report, Clash Report Clashes, Navisworks, Clash Groups, clash report, clash report clashes, navisworks, clashes
        /// </search>
        public static List<Clash> Clashes(ClashReport ClashReport)
        {
            return ClashReport.pClashItemList.FindAll(x => x is Clash).Cast<Clash>().ToList();
        }
    }

    /// <summary>
    /// Clash item generated from Naviswork clash report file
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public abstract class ClashItem
    {
        protected string pName;
        protected string pDistance;
        protected string pGridLocation;
        protected string pStatus;
        protected string pImage;
        protected string pClashPoint;
        protected string[] pElementIDs = new string[2];
        protected string[] pLayers = new string[2];
        protected string[] pItemNames = new string[2];
        protected string[] pItemTypes = new string[2];
        protected string[] pPaths = new string[2];
        protected string pDescription;
        protected string pDateCreated;
        protected List<Comment> pCommentList;

        protected ClashItem(HtmlNode HtmlNode)
        {
            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants().Where(x => x.ParentNode == HtmlNode).ToList();
            int aIndex = 0;
            int aElementIndex = 0;
            while (aIndex < aHtmlNodeList.Count)
            {
                HtmlNode aHtmlNode = aHtmlNodeList[aIndex];
                if (aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == HTML.Names.Class.NameValuePair)
                {
                    ReadParameter(aHtmlNode);
                    aIndex++;
                }
                else if (aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == HTML.Names.Class.ClashObject)
                {
                    aIndex = ReadClashObject(aHtmlNodeList, aIndex, aElementIndex);
                    aElementIndex++;
                }
                else
                    aIndex++;
            }
            pCommentList = HTML.Functions.GetCommentList(HtmlNode);
            pImage = HTML.Functions.GetImage(HtmlNode);
        }

        internal ClashItem(string Image, string Name, string Status, string Distance, string GridLocation, string Description, string DateCreated, string ClashPoint, string ItemID_1, string Layer_1, string Path_1, string ItemName_1, string ItemType_1, string ItemID_2, string Layer_2, string Path_2, string ItemName_2, string ItemType_2)
        {
            pImage = Image;
            pName = Name;
            pStatus = Status;
            pDistance = Distance;
            pGridLocation = GridLocation;
            pDescription = Description;
            pDateCreated = DateCreated;
            pClashPoint = ClashPoint;
            pElementIDs[0] = ItemID_1;
            pLayers[0] = Layer_1;
            pPaths[0] = Path_1;
            pItemNames[0] = ItemName_1;
            pItemTypes[0] = ItemType_1;
            pElementIDs[1] = ItemID_2;
            pLayers[1] = Layer_2;
            pPaths[1] = Path_2;
            pItemNames[1] = ItemName_2;
            pItemTypes[1] = ItemType_2;
        }

        protected abstract void ReadParameter(string Name, string Value);

        private void ReadParameter(HtmlNode HtmlNode)
        {
            string aName = null;
            string aValue = null;
            if (HTML.Functions.NameValuePair(HtmlNode, out aName, out aValue))
            {
                if (aName.StartsWith(HTML.Names.ClashItem.Name))
                    pName = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.Distance))
                    pDistance = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.Description))
                    pDescription = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.Status))
                    pStatus = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.ClashPoint))
                    pClashPoint = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.GridLocation))
                    pGridLocation = aValue;
                else if (aName.StartsWith(HTML.Names.ClashItem.DateCreated))
                    pDateCreated = aValue;
                else
                    ReadParameter(aName, aValue);


            }
        }

        private int ReadClashObject(List<HtmlNode> HtmlNodeList, int Index, int ElementIndex)
        {
            int aIndex = Index + 1;
            while (aIndex < HtmlNodeList.Count)
            {
                HtmlNode aHtmlNode = HtmlNodeList[aIndex];

                if (aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == HTML.Names.Class.ClashObject)
                    break;

                string aName = null;
                string aValue = null;
                if (HTML.Functions.NameValuePair(aHtmlNode, out aName, out aValue))
                {
                    if (aName.StartsWith(HTML.Names.ClashItem.ElementID))
                        pElementIDs[ElementIndex] = aValue;
                    else if (aName.StartsWith(HTML.Names.ClashItem.Layer))
                        pLayers[ElementIndex] = aValue;
                    else if (aName.StartsWith(HTML.Names.ClashItem.ItemType))
                        pItemTypes[ElementIndex] = aValue;
                    else if (aName.StartsWith(HTML.Names.ClashItem.ItemName))
                        pItemNames[ElementIndex] = aValue;
                    else if (aName.StartsWith(HTML.Names.ClashItem.Path))
                        pPaths[ElementIndex] = aValue;
                }

                aIndex++;
            }

            return aIndex;
        }
    } 
    
    /// <summary>
    /// Clash group generated from Naviswork clash report file
    /// </summary>
    public class ClashGroup : ClashItem
    {
        private List<Clash> pClashList = new List<Clash>();

        internal ClashGroup(HtmlNode HtmlNode)
            : base(HtmlNode)
        {
            pClashList = new List<Clash>();
            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == HTML.Names.Class.ViewPoint && x.ParentNode == HtmlNode).ToList();
            foreach (HtmlNode aHtmlNode in aHtmlNodeList)
                pClashList.Add(new Clash(aHtmlNode));
        }

        internal ClashGroup(string Image, string Name, string Status, string Distance, string GridLocation, string Description, string DateCreated, string ClashPoint, string ItemID_1, string Layer_1, string Path_1, string ItemName_1, string ItemType_1, string ItemID_2, string Layer_2, string Path_2, string ItemName_2, string ItemType_2)
            : base(Image, Name, Status, Distance, GridLocation, Description, DateCreated, ClashPoint, ItemID_1, Layer_1, Path_1, ItemName_1, ItemType_1, ItemID_2, Layer_2, Path_2, ItemName_2, ItemType_2)
        {
        }

        private void Initialize()
        {
            string aValue = null;
            pName = aValue;
            pDistance = aValue;
            pStatus = aValue;
            pClashPoint = aValue;
            pGridLocation = aValue;
            pElementIDs = new string[] { aValue, aValue };
            pLayers = new string[] { aValue, aValue };
            pItemNames = new string[] { aValue, aValue };
            pItemTypes = new string[] { aValue, aValue };
        }

        protected override void ReadParameter(string Name, string Value)
        {

        }

        /// <summary>
        /// Gets clash group clash point
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Point">Clash Group Clash Point</returns>
        /// <search>
        /// Clash Report, Clash Group Clash Point, Navisworks, Clash Point, clash report, clash group clash point, navisworks, clash point
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point ClashPoint(ClashGroup ClashGroup)
        {
            return HTML.Functions.GetPoint(ClashGroup.pClashPoint);
        }

        /// <summary>
        /// Gets clashes from clash group
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Clashes">Clash Group Clashes</returns>
        /// <search>
        /// Clash Report, Clash Group Clashes, Navisworks, Clashes, clash report, clash group clashes, navisworks, clashes
        /// </search>
        public static List<Clash> Clashes(ClashGroup ClashGroup)
        {
            return ClashGroup.pClashList;
        }

        /// <summary>
        /// Gets clash group name
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Name">Clash Group Name</returns>
        /// <search>
        /// Clash Report, Clash Group Name, Navisworks, Name, clash report, clash group name, navisworks, name
        /// </search>
        public static string Name(ClashGroup ClashGroup)
        {
            return ClashGroup.pName;
        }

        /// <summary>
        /// Gets clash group distance
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Distance">Clash Group Distance</returns>
        /// <search>
        /// Clash Report, Clash Group Distance, Navisworks, Distance, clash report, clash group distance, navisworks, distance
        /// </search>
        public static string Distance(ClashGroup ClashGroup)
        {
            return ClashGroup.pDistance;
        }

        /// <summary>
        /// Gets clash group status
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Status">Clash Group Status</returns>
        /// <search>
        /// Clash Report, Clash Group Status, Navisworks, Status, clash report, clash group status, navisworks, status
        /// </search>
        public static string Status(ClashGroup ClashGroup)
        {
            return ClashGroup.pStatus;
        }

        /// <summary>
        /// Gets clash group grid location
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Location">Clash Group Grid Location</returns>
        /// <search>
        /// Clash Report, Clash Group Grid Location, Navisworks, Grid Location, clash report, clash group grid location, navisworks, grid location, GridLocation, gridlocation
        /// </search>
        public static string GridLocation(ClashGroup ClashGroup)
        {
            return ClashGroup.pGridLocation;
        }

        /// <summary>
        /// Gets clash group Element ID
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="ElementID">Clash Group ElementID</returns>
        /// <search>
        /// Clash Report, Clash Group Element ID, Navisworks, Element ID, clash report, clash group element id, navisworks, element id, elementid, ElementID
        /// </search>
        public static string ElementID(ClashGroup ClashGroup, int Index)
        {
            return ClashGroup.pElementIDs[Index];
        }

        /// <summary>
        /// Gets clash group element layer
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Layer">Clash Group Element Layer</returns>
        /// <search>
        /// Clash Report, Clash Group Element Layer, Navisworks, Element Layer, clash report, clash group element layer, navisworks, layer
        /// </search>
        public static string Layer(ClashGroup ClashGroup, int Index)
        {
            return ClashGroup.pLayers[Index];
        }

        /// <summary>
        /// Gets clash group item name
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Name">Clash Group Item Name</returns>
        /// <search>
        /// Clash Report, Clash Group Item Name, Navisworks, Item Name, clash report, clash group item name, navisworks, item name, itemname, ItemName
        /// </search>
        public static string ItemName(ClashGroup ClashGroup, int Index)
        {
            return ClashGroup.pItemNames[Index];
        }

        /// <summary>
        /// Gets clash group item type
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Type">Clash Group Item Type</returns>
        /// <search>
        /// Clash Report, Clash Group Item Type, Navisworks, Item Type, clash report, clash group item type, navisworks, item type, ItemType, itemtype
        /// </search>
        public static string ItemType(ClashGroup ClashGroup, int Index)
        {
            return ClashGroup.pItemTypes[Index];
        }

        /// <summary>
        /// Gets comments for clash group
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Comments">Clash Group Comments</returns>
        /// <search>
        /// Clash Report, Clash Group Comments, Navisworks, Comments, clash report, clash group comments, navisworks, comments
        /// </search>
        public static List<Comment> Comments(ClashGroup ClashGroup)
        {
            return ClashGroup.pCommentList;
        }

        /// <summary>
        /// Gets clash group image path
        /// </summary>
        /// <param name="ClashGroup">Clash Group</param>
        /// <returns name="Path">Clash Group image path</returns>
        /// <search>
        /// Clash Report, Clash Group Image, Navisworks, Image, clash report, clash group image, navisworks, image
        /// </search>
        public static string Image(ClashGroup ClashGroup)
        {
            return ClashGroup.pImage;
        }

    }

    /// <summary>
    /// Clash generated from Naviswork clash report file
    /// </summary>
    public class Clash : ClashItem
    {
        private string pClashGroup;
        private string pAssignedTo;
        
        internal Clash(HtmlNode HtmlNode)
            : base(HtmlNode)
        {

        }

        internal Clash(string Image, string Name, string Status, string Distance, string GridLocation, string Description, string DateCreated, string ClashPoint, string ItemID_1, string Layer_1, string Path_1, string ItemName_1, string ItemType_1, string ItemID_2, string Layer_2, string Path_2, string ItemName_2, string ItemType_2)
            : base(Image, Name, Status, Distance, GridLocation, Description, DateCreated, ClashPoint, ItemID_1, Layer_1, Path_1, ItemName_1, ItemType_1, ItemID_2, Layer_2, Path_2, ItemName_2, ItemType_2)
        {
        }

        protected override void ReadParameter(string Name, string Value)
        {
            if (Name.StartsWith(HTML.Names.Clash.AssignedTo))
                pAssignedTo = Value;
            else if (Name.StartsWith(HTML.Names.Clash.ClashGroup))
                pClashGroup = Value;
        }

        /// <summary>
        /// Gets clash group name
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Name">Clash Group Name</returns>
        /// <search>
        /// Clash Report, Clash Group Name, Navisworks, Group Name, clash report, clash group name, navisworks, clash group name, clashgroup, ClashGroup
        /// </search>
        public static string ClashGroup(Clash Clash)
        {
            return Clash.pClashGroup;
        }

        /// <summary>
        /// Gets clash description
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Description">Clash Description</returns>
        /// <search>
        /// Clash Report, Clash Description, Navisworks, Description, clash report, clash description, navisworks, description
        /// </search>
        public static string Description(Clash Clash)
        {
            return Clash.pDescription;
        }

        /// <summary>
        /// Gets clash point
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Point">Clash Point</returns>
        /// <search>
        /// Clash Report, Clash Point, Navisworks, clash report, navisworks, clas point, ClashPoint, clashpoint
        /// </search>
        public static Autodesk.DesignScript.Geometry.Point ClashPoint(Clash Clash)
        {
            return HTML.Functions.GetPoint(Clash.pClashPoint);
        }

        /// <summary>
        /// Gets clash Date Created
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Date">Clash Date Created</returns>
        /// <search>
        /// Clash Report, Clash Date Created, Navisworks, Date Created, clash report, clash date created, navisworks, date created, DateCreated, datecreated
        /// </search>
        public static string DateCreated(Clash Clash)
        {
            return Clash.pDateCreated;
        }

        /// <summary>
        /// Gets clash assigned to
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="AssignedTo">Clash Assigned To</returns>
        /// <search>
        /// Clash Report, Clash Assigned To, Navisworks, Assigned To, clash report, clash assigned to, navisworks, assigned to, AssignedTo, assignedto
        /// </search>
        public static string AssignedTo(Clash Clash)
        {
            return Clash.pAssignedTo;
        }

        /// <summary>
        /// Gets clash name
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Name">Clash Name</returns>
        /// <search>
        /// Clash Report, Clash Name, Navisworks, Name, clash report, clash name, navisworks, name
        /// </search>
        public static string Name(Clash Clash)
        {
            return Clash.pName;
        }

        /// <summary>
        /// Gets clash distance
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Distance">Clash Distance</returns>
        /// <search>
        /// Clash Report, Clash Distance, Navisworks, Distance, clash report, clash distance, navisworks, distance
        /// </search>
        public static string Distance(Clash Clash)
        {
            return Clash.pDistance;
        }

        /// <summary>
        /// Gets clash status
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Status">Clash Status</returns>
        /// <search>
        /// Clash Report, Clash Status, Navisworks, Status, clash report, clash status, navisworks, status
        /// </search>
        public static string Status(Clash Clash)
        {
            return Clash.pStatus;
        }

        /// <summary>
        /// Gets clash grid location
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Location">Clash Location</returns>
        /// <search>
        /// Clash Report, Clash Grid Location, Navisworks, Grid Location, clash report, clash grid location, navisworks, grid location
        /// </search>
        public static string GridLocation(Clash Clash)
        {
            return Clash.pGridLocation;
        }

        /// <summary>
        /// Gets clash Element ID
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="ElementID">Clash Element ID</returns>
        /// <search>
        /// Clash Report, Clash Element ID, Navisworks, Element ID, clash report, clash element id, navisworks, element id, elelemntid, ElementID
        /// </search>
        public static string ElementID(Clash Clash, int Index)
        {
            return Clash.pElementIDs[Index];
        }

        /// <summary>
        /// Gets clash element layer
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Layer">Clash Layer</returns>
        /// <search>
        /// Clash Report, ClashElement Layer, Navisworks, Element Layer, clash report, clash element layer, navisworks, layer
        /// </search>
        public static string Layer(Clash Clash, int Index)
        {
            return Clash.pLayers[Index];
        }

        /// <summary>
        /// Gets clash item name
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Name">Clash Item Name</returns>
        /// <search>
        /// Clash Report, Clash Item Name, Navisworks, Item Name, clash report, clash item name, navisworks, item name, itemname, ItemName
        /// </search>
        public static string ItemName(Clash Clash, int Index)
        {
            return Clash.pItemNames[Index];
        }

        /// <summary>
        /// Gets clash item type
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Type">Clash Type Name</returns>
        /// <search>
        /// Clash Report, Clash Item Type, Navisworks, Item Type, clash report, clash item type, navisworks, item type, ItemType, itemtype
        /// </search>
        public static string ItemType(Clash Clash, int Index)
        {
            return Clash.pItemTypes[Index];
        }

        /// <summary>
        /// Gets comments for clash
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Comments">Clash Comments</returns>
        /// <search>
        /// Clash Report, Clash Comments, Navisworks, Comments, clash report, clash comments, navisworks, comments
        /// </search>
        public static List<Comment> Comments(Clash Clash)
        {
            return Clash.pCommentList;
        }

        /// <summary>
        /// Gets path for item
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <param name="Index">Item index (0 - Item 1, 1 - Item 2)</param>
        /// <returns name="Path">Element Path</returns>
        /// <search>
        /// Clash Report, Element Path, Navisworks, Path, clash report, element path, navisworks, path
        /// </search>
        public static List<string> Path(Clash Clash, int Index)
        {
            if (Clash.pPaths[Index] == null)
                return null;
            return Clash.pPaths[Index].Split(new string[1] {" -&gt;"}, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// Gets clash image path
        /// </summary>
        /// <param name="Clash">Clash</param>
        /// <returns name="Path">Clash image path</returns>
        /// <search>
        /// Clash Report, Clash Image, Navisworks, Image, clash report, clash image, navisworks, image
        /// </search>
        public static string Image(Clash Clash)
        {
            return Clash.pImage;
        }
    }

    /// <summary>
    /// Naviswork Clash comment
    /// </summary>
    public class Comment
    {
        private string pStatus;
        private string pUser;
        private string pText;
        private string pDate;

        internal Comment(HtmlNode HtmlNode)
        {
            if (HtmlNode != null)
            {
                Dictionary<string, string> aNameValueDictionary = HTML.Functions.GetNameValuePairs(HtmlNode);

                foreach (KeyValuePair<string, string> NameValuePair in aNameValueDictionary)
                {
                    if (NameValuePair.Key.StartsWith(HTML.Names.Comment.Status))
                        pStatus = NameValuePair.Value;
                    else if (NameValuePair.Key.StartsWith(HTML.Names.Comment.User))
                        pUser = NameValuePair.Value;
                    else if (NameValuePair.Key.StartsWith(HTML.Names.Comment.Text))
                        pText = NameValuePair.Value;
                }

                HtmlNode aHtmlNode = HtmlNode.LastChild;
                if(aHtmlNode != null)
                    pDate = aHtmlNode.InnerText.Trim();
            }
        }

        /// <summary>
        /// Gets comment status
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns name="Status">Comment user</returns>
        /// <search>
        /// Clash Report, Comment, Navisworks, Comment Status, clash report, comment, navisworks, comment status, comment
        /// </search>
        public static string Status(Comment Comment)
        {
            return Comment.pStatus;
        }

        /// <summary>
        /// Gets comment user
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns name="User">Comment user</returns>
        /// <search>
        /// Clash Report, Comment, Navisworks, Comment User, clash report, comment, navisworks, comment user, comment
        /// </search>
        public static string User(Comment Comment)
        {
            return Comment.pUser;
        }

        /// <summary>
        /// Gets comment text
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns name="Text">Comment text</returns>
        /// <search>
        /// Clash Report, Comment, Navisworks, Comment Text, clash report, comment, navisworks, comment text, comment
        /// </search>
        public static string Text(Comment Comment)
        {
            return Comment.pText;
        }

        /// <summary>
        /// Gets comment date
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns name="Date">Comment text</returns>
        /// <search>
        /// Clash Report, Comment, Navisworks, Comment Date, clash report, comment, navisworks, comment date, comment
        /// </search>
        public static string Date(Comment Comment)
        {
            return Comment.pDate;
        }
    }

    internal static class HTML
    {
        internal static class Names
        {
            internal static class Class
            {
                public static string ClashReport = "animation";
                public static string NameValuePair = "namevaluepair";
                public static string Name = "name";
                public static string Value = "value";
                public static string ClashGroup = "clashgroup";
                public static string ViewPoint = "viewpoint";
                public static string ClashObject = "clashobject";
                public static string Comment = "comment";
                public static string Comments = "comments";
            }

            internal static class ClashReport
            {
                public static string Tolerance = "Tolerance";
                public static string Total = "Total";
                public static string New = "New";
                public static string Active = "Active";
                public static string Reviewed = "Reviewed";
                public static string Approved = "Approved";
                public static string Resolved = "Resolved";
                public static string Type = "Type";
                public static string Status = "Status";
                public static string SelfIntersect = "Self Intersect";
            }

            internal static class ClashItem
            {
                public static string Name = "Name";
                public static string Distance = "Distance";
                public static string Description = "Description";
                public static string Status = "Status";
                public static string ClashPoint = "Clash Point";
                public static string GridLocation = "Grid Location";
                public static string ElementID = "Element ID";
                public static string Layer = "Layer";
                public static string ItemName = "Item Name";
                public static string ItemType = "Item Type";
                public static string DateCreated = "Date Created";
                public static string Path = "Path";
            }

            internal static class ClashGroup
            {
                public static string ClashObject = "clashobject";
            }

            internal static class Clash
            {
                public static string ClashGroup = "Clash Group";      
                public static string AssignedTo = "Assigned To";
            }

            internal static class Comment
            {
                public static string Status = "Status";
                public static string User = "User";
                public static string Text = "Text";
            }
        }

        internal static class Functions
        {
            internal static List<HtmlNode> GetNameValuePairsNodeList(HtmlNode HtmlNode)
            {
                return HtmlNode.Descendants("span").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == Names.Class.NameValuePair && x.ParentNode == HtmlNode).ToList();
            }

            internal static Dictionary<string, string> GetNameValuePairs(HtmlNode HtmlNode)
            {
                Dictionary<string, string> aResult = new Dictionary<string, string>();
                List<HtmlNode> aHtmlNodeList = GetNameValuePairsNodeList(HtmlNode);
                foreach (HtmlNode aHtmlNode in aHtmlNodeList)
                {
                    string aName = null;
                    string aValue = null;
                    if (NameValuePair(aHtmlNode, out aName, out aValue))
                        if (!aResult.ContainsKey(aName))
                            aResult.Add(aName, aValue);
                }
                return aResult;
            }

            internal static bool NameValuePair(HtmlNode HtmlNode, out string Name, out string Value)
            {
                List<HtmlNode> aTemporaryHtmlNodeList = HtmlNode.Descendants("span").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == Names.Class.Name && x.ParentNode == HtmlNode).ToList();
                if (aTemporaryHtmlNodeList != null && aTemporaryHtmlNodeList.Count > 0)
                {
                    string aName = aTemporaryHtmlNodeList.First().InnerText;
                    aTemporaryHtmlNodeList = HtmlNode.Descendants("span").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == Names.Class.Value).ToList();
                    if (aTemporaryHtmlNodeList != null && aTemporaryHtmlNodeList.Count > 0)
                    {
                        string aValue = aTemporaryHtmlNodeList.First().InnerText;
                        Name = aName;
                        Value = aValue;
                        return true;
                    }
                }
                Name = null;
                Value = null;
                return false;
            }

            internal static Autodesk.DesignScript.Geometry.Point GetPoint(string PointString)
            {
                if (PointString == null)
                    return null;

                PointString = new string(PointString.Where(c => (char.IsDigit(c) || c == '.' || c == ',')).ToArray());
                string[] aStringValues = PointString.Split(',');
                if (aStringValues.Length == 3)
                {
                    double[] aDoubleValues = new double[3] { double.NaN, double.NaN, double.NaN };
                    double.TryParse(aStringValues[0], out aDoubleValues[0]);
                    double.TryParse(aStringValues[1], out aDoubleValues[1]);
                    double.TryParse(aStringValues[2], out aDoubleValues[2]);
                    return Autodesk.DesignScript.Geometry.Point.ByCoordinates(aDoubleValues[0], aDoubleValues[1], aDoubleValues[2]);
                }
                return Autodesk.DesignScript.Geometry.Point.ByCoordinates(0, 0, 0);
            }

            internal static List<Comment> GetCommentList(HtmlNode HtmlNode)
            {
                List<Comment> aCommentList = null;
                List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == Names.Class.Comments && x.ParentNode == HtmlNode).ToList();
                if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                {
                    aCommentList = new List<Comment>();
                    HtmlNode aHtmlNode = aHtmlNodeList.First();
                    aHtmlNodeList = aHtmlNode.Descendants("div").Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == Names.Class.Comment && x.ParentNode == aHtmlNode).ToList();
                    foreach (HtmlNode aCommentHtmlNode in aHtmlNodeList)
                        aCommentList.Add(new Comment(aCommentHtmlNode));
                }
                return aCommentList;
            }

            internal static string GetImage(HtmlNode HtmlNode)
            {
                string aResult = null;
                List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("a").Where(x => x.ParentNode == HtmlNode).ToList();
                if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                {
                    HtmlNode aHtmlNode = aHtmlNodeList.First();
                    aHtmlNodeList = aHtmlNode.Descendants("img").Where(x => x.ParentNode == aHtmlNode).ToList();
                    if (aHtmlNodeList != null && aHtmlNodeList.Count > 0)
                    {
                        aHtmlNode = aHtmlNodeList.First();
                        HtmlAttribute aHtmlAttribute = aHtmlNode.Attributes["src"];
                        if (aHtmlAttribute != null)
                            return aHtmlAttribute.Value;
                    }
                }

                return aResult;
            }
        }
    }

    internal static class HTML_Tabular
    {
        internal static class Names
        {
            internal static class Class
            {
                internal static string TestSummaryTable = "testSummaryTable";
                internal static string MainTable = "mainTable";
                internal static string ContentRow = "contentRow";
                internal static string HeaderRow = "headerRow";
                internal static string GroupRow = "clashGroupRow";
            }

            internal static class ClashReport
            {
                public static string Tolerance = "Tolerance";
                public static string Total = "Clashes";
                public static string New = "New";
                public static string Active = "Active";
                public static string Reviewed = "Reviewed";
                public static string Approved = "Approved";
                public static string Resolved = "Resolved";
                public static string Type = "Type";
                public static string Status = "Status";
            }
        }
    }

    internal class Header
    {
        private enum Names
        {
            Image,
            ClashName,
            Status,
            Distance,
            GridLocation,
            Description,
            DateFound,
            ClashPoint,
            ItemID_1,
            Layer_1,
            Path_1,
            ItemName_1,
            ItemType_1,
            ItemID_2,
            Layer_2,
            Path_2,
            ItemName_2,
            ItemType_2
        }

        private int[] pIndexes = new int[Enum.GetNames(typeof(Names)).Length];

        internal Header(HtmlNode HtmlNode)
        {
            for (int i = 0; i < pIndexes.Length; i++)
                pIndexes[i] = -1;

            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("td").Where(x => x.ParentNode == HtmlNode).ToList();
            for (int i =0; i < aHtmlNodeList.Count; i++)
            {
                HtmlNode aHtmlNode = aHtmlNodeList[i];
                if (aHtmlNode.InnerText.StartsWith("Image"))
                    pIndexes[(int)Names.Image] = i;
                else if (aHtmlNode.InnerText.StartsWith("Clash Name"))
                    pIndexes[(int)Names.ClashName] = i;
                else if (aHtmlNode.InnerText.StartsWith("Status"))
                    pIndexes[(int)Names.Status] = i;
                else if (aHtmlNode.InnerText.StartsWith("Distance"))
                    pIndexes[(int)Names.Distance] = i;
                else if (aHtmlNode.InnerText.StartsWith("Grid Location"))
                    pIndexes[(int)Names.GridLocation] = i;
                else if (aHtmlNode.InnerText.StartsWith("Description"))
                    pIndexes[(int)Names.Description] = i;
                else if (aHtmlNode.InnerText.StartsWith("Date Found"))
                    pIndexes[(int)Names.DateFound] = i;
                else if (aHtmlNode.InnerText.StartsWith("Clash Point"))
                    pIndexes[(int)Names.ClashPoint] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item ID") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item1Header")
                    pIndexes[(int)Names.ItemID_1] = i;
                else if (aHtmlNode.InnerText.StartsWith("Layer") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item1Header")
                    pIndexes[(int)Names.Layer_1] = i;
                else if (aHtmlNode.InnerText.StartsWith("Path") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item1Header")
                    pIndexes[(int)Names.Path_1] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item Name") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item1Header")
                    pIndexes[(int)Names.ItemName_1] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item Type") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item1Header")
                    pIndexes[(int)Names.ItemType_1] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item ID") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item2Header")
                    pIndexes[(int)Names.ItemID_2] = i;
                else if (aHtmlNode.InnerText.StartsWith("Layer") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item2Header")
                    pIndexes[(int)Names.Layer_2] = i;
                else if (aHtmlNode.InnerText.StartsWith("Path") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item2Header")
                    pIndexes[(int)Names.Path_2] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item Name") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item2Header")
                    pIndexes[(int)Names.ItemName_2] = i;
                else if (aHtmlNode.InnerText.StartsWith("Item Type") && aHtmlNode.Attributes.Contains("class") && aHtmlNode.Attributes["class"].Value == "item2Header")
                    pIndexes[(int)Names.ItemType_2] = i;
            }

        }

        private string[] GetValues(HtmlNode HtmlNode)
        {
            List<HtmlNode> aHtmlNodeList = HtmlNode.Descendants("td").Where(x => x.ParentNode == HtmlNode).ToList();
            string[] aValues = new string[pIndexes.Length];
            for (int i = 0; i < pIndexes.Length; i++)
            {
                int aIndex = pIndexes[i];
                if (aIndex != -1 && aIndex < aHtmlNodeList.Count)
                {
                    if (i == (int)Names.Image)
                    {
                        List<HtmlNode> aAHtmlNodeList = aHtmlNodeList[aIndex].Descendants("a").Where(x => x.ParentNode == aHtmlNodeList[aIndex]).ToList();
                        if (aAHtmlNodeList != null && aAHtmlNodeList.Count > 0)
                        {
                            HtmlNode aHtmlNode = aAHtmlNodeList.First();
                            if (aHtmlNode.Attributes.Contains("href"))
                                aValues[i] = aHtmlNode.Attributes["href"].Value;
                        }
                    }
                    else
                    {
                        aValues[i] = aHtmlNodeList[aIndex].InnerText;
                    }
                }
                else
                {
                    aValues[i] = null;
                }
            }

            return aValues;
        }

        internal Clash GetClash(HtmlNode HtmlNode)
        {
            string[] aValues = GetValues(HtmlNode);
            return new Clash(aValues[(int)Names.Image], aValues[(int)Names.ClashName], aValues[(int)Names.Status], aValues[(int)Names.Distance], aValues[(int)Names.GridLocation], aValues[(int)Names.Description], aValues[(int)Names.DateFound], aValues[(int)Names.ClashPoint], aValues[(int)Names.ItemID_1], aValues[(int)Names.Layer_1], aValues[(int)Names.Path_1], aValues[(int)Names.ItemName_1], aValues[(int)Names.ItemType_1], aValues[(int)Names.ItemID_2], aValues[(int)Names.Layer_2], aValues[(int)Names.Path_2], aValues[(int)Names.ItemName_2], aValues[(int)Names.ItemType_2]);
        }

        internal ClashGroup GetClashGroup(HtmlNode HtmlNode)
        {
            string[] aValues = GetValues(HtmlNode);
            return new ClashGroup(aValues[(int)Names.Image], aValues[(int)Names.ClashName], aValues[(int)Names.Status], aValues[(int)Names.Distance], aValues[(int)Names.GridLocation], aValues[(int)Names.Description], aValues[(int)Names.DateFound], aValues[(int)Names.ClashPoint], aValues[(int)Names.ItemID_1], aValues[(int)Names.Layer_1], aValues[(int)Names.Path_1], aValues[(int)Names.ItemName_1], aValues[(int)Names.ItemType_1], aValues[(int)Names.ItemID_2], aValues[(int)Names.Layer_2], aValues[(int)Names.Path_2], aValues[(int)Names.ItemName_2], aValues[(int)Names.ItemType_2]);
        }
    }
}
