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
    public static class RSS
    {
        public static double tomills = 1000;

        public static Revit.Elements.StructuralFraming CreateRSS(BHE.Bar BHoMBar, Revit.Elements.Level level,/* Revit.Elements.FamilyType type,*/ object Phase, string filtercomment)
        {
            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;

            string typeName = "RSS " + secProp.TotalDepth * tomills + "x" + secProp.TotalWidth * tomills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(tomills);

            Revit.Elements.StructuralFraming revRSS = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

          
            //double diam = secprop.TotalDepth*1000;
            //revRoofBrac.SetParameterByName("OD", diam);

            //double t = secprop.Tw * 1000;
            //revRoofBrac.SetParameterByName("t", t);
            

            revRSS.SetParameterByName("z Justification", 1);

            revRSS.SetParameterByName("Phase Created", Phase);

            revRSS.SetParameterByName("_Filter Comments 05", filtercomment);




            if (BHoMBar.CustomData.ContainsKey("Z Offset"))
            {
                double zoffs = Convert.ToDouble(BHoMBar.CustomData["Z Offset"]);
                revRSS.SetParameterByName("z Offset Value", zoffs * 1000);
            }

            if (BHoMBar.CustomData.ContainsKey("X-section Rotation"))
            {
                double rot = Convert.ToDouble(BHoMBar.CustomData["X-section Rotation"]);
                revRSS.SetParameterByName("Cross-Section Rotation", rot);
            }



            return revRSS;
        }

    }
}
