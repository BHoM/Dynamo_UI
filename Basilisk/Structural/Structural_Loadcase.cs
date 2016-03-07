using System.Collections.Generic;

namespace Structural.Loads
{
    /// <summary>
    /// Nodal load class - to create and set BHoM nodal load objects.
    /// BuroHappold
    /// </summary>
    public class Loadcase
    {
        internal Loadcase() { }

        /// <summary>
        /// Create a loadcase by name and number
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static object Create(int number, string name)
        {
            BHoM.Structural.LoadcaseFactory lcaseFactory = new BHoM.Structural.LoadcaseFactory(new BHoM.Global.Project());
            return new Dictionary<string, object> { { "Loadcase", lcaseFactory.Create(number, name) } };
        }
    }
}
