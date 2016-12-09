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
    public static class CreateTRNodes
    {
        public static Revit.Elements.FamilyInstance CreateTRNodeElement(TensionRingNode BHoMTRNode, Revit.Elements.FamilyType type, object Phase, string filtercomment)
        {
            Node BHoMNode = BHoMTRNode.CentreNode;

            DSG.Point mpoint = BHPoint.ToDSPoint(BHoMNode.Point);
            DSG.Point mmpoint = (DSG.Point)mpoint.Scale(1000);
            Revit.Elements.FamilyInstance revTRNode = Revit.Elements.FamilyInstance.ByPoint(type, mmpoint);

            double rotangle = BHoMTRNode.HZ_rotation_angle;

           revTRNode = revTRNode.SetRotation(rotangle + 180);

            revTRNode.SetParameterByName("_Filter Comments 05", filtercomment);

            revTRNode.SetParameterByName("Phase Created", Phase);

            return revTRNode;
        }

    }
}
