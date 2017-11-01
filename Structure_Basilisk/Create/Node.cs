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

        public static BHSE.Node CreateNode(ADG.Point point, [ADR.DefaultArgument("null")]NodeConstraint constraint)
        {
            BHSE.Node node = new BHSE.Node();
            if (constraint == null)
                constraint = new NodeConstraint();
            node.Point = AD.Convert.IToBHoM(point as dynamic);
            node.Constraint = constraint;
            return node;
        }
    }
}