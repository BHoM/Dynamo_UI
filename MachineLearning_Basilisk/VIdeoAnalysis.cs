using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public static class VideoAnalysis
    {
        public static Dictionary<int, double> MotionLevel(string videoFile, int startFrame = 0, int endFrame = int.MaxValue, string outFolder = "", bool trigger = false)
        {
            if (trigger)
            {
                MachineLearning_Engine.MotionLevelAnalyser analyser = new MachineLearning_Engine.MotionLevelAnalyser();
                return analyser.Run(videoFile, startFrame, endFrame, outFolder).Result;
            }
            else
                return null;
        }
    }
}
