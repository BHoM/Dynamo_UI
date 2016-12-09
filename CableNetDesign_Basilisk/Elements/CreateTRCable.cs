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
    public static class CreateTRCable
    {
        public static Revit.Elements.StructuralFraming CreateTRCableElement(TensionRing BHoMTRCable, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            Bar BHoMBar = BHoMTRCable.TRBar;

            BHP.CableSection secprop = (BHP.CableSection)BHoMBar.SectionProperty;

            double cablediam = secprop.TotalDepth*1000;

            double noofcables = secprop.NumberOfCables;

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);

            Revit.Elements.StructuralFraming revTRCable = Revit.Elements.StructuralFraming.BeamByCurve(mmline, level, type);

            revTRCable.SetParameterByName("no of cables", noofcables);

            revTRCable.SetParameterByName("cable diameter", cablediam);

            revTRCable.SetParameterByName("z Justification", 1);

            revTRCable.SetParameterByName("_Filter Comments 05", filtercomment);

            revTRCable.SetParameterByName("Phase Created", Phase);

            return revTRCable;
        }
    }
}
