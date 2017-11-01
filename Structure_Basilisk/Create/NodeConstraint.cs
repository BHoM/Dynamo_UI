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

        public static NodeConstraint CreateNodeConstraint(bool Ux = true, bool Uy = true, bool Uz = true, bool Rx = true, bool Ry = true, bool Rz = true)
        {
            NodeConstraint nodeConsr = new NodeConstraint();
            nodeConsr.UX = (DOFType)(Ux ? 1 : 0);
            nodeConsr.UY = (DOFType)(Uy ? 1 : 0);
            nodeConsr.UZ = (DOFType)(Uz ? 1 : 0);
            nodeConsr.RX = (DOFType)(Rx ? 1 : 0);
            nodeConsr.RY = (DOFType)(Ry ? 1 : 0);
            nodeConsr.RZ = (DOFType)(Rz ? 1 : 0);
            return nodeConsr;
        }
    }
}