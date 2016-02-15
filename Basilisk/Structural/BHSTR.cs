using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using RMA.OpenNURBS;
using System.Drawing;
using System.Threading.Tasks;
using ExcelUtilities;
using BHExcelFormat;
using BHExcelFormat.Tabs.Geometry;
using BHExcelFormat.Tabs.Loading;
using BHExcelFormat.Tabs.Results;
using IO;
using StructuralComponents;
using StructuralComponents.Results;
using StructuralComponents.SectionProperties;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;

using Revit.Elements;
using RevitServices.Transactions;


namespace Basilisk.Structural
{
    public static class BHSTR
    {
        public static Structure ExcelIn(string path, bool execute) //out List<SectionProperty> sps, out List<Beam> beams)
        {
            Structure str = new Structure();
            List<StructuralComponents.Node> sc_nodes = new List<StructuralComponents.Node>();
            List<Beam> sc_beams = new List<Beam>();
            List<SectionProperty> sc_sps = new List<SectionProperty>();
            List<StructuralList> sc_lists = new List<StructuralList>();
            List<GridLine> sc_grids = new List<GridLine>();


            if (execute)
            {
                SSoT ssot;
                if (path != "ACTIVE")
                    ssot = new SSoT(path, false);
                else
                {
                    GeneralUtilities.Utilities.KillAllUnusedExcelInstances();
                    ssot = new SSoT();
                }

                sc_nodes = ssot.GetNodes();
                sc_beams = ssot.GetBeams(sc_nodes);
                sc_sps = ssot.GetSectionProperties(true);
                sc_lists = ssot.GetLists();
                
                ssot.GetAndPushSectionSetOut(sc_sps, true);

                str.Nodes = sc_nodes;
                str.Beams = sc_beams;
                str.SectionProperties = sc_sps;

                foreach (StructuralList lst in sc_lists)
                str.AddList(lst);

                sc_beams.SetSectionPropertyReferences(sc_sps);
            }
            return str;
        }

        [MultiReturn(new[] { "Nodes", "Beams", "SectionProperties" })]
        public static Dictionary<string, object> DeconstructStructure(Structure str)
        {
            return new Dictionary<string, object>
            {
                {"Nodes", str.Nodes},
                {"Beams", str.Beams},
                {"SectionProperties", str.SectionProperties},
            };

        }

        [MultiReturn(new[] { "Beams", "NameOfList", "Ids"})]
        public static Dictionary<string, object> GetListFromStructure(Structure str, List<int> indexes)
        {
            List<Beam> beams = new List<Beam>();
            List<string> listNames = new List<string>();
            List<List<int>> idList = new List<List<int>>();

            foreach (int index in indexes)
            {
                StructuralList list;         
                int listIndex = str.GetLists().FindIndex(x => x.Id == index);

                if (listIndex != -1)
                    list = str.GetLists()[listIndex];
                else
                    break;

                List<int> ids = list.GenerateIndicies();
                foreach (int i in ids)
                {
                    beams.Add(str.GetElement(i));
                }

            
                listNames.Add(list.Name);
                idList.Add(ids);

            }

            return new Dictionary<string, object>
            {
                {"Beams", beams},
                {"NameOfList", listNames},
                {"Ids", idList},
            };

        }

        [MultiReturn(new[] { "Origin", "Index", "Name" })]
        public static Dictionary<string, object> DeconstructNode(List<StructuralComponents.Node> nodes)
        {
            List<Point> nodeCenterPts = new List<Point>();
            List<string> nodeNames = new List<string>();
            List<int> indicies = new List<int>();

            for (int i = 0; i < nodes.Count; i++)
            {
                Point tempPt;
                tempPt = Point.ByCoordinates(nodes[i].X * 1000, nodes[i].Y * 1000, nodes[i].Z * 1000);
                nodeCenterPts.Add(tempPt);
                nodeNames.Add(nodes[i].Name);
                indicies.Add(nodes[i].Index);
            }


            return new Dictionary<string, object>
            {
                {"Origin", nodeCenterPts},
                {"Index", indicies},
                {"Name", nodeNames}
            };
        }

        [MultiReturn(new[] { "StartNodePoint", "EndNodePoint", "Index", "PropInex", "SectionProperties", "GA" , "Dummy", "ElemType", "Name", "Group", "PropName", "FamilySymbolName", "Tapered prop"})]
        public static Dictionary<string, object> DeconstructBeam(List<StructuralComponents.Beam> beam)
        {
            List<Point> startPt = new List<Point>();
            List<Point> endPt = new List<Point>();
            List<int> indicies = new List<int>();
            List<int> propIndex = new List<int>();
            List<SectionProperty> secProp = new List<SectionProperty>();
            List<double> gammaAngles = new List<double>();
            List<double> mass = new List<double>();
            List<bool> dummyList = new List<bool>();
            List<string> elemType = new List<string>();
            List<string> name = new List<string>();
            List<int> beamGroup = new List<int>();
            List<string> propName = new List<string>();
            List<string> familySymName = new List<string>();
            List<bool> isTap = new List<bool>();

            for (int i = 0; i < beam.Count; i++)
            {
                Beam tempBeam = beam[i];
                SectionProperty tempSecProp = tempBeam.SectionProperty;
                Point sPt;
                Point ePt;

                sPt = Point.ByCoordinates(tempBeam.StartNode.X * 1000, tempBeam.StartNode.Y * 1000, tempBeam.StartNode.Z * 1000);
                ePt = Point.ByCoordinates(tempBeam.EndNode.X * 1000, tempBeam.EndNode.Y * 1000, tempBeam.EndNode.Z * 1000);

                startPt.Add(sPt);
                endPt.Add(ePt);
                indicies.Add(tempBeam.Index);
                propIndex.Add(tempBeam.SectionPropertyIndex);
                secProp.Add(tempBeam.SectionProperty);
                gammaAngles.Add(tempBeam.OrientationAngle);
                dummyList.Add(tempBeam.Dummy);
                elemType.Add(tempBeam.ElementType.ToString());
                name.Add(tempBeam.Name);
                beamGroup.Add(tempBeam.Group);
                propName.Add(tempBeam.SectionProperty.Name);
                mass.Add(tempSecProp.MassPerMetre);
                isTap.Add(tempSecProp.IsTapered);

                if (tempSecProp.Type == "UC" || tempSecProp.Type == "UB")
                {
                    familySymName.Add(tempSecProp.Type + " " + (tempSecProp.D * 1000).ToString() + "x" + (tempSecProp.B * 1000).ToString() + "x" + (tempSecProp.tw * 1000).ToString());
                }
                
                else if (tempSecProp.Type == "RHS")
                {
                    if (tempSecProp.IsTapered)
                    {
                        if (!tempSecProp.TaperIntermediatePos[1].IsValidNumber() || tempSecProp.TaperIntermediatePos[1] == 0)
                        {
                            familySymName.Add("Tapered1 " + tempSecProp.Type + " " + (tempSecProp.TaperDepth[0] * 1000).ToString() + "x" + (tempSecProp.TaperDepth[1] * 1000).ToString() + "x" + (tempSecProp.TaperDepth[3] * 1000).ToString() + (tempSecProp.B * 1000).ToString() + "x" + (tempSecProp.TaperIntermediatePos[0]).ToString() + "x" + (tempSecProp.CutbackS * 1000).ToString() + "x" + (tempSecProp.CutbackE * 1000).ToString());
                        }

                        else
                        {
                            familySymName.Add("Tapered2 " + tempSecProp.Type + " " + (tempSecProp.TaperDepth[0] * 1000).ToString() + "x" + (tempSecProp.TaperDepth[1] * 1000).ToString() + "x" + (tempSecProp.TaperDepth[3] * 1000).ToString() + (tempSecProp.B * 1000).ToString() + "x" + (tempSecProp.TaperIntermediatePos[0]).ToString() + "x" + (tempSecProp.TaperIntermediatePos[1]).ToString() + "x" + (tempSecProp.CutbackS * 1000).ToString() + "x" + (tempSecProp.CutbackE * 1000).ToString());
                        }
                       
                    }
                    else
                    {
                        familySymName.Add(tempSecProp.Type + " " + (tempSecProp.B * 1000).ToString() + "x" + (tempSecProp.D * 1000).ToString() + "x" + (tempSecProp.tw * 1000).ToString());
                    }
                   
                }

                else if (tempSecProp.Type == "CHS" || (tempSecProp.Type == "EXP" && tempSecProp.Name != "LT.Ring" && tempSecProp.Name != "UT.Ring"))
                {
                    double t = tempSecProp.tw * 1000;
                    double D = tempSecProp.D * 1000;
                    if (tempSecProp.Type == "EXP")
                    {
                        t = 5;
                    }
                    if (D == 0)
                    {
                        D = 20;
                    }
                    familySymName.Add(tempSecProp.Type + " " + D.ToString() + "x" + t.ToString());
                }

                 else if (tempSecProp.Type == "EXP" && tempSecProp.Name == "LT.Ring")
                {
                    familySymName.Add("cableFamily_8");
                }

                else if (tempSecProp.Type == "EXP" && tempSecProp.Name == "UT.Ring")
                {
                    familySymName.Add("cableFamily_4");
                }
            }

            return new Dictionary<string, object>
            {
                {"StartNodePoint", startPt},
                {"EndNodePoint", endPt},
                {"Index", indicies}, 
                {"PropInex", propIndex}, 
                {"SectionProperties", secProp}, 
                {"GA" , gammaAngles}, 
                {"Dummy", dummyList}, 
                {"ElemType", elemType}, 
                {"Name", name}, 
                {"Group", beamGroup}, 
                {"PropName", propName},
                {"FamilySymbolName", familySymName},
                {"Tapered prop", isTap}
            };
        }

         [MultiReturn(new[] { "RHS", "CHS", "UC", "Unknown" })]
        public static Dictionary<string, object> DeconstructSectionProperty(List<SectionProperty> secProp)
        {
             List<string> RHS = new List<string>();
             List<string> CHS = new List<string>();
             List<string> uC = new List<string>();
             List<string> unKnown = new List<string>();
             foreach (SectionProperty sp in secProp)
             {
                 string type = sp.Type;
                 string B = ((sp.B) * 1000).ToString();
                 string D = ((sp.D) * 1000).ToString();
                 string tw = ((sp.tw) * 1000).ToString();
                 string tf = ((sp.tf) * 1000).ToString();
                 string revitSecType;
                 revitSecType = type;
                 
                 if (type == "RHS" )
                 {
                     revitSecType = type + D + "x" + B + "x" + tw + "x" + tf;
                     if (!RHS.Contains(revitSecType))
                     {
                         RHS.Add(revitSecType);
                     }
                 }

                 else if (type == "CHS")
                 {
                     revitSecType = type + D + "x" + tw;
                     if (!CHS.Contains(revitSecType))
                     {
                         CHS.Add(revitSecType);
                     }
                     
                 }

                 else if (type == "UC")
                 {
                     revitSecType = type + D + "x" + B + "x" + tw + "x" + tf;
                     if (!uC.Contains(revitSecType))
                     {
                         uC.Add(revitSecType);
                     }
                     
                 }

                 else
                 {
                     unKnown.Add(sp.Name);
                 }

             }

             return new Dictionary<string, object>
            {
                { "RHS", RHS},
                {"CHC", CHS},
                {"UC", uC},
                {"Unknown", unKnown}
            };
        }

        public static List<Autodesk.DesignScript.Geometry.Line> GetLinesFromBeams(List<Beam> beam)
        {
            List<Line> beams = new List<Line>();

            for (int i = 0; i < beam.Count; i++)
            {
                Point sPt;
                Point ePt;
                Line tempLine;

                sPt = Point.ByCoordinates(beam[i].StartNode.X * 1000, beam[i].StartNode.Y * 1000, beam[i].StartNode.Z * 1000);
                ePt = Point.ByCoordinates(beam[i].EndNode.X * 1000, beam[i].EndNode.Y * 1000, beam[i].EndNode.Z * 1000);

                tempLine = Line.ByStartPointEndPoint(sPt, ePt);
                beams.Add(tempLine);
            }

            return beams;
        }



        public static List<string> GetSectionPropertiesFromStructure(List<Beam> beam)
        {

            List<string> secProp = new List<string>();

            foreach (Beam b in beam)
            {
                string type = b.SectionProperty.Type;
                string B = ((b.SectionProperty.B) * 1000).ToString();
                string D = ((b.SectionProperty.D) * 1000).ToString();
                string tw = ((b.SectionProperty.tw) * 1000).ToString();
                string tf = ((b.SectionProperty.tf) * 1000).ToString();
                string revitSecType;
                revitSecType = type;

                if (type == "RHS" || type == "SHS")
                {
                    revitSecType = type + D + "x" + B + "x" + tw + "x" + tf;
                }

                if (type == "CHS")
                {
                    revitSecType = type + D + "x" + tw;
                }

                if (type == "UC")
                {
                    revitSecType = type + D + "x" + B + "x" + tw + "x" + tf;
                }

                secProp.Add(revitSecType);
            }
            return secProp;
        }

        public static List<double> GetGammaAngels(List<Beam> beam)
        {
            List<double> orientAng = new List<double>();
            foreach (var b in beam)
            {
                double ang = b.OrientationAngle;
                orientAng.Add(ang);
            }

            return orientAng;
        }

        [MultiReturn(new[] { "Add", "Multi", "Div", "Sub" })]
        public static Dictionary<string, object> MultiOut(double x, double y)
        {
            double add = x + y;
            double multi = x * y;
            double div = x / y;
            double sub = x - y;

            return new Dictionary<string, object>
            {
                {"Add", add},
                {"Multi", multi},
                {"Div", div},
                {"Sub", sub}
            };
        }

        public static bool HighlightRhinoObjByGUID(List<string> guid)
        {
            bool success = false;
            string path = @"C:\Users\phasarak\AppData\Roaming\McNeel\ListofGUIDs.csv";
            string delimiter = ",";
            StringBuilder sb = new StringBuilder(); 
            if (File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            for (int i = 0; i < guid.Count; i++)
            {
                sb.AppendLine(string.Join(delimiter, guid[i]));
                File.WriteAllText(path, sb.ToString()); 
            }



            return success;
        }

        public static List<string> ReadGUID(bool execute)
        {
            List<string> GUIDs = new List<string>();

            if (execute)
            {
                var reader = new StreamReader(File.OpenRead(@"C:\Users\phasarak\AppData\Roaming\McNeel\ListofGUIDs.csv"));
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    GUIDs.Add(line);
                }

                
            }
            return GUIDs;
        }


        public static List<Line> ChangedLines(List<Line> lns, List<string> GUID, List<string> changedGuid)
        {
            List<Line> outputLns = new List<Line>();

            for (int i = 0; i < changedGuid.Count; i++)
            {
                for(int j = 0; j < GUID.Count; j++)
                {
                    if (GUID[j] == changedGuid[i])
                    {
                        outputLns.Add(lns[j]);
                        break;
                    }
                }
            }
            return outputLns;
        }

        public static Point GetClosestPoint(Point pt, List<Point> ptCloud)
        {
            Point closePt=ptCloud[0];

            double distMin = pt.DistanceTo(closePt);

            foreach (Point ptTemp in ptCloud)
            {
                double nyDist = pt.DistanceTo(ptTemp);

                if (nyDist<distMin)
                {
                    closePt = ptTemp;
                    distMin = nyDist;
                }
 
            }

            return closePt;

        }



        
    }

}
