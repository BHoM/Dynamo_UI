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
    public static class Column
    {
        public static double tomills = 1000;

        public static Revit.Elements.StructuralFraming CreateColumnElement(Bar BHoMBar, Revit.Elements.Level level,/* Revit.Elements.FamilyType type,*/ object Phase, string filtercomment)
        {
            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;

            string typeName = "CHS " + secProp.TotalDepth * tomills + "x" + secProp.Tw * tomills;

            Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typeName);

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(tomills);


            if (mmline.EndPoint.Z < mmline.StartPoint.Z)
            {
                mmline = DSG.Line.ByStartPointEndPoint(mmline.EndPoint, mmline.StartPoint);
            }
          
            Revit.Elements.StructuralFraming revcolumn = Revit.Elements.StructuralFraming.ColumnByCurve(mmline, level, type);
            
            revcolumn.SetParameterByName("Phase Created", Phase);

            revcolumn.SetParameterByName("_Filter Comments 05", filtercomment);

            return revcolumn;
        }
    }
}
