using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Geometry;
using MLE = ModelLaundry_Engine;
using Autodesk.DesignScript.Runtime;

namespace ModelLaundry
{
    public static class BHSnapping
    {
        public static object VerticalSnapToHeight(object bhElement, List<double> refHeights, double tolerance = 0.2)
        {
            return MLE.Snapping.VerticalSnapToHeight(bhElement, refHeights, tolerance);
        }

        /******************************************/

        public static object VerticalSnapToShape(object bhElement, List<object> refBHElements, double tolerance = 0.2)
        {
            return MLE.Snapping.VerticalSnapToShape(bhElement, refBHElements, tolerance);
        }

        /******************************************/

        public static object HorizontalSnapToShape(object bhElement, List<object> refBHElements, double tolerance = 0.2, bool anyHeight = false)
        {
            return MLE.Snapping.HorizontalSnapToShape(bhElement, refBHElements, tolerance, anyHeight);
        }

        /******************************************/

        public static object HorizontalParallelSnap(object bhElement, List<object> refBHElements, double tolerance = 0.2, bool anyHeight = false, double angleTol = 2)
        {
            return MLE.Snapping.HorizontalParallelSnap(bhElement, refBHElements, tolerance, anyHeight, angleTol*Math.PI/180);
        }

        /******************************************/

        public static object PointToPointSnap(object bhElement, List<object> refBHElements, double tolerance = 0.2)
        {
            return MLE.Snapping.PointToPointSnap(bhElement, refBHElements, tolerance);
        }
    }
}
