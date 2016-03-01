using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using RobotToolkit;
using BHoM.Structural;

namespace Robot
{
    /// <summary>
    /// Robot node tools
    /// BuroHappold
    /// <class name="RobotNodes">Robot node tools</class>
    /// </summary>
    public class RobotNode
    {
        internal RobotNode() { }

        /// <summary>
        /// Creates Robot nodes from a BHoM structural node. 
        /// BuroHappold
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="allowRenumbering"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Nodes" })]
        public static Dictionary<string, object> CreateByNodes(IEnumerable<BHoM.Structural.Node> nodes, bool allowRenumbering = true, bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> nodes_out = new Dictionary<string, object>();
            if (activate)
            {
                int free_bar_num = 0;
                int free_nod_num = 0;
                int free_obj_num = 0;
                RobotToolkit.General.GetFreeNumbers(out free_nod_num, out free_bar_num, out free_obj_num);
                int nod_kounta = free_nod_num;
                Dictionary<string, BHoM.Structural.Constraint> restraintCollection = new Dictionary<string, BHoM.Structural.Constraint>();
                List<BHoM.Structural.Constraint> restraints = new List<Constraint>();

                if (allowRenumbering)
                {
                    foreach (BHoM.Structural.Node node in nodes)
                    {
                        if (node.Number < nod_kounta)
                        {
                            node.SetNumber(nod_kounta);
                            nod_kounta++;
                        }
                    }
                }
                RobotToolkit.Node.CreateNodesByCache(nodes.ToArray());
                RobotToolkit.Node.SetRestraints(nodes.ToList());
                RobotToolkit.General.RefreshView();
            }
            nodes_out.Add("Nodes", nodes.ToArray());
            return nodes_out;
        }


        /// <summary>
        /// Get nodes from Robot model
        /// BuroHappold
        /// </summary>
        /// <param name="activate">Set true to activate</param>
        /// <param name="allNodeData">If true gets all data for each node - slower method</param>
        /// <returns></returns>
        /// <search>BH, robot, nodes</search>
        [MultiReturn(new[] { "Points", "Numbers", "Nodes" })]
        public static Dictionary<string, object> Import(bool activate = false, bool allNodeData = false)
        {

            BHoM.Global.Project project = new BHoM.Global.Project();

            //Output dictionary definition
            Dictionary<string, object> getnodes_out = new Dictionary<string, object>();
            List<Point> points = new List<Point>();
            List<int> nodeNumbers = new List<int>();

            if (activate == true)
            {
                RobotToolkit.Node.GetNodesQuery(project);
                foreach (BHoM.Structural.Node str_node in project.Structure.Nodes)
                {
                    Point pnt = Point.ByCoordinates(str_node.X, str_node.Y, str_node.Z);
                    points.Add(pnt);
                    nodeNumbers.Add(str_node.Number);
                }
                getnodes_out.Add("Points", points);
                getnodes_out.Add("Numbers", nodeNumbers);
                getnodes_out.Add("Nodes", project.Structure.Nodes);
            }
            return getnodes_out;
        }

        /// <summary>
        /// Set a support in Robot at a node. Input node numbers and restraints to apply supports to existing nodes in Robot. 
        /// Input BHoM node objects to generate new nodes with the associated restraints. If BHoM nodes don't already have restraints, 
        /// add them here. 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="restraintName"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Nodes" })]
        public static Dictionary<string, object> SetProperties(IEnumerable<BHoM.Structural.Node> nodes, string restraintName)
        {
            //Output dictionary definition
            Dictionary<string, object> restraints_out = new Dictionary<string, object>();

            //RobotToolkit.Node.SetProperties(nodes.ToList(), restraintName);
            RobotToolkit.General.RefreshView();
            restraints_out.Add("Nodes", nodes);
            return restraints_out;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterNodeNumber"></param>
        /// <param name="slaveNodeNumbers"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Bool" })]
        public static Dictionary<string, object> RigidLinks(int masterNodeNumber, IEnumerable<int> slaveNodeNumbers, bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> getnodes_out = new Dictionary<string, object>();

            if (activate == true)
            {
                RobotToolkit.Node.SetRigidLinks(masterNodeNumber, slaveNodeNumbers.ToList());
            }
            return getnodes_out;
        }
    }

}

