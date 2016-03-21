using System;
using System.Collections.Generic;
using System.Xml;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Geometry;

namespace Fastrak
{
    /// <summary>
    /// Fastrak Node class
    /// BuroHappold
    /// <class name="Node">Fastrak node tools</class>
    /// </summary>
    public class FastrakNode
    {
        internal FastrakNode() { }
        
        /// <summary>
        /// Imports nodes from a Fastrak XML file (Tekla Designer 2015 .cxl format)
        /// BuroHappold
        /// </summary>
        /// <param name="fileName">Input a string that represents the Fastrak(Tekla) XML file location and name</param>
        /// <returns></returns>
        [MultiReturn(new[] { "Nodes", "Points", "MultiLevelList" })]
        
        public static Dictionary<string, object> Import(String fileName)
        {
            XmlDocument fastrackXMLDoc = new XmlDocument();
            fastrackXMLDoc.Load(fileName); 
           
                   
            //reading relevant data from XML
            XmlNode fastrackPointsCollection = fastrackXMLDoc.DocumentElement.SelectSingleNode("/Designer/JointCoords");

            //options of output
            List<Point> dynamoPoints= new List<Point>();
            List<List<double>> multiLevelList = new List<List<double>>();
            List<BHoM.Structural.Node> str_nodes = new List<BHoM.Structural.Node>();

            //cycling through points
            foreach (XmlNode point in fastrackPointsCollection.ChildNodes)
            {
               
                // the numbers of items read are dependent on XML format
               string currentNodeName = point.OuterXml;
               string[] currentNodeNameSplit = currentNodeName.Split('"');
                
               double currentNodeIndex = Convert.ToDouble(currentNodeNameSplit[1]);
               double currentNodeX = Convert.ToDouble(point.ChildNodes.Item(0).InnerText);
               double currentNodeY = Convert.ToDouble(point.ChildNodes.Item(1).InnerText);
               double currentNodeZ = Convert.ToDouble(point.ChildNodes.Item(2).InnerText);
               

                //writing Dynamo points list
                Point pnt = Point.ByCoordinates(currentNodeX, currentNodeY, currentNodeZ);
                dynamoPoints.Add(pnt);
                

                //writing multi level list
                List<double> sublist = new List<double>();
                sublist.Add(currentNodeIndex);
                sublist.Add(currentNodeX); 
                sublist.Add(currentNodeY);
                sublist.Add(currentNodeZ);
               
                multiLevelList.Add(sublist);

                //writing BHoM node
                
                BHoM.Global.Project bhomProject = new BHoM.Global.Project();

                BHoM.Structural.NodeFactory nodeFactory = bhomProject.Structure.Nodes;
                str_nodes.Add(nodeFactory.Create((int)currentNodeIndex, pnt.X, pnt.Y, pnt.Z));
            }

            //output
            return new Dictionary<string, object>
            {
                { "Nodes", str_nodes},
                { "Points", dynamoPoints },
                { "MultiLevelList", multiLevelList }
            };

        }
        
        
        /// <summary>
        /// Takes extisting XML file (Tekla Designer 2015 .cxl format) and overwrites joint coordinates
        /// </summary>
        /// <param name="fileName">XML file to be updated</param>
        /// <param name="Nodes">BHoM nodes with coordinates and indexes</param>
        public static void WriteToXML(BHoM.Structural.Node[] Nodes, string fileName) 
        {

            XmlDocument fastrackXMLDoc = new XmlDocument();
            fastrackXMLDoc.Load(fileName);


            //reading relevant data from XML
            XmlNode fastrackPointsCollection = fastrackXMLDoc.DocumentElement.SelectSingleNode("/Designer/JointCoords");
            fastrackPointsCollection.RemoveAll();

            foreach (BHoM.Structural.Node node in Nodes)
            {
                //XmlAttribute idAttribute = new XmlAttribute();
                XmlElement nodeToAdd = fastrackXMLDoc.CreateElement("Coord");
                nodeToAdd.SetAttribute("Id", node.Number.ToString());
                fastrackPointsCollection.AppendChild(nodeToAdd);

                //adding X coordinates
                XmlNode xCoord = fastrackXMLDoc.CreateNode(XmlNodeType.Element, "X", null);
                XmlText xCoordValue = fastrackXMLDoc.CreateTextNode(node.X.ToString());
                nodeToAdd.AppendChild(xCoord);
                xCoord.AppendChild(xCoordValue);

                //adding Y coordinates
                XmlNode yCoord = fastrackXMLDoc.CreateNode(XmlNodeType.Element, "Y", null);
                XmlText yCoordValue = fastrackXMLDoc.CreateTextNode(node.Y.ToString());
                nodeToAdd.AppendChild(yCoord);
                yCoord.AppendChild(yCoordValue);

                //adding Z coordinates
                XmlNode zCoord = fastrackXMLDoc.CreateNode(XmlNodeType.Element, "Z", null);
                XmlText zCoordValue = fastrackXMLDoc.CreateTextNode(node.Z.ToString());
                nodeToAdd.AppendChild(zCoord);
                zCoord.AppendChild(zCoordValue);
            }

            fastrackXMLDoc.Save(fileName);
            
        }
    }
        

}
