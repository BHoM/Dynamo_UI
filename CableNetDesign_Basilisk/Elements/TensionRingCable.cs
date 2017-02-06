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
    public static class TensionRingCable
    {
        public static double tomills = 1000;

        public static Revit.Elements.StructuralFraming CreateTRCableElement(BHE.Bar TRBar, Revit.Elements.Level level,/* Revit.Elements.FamilyType type,*/ object Phase, string filtercomment)
        {
            //Bar BHoMBar = BHoMTRCable.TRBar;

            BHP.CableSection secProp = (BHP.CableSection)TRBar.SectionProperty;

            string typeName = secProp.NumberOfCables + "-TRCABLE LCØ" + secProp.TotalDepth * tomills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);


            //double cablediam = secProp.TotalDepth*1000;

            //double noofcables = secProp.NumberOfCables;

            DSG.Line mline = BHLine.ToDSLine(TRBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revTRCable = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            //revTRCable.SetParameterByName("no of cables", noofcables);

            //revTRCable.SetParameterByName("cable diameter", cablediam);

            revTRCable.SetParameterByName("z Justification", 1);

            revTRCable.SetParameterByName("_Filter Comments 05", filtercomment);

            revTRCable.SetParameterByName("Phase Created", Phase);

            return revTRCable;
        }
    }
}
