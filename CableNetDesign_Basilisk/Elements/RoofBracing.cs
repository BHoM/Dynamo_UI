using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;
using BHE = BHoM.Structural.Elements;
using BHP = BHoM.Structural.Properties;
using BHoM.Structural.Elements;
using Geometry;


namespace Elements
{
    public static class RoofBracing
    {
        public static Revit.Elements.StructuralFraming CreateRoofBracing(BHE.Bar BHoMBar, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            BHP.SteelSection secprop = (BHP.SteelSection)BHoMBar.SectionProperty;
            
            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revRoofBrac = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            double diam = secprop.TotalDepth*1000;
            revRoofBrac.SetParameterByName("OD", diam);

            double t = secprop.Tw * 1000;
            revRoofBrac.SetParameterByName("t", t);
            
            revRoofBrac.SetParameterByName("z Justification", 1);

            revRoofBrac.SetParameterByName("Phase Created", Phase);

            revRoofBrac.SetParameterByName("_Filter Comments 05", filtercomment);
            
            
            return revRoofBrac;
        }

    }
}
