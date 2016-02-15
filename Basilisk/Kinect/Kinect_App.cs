using System.Collections.Generic;
using DS = Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
using DynamoServices;
using KinectToolkit;

namespace Kinect
{
    /// <summary>
    ///Body tracking using Kinect
    /// </summary>
    public class Sensor
    {
        internal Sensor() { }

       /// <summary>
       /// Get Kinect sensor object
       /// </summary>
       /// <param name="activate"></param>
       /// <returns></returns>
        [MultiReturn(new[] { "Sensor" })]
        public static Dictionary<string, object> GetSensor(bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> sensor_out = new Dictionary<string, object>();
            
            if (activate)
            {
                KinectToolkit.Sensor sensor = new KinectToolkit.Sensor();
                sensor_out.Add("Sensor", sensor);
            }
            else
            {
                KinectToolkit.Sensor.Close();
            }
            return sensor_out;
        }
    }
}
