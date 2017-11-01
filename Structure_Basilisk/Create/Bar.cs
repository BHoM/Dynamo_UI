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
        public static BHSE.Bar CreateBarFromNodes(BHSE.Node NodeA, BHSE.Node NodeB
            //[ADR.DefaultArgument("null")]SectionProperty SectionProperty,
            //[ADR.DefaultArgument("null")]BHM.Material Material,
            //[ADR.DefaultArgument("0")]double OrientationAngle,
            //[ADR.DefaultArgument("")]string Name
            //[ADR.DefaultArgument("null")]Dictionary<string, object> customData)
            )
        {
            BHSE.Bar bar = new BHSE.Bar();
            bar = BHES.Create.IBar(NodeA, NodeB);
            BH.oM.Structural.Properties.SectionProperty sec3 = new BH.oM.Structural.Properties.ExplicitSectionProperty();
            sec3.Material = new BH.oM.Materials.Material("Material2", BH.oM.Materials.MaterialType.Concrete, 10, 10, 10, 10, 10);
            sec3.Name = "Section 3";
            bar.SectionProperty = sec3;
            return bar;
        }


        public static BHSE.Bar CreateBarFromLine(ADG.Line line
            //[ADR.DefaultArgument("null")]SectionProperty SectionProperty,
            //[ADR.DefaultArgument("null")]BHM.Material Material,
            //[ADR.DefaultArgument("0")]double OrientationAngle,
            //[ADR.DefaultArgument("")]string Name
            //[ADR.DefaultArgument("null")]Dictionary<string, object> customData)
            )
        {
            BHSE.Bar bar = new BHSE.Bar();
            bar = BHES.Create.IBar((ICurve)AD.Convert.IToBHoM(line));
            return bar;
        }

        public static BHSE.Bar CreateBarFromPoints(ADG.Point pointA, ADG.Point pointB
           //[ADR.DefaultArgument("null")]SectionProperty SectionProperty,
           //[ADR.DefaultArgument("null")]BHM.Material Material,
           //[ADR.DefaultArgument("0")]double OrientationAngle,
           //[ADR.DefaultArgument("")]string Name
           //[ADR.DefaultArgument("null")]Dictionary<string, object> customData)
           )
        {
            BHSE.Bar bar = new BHSE.Bar();
            bar = BHES.Create.IBar(AD.Convert.IToBHoM(pointA), AD.Convert.IToBHoM(pointB));
            return bar;
        }
    }
}