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
        public static object VerticalSnapToHeight(object bhElement, List<double> refHeights, double tolerance)
        {
            return MLE.Snapping.VerticalPointSnap(bhElement, refHeights, tolerance);
        }

        /******************************************/

        public static object VerticalSnapToObjects(object bhElement, List<object> refBHElements, double tolerance)
        {
            return MLE.Snapping.VerticalPointSnap(bhElement, refBHElements, tolerance);
        }

        /******************************************/

        public static object HorizontalPointSnap(object bhElement, List<object> refBHElements, double tolerance)
        {
            return MLE.Snapping.HorizontalPointSnap(bhElement, refBHElements, tolerance);
        }

        /******************************************/

        public static object HorizontalParallelSnap(object bhElement, List<object> refBHElements, double tolerance)
        {
            return MLE.Snapping.HorizontalParallelSnap(bhElement, refBHElements, tolerance);
        }
    }
}
