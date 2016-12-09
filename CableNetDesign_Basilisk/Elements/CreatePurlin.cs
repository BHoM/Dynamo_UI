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
    public static class CreatePurlin
    {
        public static Revit.Elements.StructuralFraming CreatePurlinElement(BHE.Bar BHoMBar, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            BHP.SteelSection secprop = (BHP.SteelSection)BHoMBar.SectionProperty;

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revRHS = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            double depth = secprop.TotalDepth*1000;
            revRHS.SetParameterByName("Start depth", depth);

            double width = secprop.TotalWidth*1000;
            revRHS.SetParameterByName("Breadth", width);

            double tf = secprop.Tf1*1000;
            revRHS.SetParameterByName("tf", tf);

            double tw = secprop.Tw*1000;
            revRHS.SetParameterByName("tw", tw);

            revRHS.SetParameterByName("z Justification", 1);
            
            double zoffs = Convert.ToDouble(BHoMBar.CustomData["Z Offset"]);

            double rot = Convert.ToDouble(BHoMBar.CustomData["X-section Rotation"]);
           
            revRHS.SetParameterByName("Cross-Section Rotation", rot);
           

            revRHS.SetParameterByName("z Offset Value", zoffs*1000);

            revRHS.SetParameterByName("_Filter Comments 05", filtercomment);

            revRHS.SetParameterByName("Phase Created", Phase);

            return revRHS;


    }
        
    }
}
