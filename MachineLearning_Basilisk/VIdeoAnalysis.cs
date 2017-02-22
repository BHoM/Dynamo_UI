using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearning
{
    public static class VideoAnalysis
    {
        public static Dictionary<int, List<double>> MotionLevel(string videoFile, int startFrame = 0, int endFrame = int.MaxValue, int frameStep = 1, int nbRows = 1, int nbColumns = 1, string outFolder = "", bool trigger = false)
        {
            if (trigger)
            {

                MachineLearning_Engine.MotionLevelAnalyser analyser = new MachineLearning_Engine.MotionLevelAnalyser();

                MachineLearning_Engine.MotionLevelAnalyser.Config config = new MachineLearning_Engine.MotionLevelAnalyser.Config();
                config.StartFrame = startFrame;
                config.EndFrame = endFrame;
                config.FrameStep = frameStep;
                config.NbRows = nbRows;
                config.NbColumns = nbColumns;
                config.OutFolder = outFolder;

                return analyser.Run(videoFile, config).Result;
            }
            else
                return null;
        }
    }
}
