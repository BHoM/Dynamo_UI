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
    public static class CreateRafter
    {
        public static Revit.Elements.StructuralFraming CreateRafterElement(BHE.RafterBeam BHoMRafter, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            BHE.Bar BHoMBar = BHoMRafter.RafterBar;

            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;

            
            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revbeam = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);



            double enddepth = BHoMRafter.EndDepth * 1000;
            revbeam.SetParameterByName("End depth", enddepth);

            double startdepth = BHoMRafter.StartDepth * 1000;
            revbeam.SetParameterByName("Start depth", startdepth);  

            double startmiddledepth = BHoMRafter.SrtartMiddleDepth * 1000;
            revbeam.SetParameterByName("TIM1 depth", startmiddledepth);

            double endmiddledepth = BHoMRafter.EndMiddleDepth * 1000;
            revbeam.SetParameterByName("TIM2 depth", endmiddledepth);

            double uniformdepth = secProp.TotalDepth * 1000;

            if (startdepth + enddepth + endmiddledepth + startmiddledepth == 0)
            {
                revbeam.SetParameterByName("End depth", uniformdepth);
                revbeam.SetParameterByName("TIM1 depth", uniformdepth);
                revbeam.SetParameterByName("Start depth", uniformdepth);
                revbeam.SetParameterByName("TIM2 depth", uniformdepth);
            }

            double width = secProp.TotalWidth * 1000;
            revbeam.SetParameterByName("Breadth", width);

            double tw = secProp.Tw * 1000;
            revbeam.SetParameterByName("tw", tw);

            double tf = secProp.Tf1 * 1000;
            revbeam.SetParameterByName("tf", tf);

            // double t1 = (double)BHoMBar.CustomData["0.0"];
            double t1 = BHoMRafter.StartMiddleParam;
            revbeam.SetParameterByName("t1", t1);

            // double t2 = (double)BHoMBar.CustomData["1.0"];
            double t2 = BHoMRafter.EndMiddleParam;
            revbeam.SetParameterByName("t2", t2);

            revbeam.SetParameterByName("z Justification", 1);
            
            revbeam.SetParameterByName("_Filter Comments 05", filtercomment);

            revbeam.SetParameterByName("Phase Created", Phase);


            return revbeam;
        }

    }
}
