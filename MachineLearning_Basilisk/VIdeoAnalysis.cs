using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public static class VideoAnalysis
    {
        public static Dictionary<int, List<double>> MotionLevel(string videoFileName, int startFrame = 0, int endFrame = int.MaxValue, string outFolder = "", bool trigger = false)
        {
            if (trigger)
            {

                MachineLearning_Engine.MotionLevelAnalyser analyser = new MachineLearning_Engine.MotionLevelAnalyser();
                MachineLearning_Engine.MotionLevelAnalyser.Config config = new MachineLearning_Engine.MotionLevelAnalyser.Config();
                config.StartFrame = startFrame;
                config.EndFrame = endFrame;
                config.OutFolder = outFolder;
                
                return analyser.Run(videoFileName, config).Result;
            }
            else
                return null;
        }
    }
}
