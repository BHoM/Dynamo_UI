using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.GeometryConversion;

namespace Revit
{
    /// <summary>
    /// A Revit Independent Tag (Tag)
    /// </summary>
    public static class IndependentTag
    {
        /// <summary>
        /// Creates floor based on Curves
        /// </summary>
        /// <param name="IndependentTag">Independent Tag</param>
        /// <returns name="Text">Tag Text</returns>
        /// <search>
        /// Get tag text, Get independenttag text, get independent tag text
        /// </search>
        public static string Text(Elements.Element IndependentTag)
        {
            Autodesk.Revit.DB.IndependentTag aIndependentTag = IndependentTag.InternalElement as Autodesk.Revit.DB.IndependentTag;
            if (!aIndependentTag.IsOrphaned)
                return aIndependentTag.TagText;
            return null;
        }

        /// <summary>
        /// Creates tag by view and element
        /// </summary>
        /// <param name="View">View</param>
        /// <param name="Element">Element to be tagged</param>
        /// <param name="Point">Point</param>
        /// <param name="Horizontal">Horizontal tag orientation</param>
        /// <param name="AddLeader">Add Leader</param>
        /// <returns name="Tag">Element Tag</returns>
        /// <search>
        /// Tag, Revit, ByViewAndElement, By View And Element
        /// </search>
        public static Elements.Element ByViewAndElement(Elements.Views.View View, Elements.Element Element, Autodesk.DesignScript.Geometry.Point Point, bool Horizontal = true, bool AddLeader = true)
        {
            Autodesk.Revit.DB.Document aDocument = Element.InternalElement.Document;
            Autodesk.Revit.DB.TagOrientation aTagOrientation = Autodesk.Revit.DB.TagOrientation.Vertical;
            if (Horizontal)
                aTagOrientation = Autodesk.Revit.DB.TagOrientation.Horizontal;

            Autodesk.Revit.DB.IndependentTag aIndependentTag = aDocument.Create.NewTag(View.InternalElement as Autodesk.Revit.DB.View, Element.InternalElement, AddLeader, Autodesk.Revit.DB.TagMode.TM_ADDBY_CATEGORY, aTagOrientation, Point.ToRevitType(false));
            return Elements.ElementWrapper.ToDSType(aIndependentTag, true);
        }
    }
}
