
namespace Robot.Results.Bars
{
    /// <summary>
    /// 
    /// </summary>
    public class RobotBarForce
    {
        internal RobotBarForce() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activate"></param>
        /// <returns></returns>
        public static BHoM.Structural.Results.Bars.BarForceCollection GetBarForces(bool activate = false)
        {
            return RobotToolkit.Results.Bars.BarForces.GetBarForcesQuery(new BHoM.Global.Project());
         }
    }
}
