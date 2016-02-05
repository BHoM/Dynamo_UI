using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
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
        /// <param name="localAxes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [RegisterForTrace]
        [MultiReturn(new[] { "Node", "Bool"})]
        public static Dictionary<string, object> ByPoint(
            Point point,
            [DefaultArgument("\"Unassigned\"")] string name,
            [DefaultArgument("0")] int number,
            [DefaultArgument("\"Unassigned\"")] object constraint,
            [DefaultArgument("Unassigned")] CoordinateSystem coordSys)
        {
            Dictionary<string, object> nodes_out = new Dictionary<string, object>();
            int node_number = (number == 0)? Structural.NodeManager.GetNextUnusedID() : number;
            BHoM.Structural.Node node = new BHoM.Structural.Node(point.X, point.Y, point.Z, node_number);
            Node.SetProperties(node, name, number, constraint, coordSys);
            
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
                {"CoordSys", node.CoordinateSystem}
            };
        }

        /// <summary>
        /// Set the properties of a BHoM structural node.
        /// BuroHappold
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="constraint"></param>
        /// <param name="coordSys"></param>
        /// <returns></returns>
        //[RegisterForTrace]
        [MultiReturn(new[] { "Node"})]
        public static Dictionary<string, object> SetProperties(BHoM.Structural.Node node,
            [DefaultArgument("\"Unassigned\"")] string name,
            [DefaultArgument("0")] int number,
            [DefaultArgument("\"Unassigned\"")] object constraint,
            [DefaultArgument("Unassigned")] CoordinateSystem coordSys)
        {
            BHoM.Structural.Constraint new_constraint = null;
            if (constraint != "Unassigned")
            {
                try
                {
                    new_constraint = (BHoM.Structural.Constraint)constraint;
                    Structural.ConstraintManager.storeConstraint(new_constraint);
                    new_constraint.AddNode(node);
                }
                catch
                {
                    try
                    {
                        string res_name = (string)constraint;
                        new_constraint = new BHoM.Structural.Constraint();
                        new_constraint.SetName(res_name);
                        
                        if (res_name == "Pinned" | res_name == "pinned") new_constraint.SetPinned();
                        if (res_name == "Fixed" | res_name == "fixed") new_constraint.SetFixed();
                        if (res_name == "Sliding" | res_name == "sliding" | res_name == "UZ") new_constraint.SetSliding();
                    }
                    catch { }
                Structural.ConstraintManager.storeConstraint(new_constraint);
                new_constraint.AddNode(node);
                }
            }
            
          
            if (name != "Unassigned" && name != null) node.SetName(name);
            if (number != 0) node.SetNumber(number);
            if (name != "Unassigned" && constraint != null) node.SetName(name);
            if (constraint != "Unassigned" && constraint != null) node.SetConstraint(new_constraint);
            try  
            {
                node.SetCoordinateSystem(new BHoM.Geometry.Plane(
                    new BHoM.Geometry.Vector(coordSys.XAxis.X, coordSys.XAxis.Y, coordSys.XAxis.Z),
                    new BHoM.Geometry.Vector(coordSys.YAxis.X, coordSys.YAxis.Z, coordSys.YAxis.Z),
                    new BHoM.Geometry.Point(coordSys.Origin.X, coordSys.Origin.Y, coordSys.Origin.Z)));
            }
            catch { }
            return new Dictionary<string, object>
            {
                {"Node", node},
            };
        }

       
    }
}