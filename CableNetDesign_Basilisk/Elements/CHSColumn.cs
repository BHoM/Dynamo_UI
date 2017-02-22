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
    public static class CHS
    {
        public static double toMills = 1000;

        public static Revit.Elements.StructuralFraming CreateCHSElement(Bar BHoMBar, Revit.Elements.Level level, object Phase, string filtercomment)
        {
          
            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;
            
            string typeName = "CHS " + secProp.TotalDepth * toMills + "x" + secProp.Tw * toMills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);


            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);
            
         
            Revit.Elements.StructuralFraming revprop = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);
            
           
            revprop.SetParameterByName("z Justification", 1);

            revprop.SetParameterByName("_Filter Comments 05", filtercomment);
            
            revprop.SetParameterByName("Phase Created", Phase);
            
            return revprop;
        }
    }
}
