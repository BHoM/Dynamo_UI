using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BH.oM.Geometry;
using BHSE = BH.oM.Structural.Elements;
using BH.oM.Structural.Properties;
using ADG = Autodesk.DesignScript.Geometry;
using AD = BH.Adapter.DesignScript;
using BHES = BH.Engine.Structure;
using ADR = Autodesk.DesignScript.Runtime;
using Dynamo.Graph.Nodes;
using BHM = BH.oM.Materials;
using BH.oM.Base;

namespace Basilisk.Structure
{
    public static partial class Create
    {

        public static BarRelease CreateBarRelease(NodeConstraint startConstraint, NodeConstraint endConstraint)
        {
            BarRelease release = new BarRelease();
            release.StartConstraint = startConstraint;
            release.EndConstraint = endConstraint;

            return release;
        }
    }
}
