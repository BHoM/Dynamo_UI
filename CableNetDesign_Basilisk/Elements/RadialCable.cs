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
    public static class RadialCable
    {
        public static double tomills = 1000;

        public static Revit.Elements.StructuralFraming CreateRadialCableElement(BHE.Bar RadialBar, Revit.Elements.Level level, /*Revit.Elements.FamilyType type,*/ object Phase, string filtercomment)
        {
            //Bar BHoMBar = BHoMRadial.RadialBar;
            
            BHP.CableSection secProp = (BHP.CableSection)RadialBar.SectionProperty;
            

            string typeName = secProp.NumberOfCables + "-RADIALCABLE LCØ" + secProp.TotalDepth * tomills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);


            DSG.Line mline = BHLine.ToDSLine(RadialBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revradial = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            //double diam = secProp.TotalDepth * 1000;
            //revradial.SetParameterByName("OD", diam);

            revradial.SetParameterByName("z Justification", 1);

            revradial.SetParameterByName("_Filter Comments 05", filtercomment);

            revradial.SetParameterByName("Phase Created", Phase);

            return revradial;
        }
    }
}
