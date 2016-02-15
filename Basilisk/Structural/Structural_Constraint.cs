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
    public class Constraint
    {        
        internal Constraint(){}
        /// <summary>
        /// Create a BHoM constraint (support/boundary condition/release) for downstream application to nodes, bar ends, surfaces 
        /// and surface edges. If name only is inserted the program will attempt to construct a constraint based on intent 
        /// (e.g. setting the name to "Pinned" will set the constraints input to {-1,-1,-1,0,0,0}, "Fixed" will set contraints 
        /// input to {-1,-1,-1,-1,-1,-1}). Rigid or Elastic constraints can be set. For Friction, Gap, Non-linear and Damping 
        /// constraints, use the relevant node.
        /// BuroHappold
        /// </summary>
        /// <param name="name">Provide a name for the constraint</param>
        /// <param name="dof">Example input {-1,-1,-1,0,0,0} for a pinned support, {0,0,100,0,0,0} for a 
        /// vertical spring with stiffness 100 N/m </param>
        /// <returns></returns>
        /// <search>BH, constraint, restraint, support, release</search>
        //[RegisterForTrace]
        [MultiReturn(new[] {"Constraint"})]
        public static Dictionary<string, object> Create(
        [DefaultArgument("\"Pinned\"")] string name,
        [DefaultArgument("{-1,-1,-1,0,0,0}")] object[] dof)
        {
            Dictionary<string, object> constraints_out = new Dictionary<string, object>();
                 BHoM.Structural.Constraint constraint = new BHoM.Structural.Constraint(
                    Convert.ToDouble(dof[0]),
                    Convert.ToDouble(dof[1]),
                    Convert.ToDouble(dof[2]),
                    Convert.ToDouble(dof[3]),
                    Convert.ToDouble(dof[4]),
                    Convert.ToDouble(dof[5]));
                constraint.SetName(name);
                constraints_out.Add("Constraint", constraint);
            return constraints_out;
        }


        /// <summary>
        /// Deconstructs a BHoM constraint
        /// BuroHappold
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        /// <search>BH, constraint, deconstruct</search>
        [MultiReturn(new[] { "Name", "DOFValues", "DOFDescriptions", "Nodes"})]
        public static Dictionary<string, object> Deconstruct(BHoM.Structural.Constraint constraint)
        {
            double[] res_values = constraint.GetValues();
            string[] res_descriptions = constraint.GetDescriptions();

            return new Dictionary<string, object>
            {
                {"Name", constraint.Name},
                {"DOFValues", res_values},
                {"DOFDescriptions", res_descriptions},
             };
        }
       
    }
}