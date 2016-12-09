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
    public static class CreateColumn
    {
        public static Revit.Elements.StructuralFraming CreateColumnElement(Bar BHoMBar, Revit.Elements.Level level, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            BHP.SteelSection secProp = (BHP.SteelSection)BHoMBar.SectionProperty;

            DSG.Line mline = BHLine.ToDSLine(BHoMBar.Line);
            DSG.Line mmline = (DSG.Line)mline.Scale(1000);


            if (mmline.EndPoint.Z < mmline.StartPoint.Z)
            {
                mmline = DSG.Line.ByStartPointEndPoint(mmline.EndPoint, mmline.StartPoint);
            }
            //string typestr = (string)BHoMBar.CustomData["family type"];

            //Revit.Elements.FamilyType type = Revit.Elements.FamilyType.ByName(typestr);
            
            Revit.Elements.StructuralFraming revcolumn = Revit.Elements.StructuralFraming.ColumnByCurve(mmline, level, type);

            double od = secProp.TotalDepth*1000;
            revcolumn.SetParameterByName("OD", od);

            double t = secProp.Tw*1000;
            revcolumn.SetParameterByName("t", t);

            revcolumn.SetParameterByName("Phase Created", Phase);

            revcolumn.SetParameterByName("_Filter Comments 05", filtercomment);

            return revcolumn;
        }
    }
}
