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
    public static class RHS
    {
        public static double toMills = 1000;

        public static Revit.Elements.StructuralFraming CreateRHSElement(BHE.Bar BHoMBar, Revit.Elements.Level level, object Phase, string filtercomment)
        {
            BHP.SteelSection secprop = (BHP.SteelSection)BHoMBar.SectionProperty;

            string typeName = "RHS " + secprop.TotalDepth * toMills + "x" + secprop.TotalWidth * toMills + "x" + secprop.Tw * toMills + "x" + secprop.Tf1 * toMills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revRHS = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

        
            revRHS.SetParameterByName("z Justification", 1);

            revRHS.SetParameterByName("_Filter Comments 05", filtercomment);

            revRHS.SetParameterByName("Phase Created", Phase);
            

            if (BHoMBar.CustomData.ContainsKey("Z Offset"))
            {
                double zoffs = Convert.ToDouble(BHoMBar.CustomData["Z Offset"]);
                revRHS.SetParameterByName("z Offset Value", zoffs * 1000);
            }

            if (BHoMBar.CustomData.ContainsKey("X-section Rotation"))
            {
                double rot = Convert.ToDouble(BHoMBar.CustomData["X-section Rotation"]);
                revRHS.SetParameterByName("Cross-Section Rotation", rot);
            }
            
            
            return revRHS;


    }
        
    }
}
