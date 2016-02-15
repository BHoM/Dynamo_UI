using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Autodesk.DesignScript.Runtime;
using DynamoServices;
using BHoM;

namespace Structural
{
    /// <summary>
    /// A class which maintains a static dictionary for storing
    /// objects, keyed by their trace id.
    /// </summary>
    //[IsVisibleInDynamoLibrary(false)]
    public class ConstraintManager
    {
        private static Dictionary<string, BHoM.Structural.Constraint> Constraints = new Dictionary<string, BHoM.Structural.Constraint>();

        /// <summary>
        /// Store constraint object
        /// </summary>
        /// <param name="constraint"></param>
        public static void storeConstraint(BHoM.Structural.Constraint constraint)
        {
            if (Constraints.ContainsKey(constraint.Name))
            {
                constraint = Constraints[constraint.Name];
            }
            else
            {
                Constraints.Add(constraint.Name, constraint);
                //TraceUtils.SetTraceData(constraint.Name, constraint.ID);
            }
        }

        /// <summary>
        /// Clear constriant dictionary
        /// </summary>
        public static void Clear()
        {
            Constraints.Clear();
        }

        /// <summary>
        /// Get all constraints
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, BHoM.Structural.Constraint> GetConstraints()
        {
            return Constraints;
        }
    }

}