using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

using ModularHousingToolkit;

using Dynamo.Services;

using BHoM;

namespace ProjectSpecific
{
    public static class ModularHousing
    {
        [MultiReturn(new[] { "Columns", "CornerColumns", "EdgeColumns", "Beams", "EdgeBeams", "Planks" })]
        public static Dictionary<string, object> CreateModularComponents(BHoM.Structural.Structure str, double offset, int noStoreys, double height)
        {
            List<ModularColumn> columns = new List<ModularColumn>();
            List<ModularColumn> cornerColumns = new List<ModularColumn>();
            List<ModularColumn> edgeColumns = new List<ModularColumn>();
            List<ModularBeam> beams = new List<ModularBeam>();
            List<ModularBeam> edgeBeams = new List<ModularBeam>();
            List<ModularPlank> planks = new List<ModularPlank>();

            foreach (BHoM.Structural.Node n in str.Nodes)
                n.SetCoordinateSystemAsDefault();

            //Create Columns 
            foreach (BHoM.Structural.Node n in str.Nodes)
            {
                bool found = false;
                if (n.Valence > 1)
                {

                    switch (n.Valence)
                    {
                        case 2:
                            ModularColumn cc = new ModularColumn(n, height);
                            cc.CalculateOrientationAngle_Corner();
                            cornerColumns.Add(cc);
                            break;
                        case 3:
                            ModularColumn ec = new ModularColumn(n, height);
                            ec.CalculateOrientationAngle_Edge();
                            edgeColumns.Add(ec);
                            break;
                        case 4:
                            columns.Add(new ModularColumn(n, height));
                            break;

                        default:
                            break;
                    }

                }
            }


            //Create Beams
            foreach (BHoM.Structural.Bar b in str.Bars)
            {
                bool found = false;

                if (b.StartNode.Valence > 1 && b.EndNode.Valence > 1) //zero because of hack above
                {
                    if (b.StartNode.Valence == 4 || b.EndNode.Valence == 4)
                        beams.Add(new ModularBeam(b, offset));
                    else
                        edgeBeams.Add(new ModularBeam(b, offset));
                }
            }


            //Create Planks
            foreach (BHoM.Structural.Face f in str.Faces)
            {
                ModularSlab slab = new ModularSlab(f, offset);
                foreach (ModularPlank plank in slab.Planks)
                    planks.Add(plank);
            }



            int bi = beams.Count;
            int ebi = edgeBeams.Count;
            int pi = planks.Count;
            int ci = columns.Count;
            int eci = edgeColumns.Count;
            int cci = cornerColumns.Count;


            for (int s = 2; s <= noStoreys; s++)
            {
                string sLevel = "Level " + s.ToString();
                double elevation = s * 2200;

                for (int i = 0; i < bi; i++)
                    beams.Add(new ModularBeam(beams[i], sLevel, elevation));
                for (int i = 0; i < ebi; i++)
                    edgeBeams.Add(new ModularBeam(edgeBeams[i], sLevel, elevation));
                for (int i = 0; i < pi; i++)
                    planks.Add(new ModularPlank(planks[i], sLevel, elevation));


                sLevel = "Level " + (s - 1).ToString();
                elevation = (s - 1) * 2200;

                for (int i = 0; i < ci; i++)
                    columns.Add(new ModularColumn(columns[i], sLevel, elevation));
                for (int i = 0; i < eci; i++)
                    edgeColumns.Add(new ModularColumn(edgeColumns[i], sLevel, elevation));
                for (int i = 0; i < cci; i++)
                    cornerColumns.Add(new ModularColumn(cornerColumns[i], sLevel, elevation));

            }

            return new Dictionary<string, object>
            {
                {"Columns", columns},
                {"CornerColumns", cornerColumns},
                {"EdgeColumns", edgeColumns},
                {"Beams", beams},
                {"EdgeBeams", edgeBeams},
                {"Planks", planks},
                                
            };
        }



        private static Line LineFromModularCoreWall(ModularCoreWall mc)
        {
            Autodesk.DesignScript.Geometry.Point sPt = Point.ByCoordinates(mc.StartPt.X, mc.StartPt.Y, mc.StartPt.Z);
            Autodesk.DesignScript.Geometry.Point ePt = Point.ByCoordinates(mc.EndPt.X, mc.EndPt.Y, mc.EndPt.Z);

            return Line.ByStartPointEndPoint(sPt, ePt);

        }
 
        private static Line LineFromModularColumn(ModularColumn mc)
        {
            Autodesk.DesignScript.Geometry.Point sPt = Point.ByCoordinates(mc.StartPt.X, mc.StartPt.Y, mc.StartPt.Z);
            Autodesk.DesignScript.Geometry.Point ePt = Point.ByCoordinates(mc.EndPt.X, mc.EndPt.Y, mc.EndPt.Z);

            return Line.ByStartPointEndPoint(sPt, ePt);

        }

        private static Line LineFromModularBeam(ModularBeam mb)
        {
            Autodesk.DesignScript.Geometry.Point sPt = Point.ByCoordinates(mb.StartPt.X,mb.StartPt.Y,mb.StartPt.Z);
            Autodesk.DesignScript.Geometry.Point ePt = Point.ByCoordinates(mb.EndPt.X,mb.EndPt.Y,mb.EndPt.Z);

            return Line.ByStartPointEndPoint(sPt, ePt);
            
        }

        private static Line LineFromModularPlank(ModularPlank mp)
        {
            Autodesk.DesignScript.Geometry.Point sPt = Point.ByCoordinates(mp.StartPt.X, mp.StartPt.Y, mp.StartPt.Z);
            Autodesk.DesignScript.Geometry.Point ePt = Point.ByCoordinates(mp.EndPt.X, mp.EndPt.Y, mp.EndPt.Z);

            return Line.ByStartPointEndPoint(sPt, ePt);

        }



        [MultiReturn(new[] { "CL" })]
        public static Dictionary<string, object> DeconstructModularCoreWall(ModularCoreWall mc)
        {
            Line cl = LineFromModularCoreWall(mc);
            return new Dictionary<string, object>
            {
                {"CL", cl},
            };

        }


        [MultiReturn(new[] { "CL", "Width", "LeftWidth", "RightWidth", "OrientationAngle", "Level"})]
        public static Dictionary<string, object> DeconstructModularColumn(ModularColumn mc)
        {
            Line cl = LineFromModularColumn(mc);
            return new Dictionary<string, object>
            {
                {"CL", cl},
                {"Width", mc.Width},
                {"LeftWidth", mc.LeftWidth},
                {"RightWidth", mc.RightWidth},
                {"OrientationAngle", mc.OrientationAngle},
                {"Level", mc.Level},
            };

        }



        [MultiReturn(new[] { "CL", "StartAngle", "EndAngle", "StartOffset", "EndOffset", "Width", "Depth", "CorbelWidth", "CorbelDepth", "Level" })]
        public static Dictionary<string, object> DeconstructModularBeam(ModularBeam mb)
        {
            Line cl = LineFromModularBeam(mb);
            return new Dictionary<string, object>
            {
                {"CL", cl},
                {"StartAngle", mb.StartAngle},
                {"EndAngle", mb.EndAngle},
                {"StartOffset", mb.StartOffset},
                {"EndOffset", mb.EndOffset},
                {"Width", mb.Width},
                {"Depth", mb.Depth},
                {"CorbelWidth", mb.CorbelWidth},
                {"CorbelDepth", mb.CorbelThickness},
                {"Level", mb.Level},
            };

        }


        [MultiReturn(new[] { "CL", "StartAngle", "EndAngle", "Width", "Depth", "CorbelWidth", "CorbelDepth", "LeftCorbel", "RightCorbel", "Level" })]
        public static Dictionary<string, object> DeconstructModularPlank(ModularPlank mp)
        {
            Line cl = LineFromModularPlank(mp);
            return new Dictionary<string, object>
            {
                {"CL", cl},
                {"StartAngle", mp.StartAngle},
                {"EndAngle", mp.EndAngle},
                {"Width", mp.Width},
                {"Depth", mp.Depth},
                {"CorbelWidth", mp.CorbelWidth},
                {"CorbelDepth", mp.CorbelThickness},
                {"LeftCorbel", mp.LeftCorbel},
                {"RightCorbel", mp.RightCorbel},
                {"Level", mp.Level},
            };

        }


        
 
    }
}
