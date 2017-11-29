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

namespace Basilisk.Base
{
    public static class DesignScriptObject
    {
        public static ADG.Geometry DesignScriptGeometry(object bhElement)
        {
            return DA.Convert.IToDesignScript(bhElement as dynamic);
        }   
    }
}