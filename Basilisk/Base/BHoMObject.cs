using Autodesk.DesignScript.Runtime;
using BH.Adapter;
using BH.Engine.Base;
using BH.Engine.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHB = BH.oM.Base;
using BHG = BH.oM.Geometry;
using ADG = Autodesk.DesignScript.Geometry;
using DA = BH.Adapter.DesignScript;
using ADR = Autodesk.DesignScript.Runtime;

namespace Basilisk.Base
{
    public static class BHoMObject
    {
        public static object BHoMGeometry(ADG.Geometry geometry)
        {
            return DA.Convert.IToBHoM(geometry);
        }

        public static BHB.CustomObject CreateCustomObject(string Name, List<string> Tags, params Dictionary<string, object>[] customData)
        {
            
            BHB.CustomObject customObj = new BHB.CustomObject();
            customObj.Name = Name;
            customObj.Tags = new HashSet<string>(Tags);

            foreach (object o in customData)
                customObj.CustomData.Add("A", o);

            return customObj;
        }
    }
}
