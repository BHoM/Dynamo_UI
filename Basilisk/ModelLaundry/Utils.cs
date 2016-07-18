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
    public static class Utils
    {
        public static object HorizontalExtend(object element, double dist)
        {
            return Util.HorizontalExtend(element, dist);
        }

        /*************************************/

        [MultiReturn(new[] { "insiders", "outsiders" })]
        public static Dictionary<string, object> FilterByBoundingBox(List<object> elements, List<BoundingBox> boxes)
        {
            List<object> outsiders = new List<object>();
            List<object> insiders = Util.FilterByBoundingBox(elements, boxes, out outsiders);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["insiders"] = insiders;
            result["outsiders"] = outsiders;
            return result;
        }

        /*************************************/

        [MultiReturn(new[] { "result", "removed" })]
        public static Dictionary<string, object> RemoveSmallContours(object element, double maxLength)
        {
            Group<Curve> removed = new Group<Curve>();
            object remaining = Util.RemoveSmallContours(element, maxLength, out removed);

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["result"] = remaining;
            result["removed"] = removed;
            return result;
        }
    }
}
