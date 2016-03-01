using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    /// Robot finite element tools
    /// BuroHappold
    /// <class name="FiniteElement">Robot finite element tools</class>
    /// </summary>
    public class RobotFE
    {
        internal RobotFE() {}
        /// <summary>
        /// Gets 2D shell finite element shells from Robot
        /// BuroHappold
        /// </summary>
        /// <param name="activate">Set true to activate</param>
        /// <returns></returns>
        /// <search>BH, robot, fe</search>
        [MultiReturn(new[] { "FEMeshes", "Nodes" })]
        public static Dictionary<string, object> Import(bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> getmeshes_out = new Dictionary<string, object>();

            //Mesh parameters
            int[] panel_ids = null;
            double[][] nodeCoords = null;
            Dictionary<int, int[]> vertex_indices = new Dictionary<int, int[]>();
            List<Point> vertices = new List<Point>();
            Dictionary<int, BHoM.Structural.Node> str_nodes = new Dictionary<int, BHoM.Structural.Node>();
            if (activate == true)
            {
                BHoM.Global.Project project = new BHoM.Global.Project();

                RobotToolkit.FiniteElement.GetFEMeshQuery(project, out panel_ids, out nodeCoords, out vertex_indices, out str_nodes);
                List<IndexGroup> _ig_list = new List<IndexGroup>();
                IEnumerable<IndexGroup> ig_list = null;
                for (int i = 0; i < str_nodes.Count; i++)
                {
                    BHoM.Structural.Node str_node = str_nodes.ElementAt(i).Value;
                    vertices.Add(Point.ByCoordinates(str_node.X, str_node.Y, str_node.Z));
                 }
                for (int i = 0; i < vertex_indices.Count; i++)
                {
                    int[] _ig = vertex_indices.ElementAt(i).Value.ToArray();
                    IndexGroup ig = (_ig.Length == 4) ? IndexGroup.ByIndices((uint)_ig[0], (uint)_ig[1], (uint)_ig[2], (uint)_ig[3]) : IndexGroup.ByIndices((uint)_ig[0], (uint)_ig[1], (uint)_ig[2]);
                    _ig_list.Add(ig);
                }

                ig_list = _ig_list.ToArray();
                _ig_list = null;
                nodeCoords = null;
                Mesh msh = Mesh.ByPointsFaceIndices(vertices.ToArray(), ig_list);
                vertices = null;
                getmeshes_out.Add("FEMeshes", msh);
                getmeshes_out.Add("Nodes", str_nodes.Values);
             }
           return getmeshes_out;
        }
    }
       
    
   }

