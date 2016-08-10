using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;

using RevitServices.Transactions;

namespace Revit
{
    /// <summary>
    /// A Space Tag
    /// </summary>
    public static class SpaceTag
    {
        /// <summary>
        /// Set family symbol of space tag
        /// </summary>
        /// <param name="Element">Element</param>
        /// <param name="FamilyType">Family Type</param>
        /// <returns name="SetFamilySymbol">Family Symbol</returns>
        /// <search>
        /// Space Tag, Set Family Symbol, space tag, set family symbol
        /// </search>
        public static Revit.Elements.Element SetFamilySymbol(Revit.Elements.Element Element, Revit.Elements.FamilyType FamilyType)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.SpaceTag)
                if (FamilyType != null && FamilyType.InternalElement != null)
                {
                    Autodesk.Revit.DB.Mechanical.SpaceTag aSpaceTag = Element.InternalElement as Autodesk.Revit.DB.Mechanical.SpaceTag;
                    TransactionManager.Instance.EnsureInTransaction(Element.InternalElement.Document);
                    aSpaceTag.ChangeTypeId(FamilyType.InternalElement.Id);
                    TransactionManager.Instance.TransactionTaskDone();
                    return aSpaceTag.SpaceTagType.ToDSType(true);
                }
            return null;
        }

        /// <summary>
        /// Space of space tag
        /// </summary>
        /// <param name="Element">Space tag</param>
        /// <returns name="Space">Associated space</returns>
        /// <search>
        /// Space Tag, Associated space, space tag, assoicated space
        /// </search>
        public static Revit.Elements.Element Space(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.SpaceTag)
            {
                Autodesk.Revit.DB.Mechanical.SpaceTag aSpaceTag = Element.InternalElement as Autodesk.Revit.DB.Mechanical.SpaceTag;
                if (aSpaceTag != null)
                    return aSpaceTag.Space.ToDSType(true);
            }

            return null;
        }

        /// <summary>
        /// View of space tag
        /// </summary>
        /// <param name="Element">Space tag</param>
        /// <returns name="Space">Associated view</returns>
        /// <search>
        /// Space Tag, Associated view, space tag, assoicated view
        /// </search>
        public static Revit.Elements.Element View(Revit.Elements.Element Element)
        {
            if (Element != null && Element.InternalElement != null && Element.InternalElement is Autodesk.Revit.DB.Mechanical.SpaceTag)
            {
                Autodesk.Revit.DB.Mechanical.SpaceTag aSpaceTag = Element.InternalElement as Autodesk.Revit.DB.Mechanical.SpaceTag;
                if (aSpaceTag != null)
                    return aSpaceTag.View.ToDSType(true);
            }

            return null;
        }
    }
}
