using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoM.Geometry;
using ModelLaundry_Engine;
using Autodesk.DesignScript.Runtime;

namespace ModelLaundry
{
    public static class LaundryUtils
    {
        public static object HorizontalExtend(object bhElement, double dist)
        {
            return Util.HorizontalExtend(bhElement, dist);
        }

        /*************************************/

        [MultiReturn(new[] { "insiders", "outsiders" })]
        public static Dictionary<string, object> FilterByBoundingBox(List<object> bhElement, List<BoundingBox> bhBoxes)
        {
            List<object> outsiders = new List<object>();
            List<object> insiders = Util.FilterByBoundingBox(bhElement, bhBoxes, out outsiders);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["insiders"] = insiders;
            result["outsiders"] = outsiders;
            return result;
        }

        /*************************************/

        [MultiReturn(new[] { "result", "removed" })]
        public static Dictionary<string, object> RemoveSmallContours(object bhElement, double maxLength)
        {
            Group<Curve> removed = new Group<Curve>();
            object remaining = Util.RemoveSmallContours(bhElement, maxLength, out removed);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["result"] = remaining;
            result["removed"] = removed;
            return result;
        }
    }
}
