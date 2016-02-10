using System.Collections.Generic;
using DS = Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
using DynamoServices;
using KinectToolkit;
using BHoM.HumanBody;
using BHoM.Geometry;


//using Microsoft.Kinect;

namespace Kinect
{
    /// <summary>
    /// This class converts Kinect body frames to Dynamo objects through the BHoM HumanBody classes. 
    /// BuroHappold
    /// </summary>
    public class BodyTracking
    {
        internal BodyTracking() { }

        /// <summary>
        /// Tracks skeletal movement using Kinect sensor
        /// BuroHappold
        /// </summary>
        /// <param name="activate"></param>
        /// <param name="sensor"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Skeletons", "Bool"})]
        [CanUpdatePeriodically(true)]
        public static Dictionary<string, object> GetSkeletons(KinectToolkit.Sensor sensor, bool activate = false)
        { 
            //Output dictionary definition
            Dictionary<string, object> skeleton_out = new Dictionary<string, object>();
            if (activate)
            {
                List<BHoM.Geometry.Point> points = new List<BHoM.Geometry.Point>();
                
                KinectBody kinectBody = new KinectBody();
                List<BHoM.HumanBody.Skeleton> skeletons = new List<BHoM.HumanBody.Skeleton>();
                kinectBody.GetSkeleton(out skeletons, out points, sensor);

            skeleton_out.Add("Skeletons", skeletons);
  
            }
          return skeleton_out;
        }


        /// <summary>
        /// Explode a skeleton into its body parts
        /// BuroHappold
        /// </summary>
        /// <param name="skeleton">Use the GetSkeleton node to get a skeleton from Kinect</param>
        /// <returns></returns>
        [MultiReturn(new[] { "RightHand","RightHandState","RightHandPoint", "LeftHand","LeftHandState","LeftHandPoint", "RightThumb", "LeftThumb"  })]
        public static Dictionary<string, object> GetHands(Skeleton skeleton)
        {
            //Output dictionary definition
            Dictionary<string, object> hands_out = new Dictionary<string, object>();
            
            hands_out.Add("RightHand", skeleton.HandRight);
            hands_out.Add("RightHandState", skeleton.HandRight.State.ToString());
            hands_out.Add("RightHandPoint", ToDesignScriptPoint(skeleton.HandRight.TrackingLine.EndPoint));
            hands_out.Add("LeftHand", skeleton.HandLeft);
            hands_out.Add("LeftHandState", skeleton.HandLeft.State.ToString());
            hands_out.Add("LeftHandPoint", ToDesignScriptPoint(skeleton.HandLeft.TrackingLine.EndPoint));
            hands_out.Add("RightThumb", skeleton.ThumbRight);
            hands_out.Add("LeftThumb", skeleton.ThumbLeft);
            return hands_out;
        }

        /// <summary>
        /// Explode a skeleton into its body parts
        /// BuroHappold
        /// </summary>
        /// <param name="skeleton">Use the GetSkeleton node to get a skeleton from Kinect</param>
        /// <returns></returns>
        [MultiReturn(new[] { "TrackingLines", "BodyPartNames", "Head", "leftHand", "rightHand"})]
        public static Dictionary<string, object> GetSkeletonLines(Skeleton skeleton)
        {
            //Output dictionary definition
            Dictionary<string, object> lines_out = new Dictionary<string, object>();

            List<DS.Line> TrackingLines = new List<DS.Line>();
            List<string> BodyPartNames = new List<string>();

            if (skeleton != null)
            {
                Dictionary<string, Line> trackingLines = skeleton.GetAllTrackingLines();
                foreach (Line line in trackingLines.Values)
                {
                    BodyPartNames.Add(trackingLines.GetEnumerator().Current.Key);
                    TrackingLines.Add(ToDesignScriptLine(line));
                    trackingLines.GetEnumerator().MoveNext();
                }

                try
                {
                    DS.Sphere head = DS.Sphere.ByCenterPointRadius(ToDesignScriptPoint(trackingLines["Neck"].StartPoint), 0.1);
                    lines_out.Add("Head", head);
                    if (skeleton.HandRight.State == HandStateName.Closed) 
                    {
                        lines_out.Add("rightHand", DS.Sphere.ByCenterPointRadius(ToDesignScriptPoint(trackingLines["HandRight"].EndPoint), 0.05));
                    }
                    if (skeleton.HandLeft.State == HandStateName.Closed) 
                    {
                        lines_out.Add("leftHand", DS.Sphere.ByCenterPointRadius(ToDesignScriptPoint(trackingLines["HandLeft"].EndPoint), 0.05));
                    }
                }
                catch { }
            }
            lines_out.Add("TrackingLines", TrackingLines.ToArray());
            lines_out.Add("BodyPartNames", BodyPartNames.ToArray());
            
            return lines_out;
        }

        /// <summary>
        /// Internal function to convert a BHoM line object to a DesignScript line object
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static DS.Line ToDesignScriptLine(BHoM.Geometry.Line line)
        {
          return  DS.Line.ByStartPointEndPoint(
                DS.Point.ByCoordinates(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z),
                DS.Point.ByCoordinates(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z));
        }

        /// <summary>
        /// Convert to designsript point object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static DS.Point ToDesignScriptPoint(BHoM.Geometry.Point point)
        {
            return DS.Point.ByCoordinates(point.X, point.Y, point.Z);
        }

    }
}
