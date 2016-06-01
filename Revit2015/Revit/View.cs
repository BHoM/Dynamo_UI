using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.Elements;

namespace Revit
{
    /// <summary>
    /// A View
    /// </summary>
    public static class View
    {
        /// <summary>
        /// Returns the closest level to point
        /// </summary>
        /// <param name="View">View</param>
        /// <param name="Category">Category</param>
        /// <returns name="Elements">Elements</returns>
        /// <search>
        /// View Elements, View, view, get view elements by category 
        /// </search>
        public static List<Elements.Element> GetElements(Elements.Views.View View,  Elements.Category Category)
        {
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
            Autodesk.Revit.DB.Document aDocument = aView.Document;
            List<Autodesk.Revit.DB.Element> aElementList = new FilteredElementCollector(aDocument, aView.Id).OfCategory((BuiltInCategory)Category.Id).ToElements().ToList();
            return aElementList.ConvertAll(x => x.ToDSType(true));
        }

        /// <summary>
        /// Gets View Type
        /// </summary>
        /// <param name="View">View</param>
        /// <returns name="Elements">Elements</returns>
        /// <search>
        /// View Elements, View, view, get view type, ViewType
        /// </search>
        public static string ViewType(Elements.Views.View View)
        {
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
            return aView.ViewType.ToString();
        }

        /// <summary>
        /// Gets View Template
        /// </summary>
        /// <param name="View">View</param>
        /// <returns name="Element">View Template</returns>
        /// <search>
        /// View Elements, View, view, get view template, GetViewTemplate
        /// </search>
        public static Elements.Element GetViewTemplate(Elements.Views.View View)
        {
            Autodesk.Revit.DB.View aView = View.InternalElement as Autodesk.Revit.DB.View;
            return aView.Document.GetElement(aView.ViewTemplateId).ToDSType(true);
        }

        /// <summary>
        /// Checks if View is View template 
        /// </summary>
        /// <param name="Element">Element</param>
        /// <returns name="IsTemplate">Is Template</returns>
        /// <search>
        /// Revit, View, Is Template, istemplate
        /// </search>
        public static bool IsTemplate(Elements.Element Element)
        {
            Autodesk.Revit.DB.View aView = Element.InternalElement as Autodesk.Revit.DB.View;
            return aView.IsTemplate;

        }

        /// <summary>
        /// Export View to Naviswork file
        /// </summary>
        /// <param name="View">View</param>
        /// <param name="Directory">File Directory</param>
        /// <param name="Name">File name</param>
        /// <param name="ConvertElementProperties">Convert Element Properties</param>
        /// <param name="UseSharedCoordinates">Use Shared Coordinates</param>
        /// <param name="DivideFileIntoLevels">Divide File Into Levels</param>
        /// <param name="ExportElementIds">Export Element Ids</param>
        /// <param name="ExportLinks">Export Links</param>
        /// <param name="ExportParts">Export Parts</param>
        /// <param name="ExportRoomAsAttribute">Export Room As Attribute</param>
        /// <param name="ExportRoomGeometry">Export Room Geometry</param>
        /// <param name="ExportUrls">Export Urls</param>
        /// <param name="FindMissingMaterials">Find Missing Materials</param>
        /// <returns name="View">View</returns>
        /// <search>
        /// View Elements, View, view, NWCExport, Export, Navisowrks
        /// </search>
        public static Elements.Element NWCExport(Elements.Element View, string Directory, string Name, bool ConvertElementProperties = false, bool UseSharedCoordinates = true, bool DivideFileIntoLevels = true, bool ExportElementIds = true, bool ExportLinks = false, bool ExportParts = false, bool ExportRoomAsAttribute = true, bool ExportRoomGeometry = true, bool ExportUrls = true, bool FindMissingMaterials = true)
        {
            NavisworksExportOptions aNavisworksExportOptions = new NavisworksExportOptions();
            aNavisworksExportOptions.ExportScope = NavisworksExportScope.View;
            aNavisworksExportOptions.ViewId = View.InternalElement.Id;
            aNavisworksExportOptions.ConvertElementProperties = ConvertElementProperties;
            if (UseSharedCoordinates)
                aNavisworksExportOptions.Coordinates = NavisworksCoordinates.Shared;
            else
                aNavisworksExportOptions.Coordinates = NavisworksCoordinates.Internal;

            aNavisworksExportOptions.DivideFileIntoLevels = DivideFileIntoLevels;
            aNavisworksExportOptions.ExportElementIds = ExportElementIds;
            aNavisworksExportOptions.ExportLinks = ExportLinks;
            aNavisworksExportOptions.ExportParts = ExportParts;
            aNavisworksExportOptions.ExportRoomAsAttribute = ExportRoomAsAttribute;
            aNavisworksExportOptions.ExportRoomGeometry = ExportRoomGeometry;
            aNavisworksExportOptions.ExportUrls = ExportUrls;
            aNavisworksExportOptions.FindMissingMaterials = FindMissingMaterials;
            aNavisworksExportOptions.Parameters = NavisworksParameters.All;

            View.InternalElement.Document.Export(Directory, Name, aNavisworksExportOptions);
            return View;
        }
    }
}
