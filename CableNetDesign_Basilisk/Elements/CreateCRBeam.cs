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
    public static class CreateCRBeam
    {
        public static Revit.Elements.StructuralFraming CreateCRBeamElement(CompressionRingBeam BHoMCReam, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {

            BHP.SteelSection secProp = (BHP.SteelSection)BHoMCReam.CRBar.SectionProperty;
            
            DSG.Line mline = BHLine.ToDSLine(BHoMCReam.CRBar.Line);
            DSG.Line mmline = (DSG.Line) mline.Scale(1000);
            
            Revit.Elements.StructuralFraming revbeam = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            double depth = secProp.TotalDepth * 1000;
            revbeam.SetParameterByName("Ht", depth);

            double width = secProp.TotalWidth * 1000;
            revbeam.SetParameterByName("b", width);

            double tw = secProp.Tw * 1000;
            revbeam.SetParameterByName("tw", tw);

            double tf = secProp.Tf1 * 1000;
            revbeam.SetParameterByName("tf", tf);

            double endangle = BHoMCReam.EndAngle;
            revbeam.SetParameterByName("End_Angle", endangle);

            double startangle = BHoMCReam.StartAngle;
            revbeam.SetParameterByName("Start_angle", startangle);

            revbeam.SetParameterByName("z Justification", 1);

            revbeam.SetParameterByName("_Filter Comments 05", filtercomment);


            revbeam.SetParameterByName("Phase Created", Phase);
            

            return revbeam;
        }
  
    }
}
