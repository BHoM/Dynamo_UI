using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Revit.Elements;
using RevitServices.Transactions;


using ExcelUtilities;
using BHExcelFormat;
using BHExcelFormat.Tabs.Geometry;
using BHExcelFormat.Tabs.Loading;
using BHExcelFormat.Tabs.Results;
using IO;
using StructuralComponents;
using StructuralComponents.Results;
using StructuralComponents.SectionProperties;

using RevitServices.Persistence;
using Revit.GeometryConversion;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;


namespace Basilisk.Structural
{
    public static class ContainerNodes
    {
        public static Curve Curve(Curve Crv)
        {
            return Crv;
        }

        public static Point Point(Point Pt)
        {
            return Pt;
        }

        public static Geometry Geometry(Geometry Geo)
        {
            return Geo;
        }

        public static Vector Vector(Vector Vec)
        {
            return Vec;
        }
    }
}

