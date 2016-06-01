using System.Collections.Generic;
using System.Linq;

using Revit.Elements;
using Autodesk.DesignScript.Runtime;

namespace Revit
{
    /// <summary>
    /// MEP System
    /// </summary>
    public static class MEPSystem
    {
        /// <summary>
        /// Returns all elements of MEP System.
        /// </summary>
        /// <param name="System">MEP System</param>
        /// <returns name="Elements">MEP System Elements</returns>
        /// <search>
        /// MEPSystem, MEP System, Elements, mepsystem, mep system, elements
        /// </search>
        public static List<Elements.Element> Elements(Elements.Element System)
        {
            if (System != null && System.InternalElement != null)
            {
                Autodesk.Revit.DB.MEPSystem aMEPSystem = System.InternalElement as Autodesk.Revit.DB.MEPSystem;
                if (aMEPSystem != null)
                {
                    List<Autodesk.Revit.DB.Element> aElementList = aMEPSystem.Elements.Cast<Autodesk.Revit.DB.Element>().ToList();
                    if(aMEPSystem is Autodesk.Revit.DB.Mechanical.MechanicalSystem)
                    {
                        Autodesk.Revit.DB.Mechanical.MechanicalSystem aMechanicalSystem = aMEPSystem as Autodesk.Revit.DB.Mechanical.MechanicalSystem;
                        aElementList.AddRange(aMechanicalSystem.DuctNetwork.Cast<Autodesk.Revit.DB.Element>().ToList());
                    }
                    return aElementList.ConvertAll(x => ElementWrapper.ToDSType(x, true));
                }
                
            }

            return null;
        }

        /// <summary>
        /// Returns all elements of MEP System in order. Function search for longest/shortest branch from wchich list starts.
        /// </summary>
        /// <param name="System">MEP System</param>
        /// <param name="FirstElement">First Element from which numbering starts</param>
        /// <param name="ByLongest">Starts from longest branch</param>
        /// <returns name="Elements">MEP System Elements</returns>
        /// <returns name="Branches">Lists of boundary curves</returns>
        /// <search>
        /// MEPSystem, MEP System, Elements, Ordered Elements, ordered elements of mep system
        /// </search>
        [MultiReturn(new[] { "Elements", "Branches" })]
        public static Dictionary<string, object> OrderedElements(Elements.Element System, Elements.Element FirstElement, bool ByLongest = true)
        {

            Autodesk.Revit.DB.MEPSystem aMEPSystem = System.InternalElement as Autodesk.Revit.DB.MEPSystem;
            MechanicalElementList aMechanicalElementList = new MechanicalElementList(aMEPSystem);
            List<Autodesk.Revit.DB.Element> aElementList = null;
            List<List<Autodesk.Revit.DB.Element>> aBranches = null;
            aMechanicalElementList.OrderedElements(FirstElement.InternalElement, ByLongest, out aElementList, out aBranches);
            List<List<Elements.Element>> aResultBranches = new List<List<Revit.Elements.Element>>();
            foreach (List<Autodesk.Revit.DB.Element> aBranch in aBranches)
                aResultBranches.Add(aBranch.ConvertAll(x => x.ToDSType(true)));

            return new Dictionary<string, object>
                {
                    { "Elements", aElementList.ConvertAll(x => x.ToDSType(true))},
                    { "Branches", aResultBranches}
                };
        }

        /// <summary>
        /// Returns branches of MEP System 
        /// </summary>
        /// <param name="System">MEP System</param>
        /// <param name="FirstElement">First Element from which numbering starts</param>
        /// <returns name="Branches">MEP System Elements</returns>
        /// <search>
        /// MEPSystem, MEP System, Branches, branches of mep system
        /// </search>
        public static List<List<Elements.Element>> Branches(Elements.Element System, Elements.Element FirstElement)
        {
            Autodesk.Revit.DB.MEPSystem aMEPSystem = System.InternalElement as Autodesk.Revit.DB.MEPSystem;
            List<List<Elements.Element>> aResult = new List<List<Revit.Elements.Element>>();
            MechanicalElementList aMechanicalElementList = new MechanicalElementList(aMEPSystem);
            List<List<Autodesk.Revit.DB.Element>> aBranches = aMechanicalElementList.Branches(FirstElement.InternalElement);
            foreach (List<Autodesk.Revit.DB.Element> aBranch in aBranches)
                aResult.Add(aBranch.ConvertAll(x => x.ToDSType(true)));
            return aResult;
        }
    }
}
