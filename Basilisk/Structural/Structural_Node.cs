using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Structural
{
    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class Node
    {        
        internal Node(){}
        /// <summary>
        /// Create a BHoM structural node by point
        /// BuroHappold
        /// </summary>
        /// <param name="point"></param>
        /// <param name="number"></param>
        /// <param name="constraint"></param>
        /// <param name="coordSys"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [RegisterForTrace]
        [MultiReturn(new[] { "Node", "Bool"})]
        public static Dictionary<string, object> ByPoint(
            Point point,
            [DefaultArgument("\"Unassigned\"")] string name,
            [DefaultArgument("0")] int number,
            [DefaultArgument("\"Unassigned\"")] BHoM.Structural.Constraint constraint,
            [DefaultArgument("Unassigned")] CoordinateSystem coordSys)
        {
            Dictionary<string, object> nodes_out = new Dictionary<string, object>();
            int node_number = (number == 0)? Structural.NodeManager.GetNextUnusedID() : number;

            BHoM.Global.Project project = new BHoM.Global.Project();
            BHoM.Structural.NodeFactory nodeFactory = new BHoM.Structural.NodeFactory(project);

            BHoM.Structural.Node node = nodeFactory.Create(node_number, point.X, point.Y, point.Z);
            
            
            nodes_out.Add("Node", node);
            return nodes_out;
        }
       

        /// <summary>
        /// Deconstructs a single BHoM Node
        /// BuroHappold
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        /// <search>BH, node, deconstruct</search>
        [RegisterForTrace]
        [MultiReturn(new[] { "Point", "Number", "Name", "Constraint", "ConstraintName" })]
        public static Dictionary<string, object> Deconstruct(BHoM.Structural.Node node)
        {
            return new Dictionary<string, object>
            {
                {"Point", Autodesk.DesignScript.Geometry.Point.ByCoordinates(node.X, node.Y, node.Z)},
                {"Number", node.Number},
                {"Name", node.Name},
                {"Constraint", node.Constraint},
                {"ConstraintName", node.ConstraintName},
                {"CoordSys", node.Plane}
            };
        }      

       
    }
}