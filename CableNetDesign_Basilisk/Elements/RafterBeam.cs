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
    public static class RafterBeam
    {
        public static double toMills = 1000;

        public static Revit.Elements.StructuralFraming CreateRafterElement(BHE.RafterBeam BHoMRafter, Revit.Elements.Level level, object Phase, string filtercomment)
        {
            BHE.Bar BHoMBar = BHoMRafter.RafterBar;

            BHP.SteelSection sectionProperty = (BHP.SteelSection)BHoMBar.SectionProperty;

            string typeName = "Rafter " + sectionProperty.TotalDepth * toMills + "x" + sectionProperty.TotalWidth * toMills + "x" + sectionProperty.Tw * toMills + "x" + sectionProperty.Tf1 * toMills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revbeam = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            double t1 = BHoMRafter.StartMiddleParam;
            revbeam.SetParameterByName("TIM1 pos", t1);
            
            double t2 = BHoMRafter.EndMiddleParam;
            revbeam.SetParameterByName("TIM2 pos", t2);

            revbeam.SetParameterByName("z Justification", 1);
            
            revbeam.SetParameterByName("_Filter Comments 05", filtercomment);

            revbeam.SetParameterByName("Phase Created", Phase);


            return revbeam;
        }

    }

  
}
