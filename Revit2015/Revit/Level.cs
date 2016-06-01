using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;

using RevitServices.Persistence;
using Revit.Elements;

namespace Revit
{
    /// <summary>
    /// A level
    /// </summary>
    public static class Level
    {
        /// <summary>
        /// Returns the closest level to point
        /// </summary>
        /// <param name="Point">Point</param>
        /// <returns name="Level">Level</returns>
        /// <search>
        /// Get Closest level, get closest level
        /// </search>
        public static Revit.Elements.Level GetClosest(Autodesk.DesignScript.Geometry.Point Point)
        {
            if (Point != null)
            {
                Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
                if (aDocument != null)
                {
                    FilteredElementCollector aFilteredElementCollector = new FilteredElementCollector(aDocument).OfClass(typeof(Autodesk.Revit.DB.Level));
                    List<Autodesk.Revit.DB.Level> aLevelList = aFilteredElementCollector.ToElements().ToList().ConvertAll(x => x as Autodesk.Revit.DB.Level);
                    aLevelList = aLevelList.OrderBy(x => x.ProjectElevation).ToList();

                    for (int i = 1; i < aLevelList.Count; i++)
                        if (aLevelList[i].ProjectElevation > Point.Z)
                            return aLevelList[i - 1].ToDSType(true) as Revit.Elements.Level;

                    if (aLevelList.Count > 0)
                    {
                        if (aLevelList.First().ProjectElevation > Point.Z)
                            return aLevelList.First().ToDSType(true) as Revit.Elements.Level;
                        else if (aLevelList.Last().ProjectElevation < Point.Z)
                            return aLevelList.Last().ToDSType(true) as Revit.Elements.Level;
                    }
                }
            }
            return null;
        }
    }
}
