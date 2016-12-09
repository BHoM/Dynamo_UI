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
    public static class CreateProp
    {

        public static Revit.Elements.StructuralFraming CreatePropElement(Bar BHoMBar, Revit.Elements.Level level, object Phase, string filtercomment)
        {
          
            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;

            
            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);
            
            string typeStr = (string)BHoMBar.CustomData["family type"];
            

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeStr);

            Revit.Elements.StructuralFraming revprop = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);
            
            double od = secProp.TotalDepth*1000;
            revprop.SetParameterByName("OD", od);

            double t = secProp.Tw*1000;
            revprop.SetParameterByName("t", t);

            revprop.SetParameterByName("z Justification", 1);

            revprop.SetParameterByName("_Filter Comments 05", filtercomment);
            
            revprop.SetParameterByName("Phase Created", Phase);
            
            return revprop;
        }
    }
}
