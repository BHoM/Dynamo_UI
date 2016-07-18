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
    public static class Snapping
    {
        public static object VerticalSnapToHeight(object element, List<double> refHeights, double tolerance)
        {
            return MLE.Snapping.VerticalPointSnap(element, refHeights, tolerance);
        }

        /******************************************/

        public static object VerticalSnapToObjects(object element, List<object> refElements, double tolerance)
        {
            return MLE.Snapping.VerticalPointSnap(element, refElements, tolerance);
        }

        /******************************************/

        public static object HorizontalPointSnap(object element, List<object> refElements, double tolerance)
        {
            return MLE.Snapping.HorizontalPointSnap(element, refElements, tolerance);
        }

        /******************************************/

        public static object HorizontalParallelSnap(object element, List<object> refElements, double tolerance)
        {
            return MLE.Snapping.HorizontalParallelSnap(element, refElements, tolerance);
        }
    }
}
