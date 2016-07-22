using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace Robot
{
    /// <summary>
    /// Robot panel tools
    /// BuroHappold
    /// <class name="RobotPanels">Robot panel tools</class>
    /// </summary>
    public class RobotPanel
    {
        internal RobotPanel() {}
       /// <summary>
       /// 
       /// </summary>
       /// <param name="surfaces"></param>
       /// <param name="thicknessNames"></param>
       /// <param name="panelType"></param>
       /// <param name="isOpening"></param>
       /// <param name="deletePanels"></param>
       /// <param name="activate"></param>
       /// <returns></returns>
        public static List<int> CreateBySurfaces(
            Surface[] surfaces,
            [DefaultArgument("{\"0\"}")] string[] thicknessNames,
            [DefaultArgument("{\"0\"}")] string[] panelType,
            bool isOpening = false,
            bool deletePanels = false,
            bool activate = false)
        {
            
            //Output dictionary definition
            Dictionary<string, object> createpanels_out = new Dictionary<string, object>();

            //Panel parameters
            Dictionary<string, Object> panels = new Dictionary<string, Object>();
            List<int> panel_nums = new List<int>();
            List<double[,]> defpnt_coords =  new List<double[,]>();
            int kounta = 1;
           
            List<string> _thicknessNames = new List<string>();

            if (activate == true)
            {
                RobotToolkit.Panel.GetFreePanelNumber(out kounta);
                for (int i = 0; i < surfaces.Length; i++) 
                {
                    Surface srf = surfaces[i];
                    double[,] coords = new double[srf.Vertices.Length,3];
                    for (int j = 0; j < srf.Vertices.Length; j++)
                    { 
                        Point pnt = (Point)srf.Vertices[j].PointGeometry;
                        coords[j,0] = pnt.X;
                        coords[j,1] = pnt.Y;
                        coords[j,2] = pnt.Z;
                    }
                    defpnt_coords.Add(coords);
                    panel_nums.Add(kounta);
                    kounta = kounta + 1;

                    if (thicknessNames.Count() < surfaces.Count())
                    {
                        _thicknessNames.Add(thicknessNames[0]);
                    }
                    else
                    {
                        _thicknessNames.Add(thicknessNames[i]);
                    }

                  
                }
                if (deletePanels) RobotToolkit.Panel.DeletePanels("all");
                if (!isOpening)
                {
                    RobotToolkit.Panel.CreatePanels(panel_nums, defpnt_coords, _thicknessNames);
                }
                else
                {
                    RobotToolkit.Panel.CreateOpenings(panel_nums, defpnt_coords);
                }
                RobotToolkit.General.RefreshView();
                createpanels_out.Add("PanelNumbers", panel_nums);
               
                               
            }
            return panel_nums;
        }

        /// <summary>
        /// Creates Panels and Contours in Robot. Note: edge curves are simplifed to polylines.
        /// BuroHappold
        /// </summary>
        /// <param name="activate">Use a boolean 'true/false' node to activate</param>
        /// <param name="contours">Set to true to include contours in the import</param>
        /// <returns name="Panel Surfaces">Patch surface based on polycurves with straight segments</returns>
        /// <returns name="Panel Numbers">Robot panel numbers in same sequential order as panel surfaces</returns>
        /// <returns name="ContourPolyCurves">Polycurves for countours</returns>
        /// <returns name="ContourNumbers">Robot contour numbers in same sequential order as panel surfaces</returns>
        /// <search>
        /// BH, robot, structure, panels import
        /// </search>

        [MultiReturn(new[] { "PanelSurfaces", "PanelNumbers", "ContourPolycurves", "ContourNumbers" })]
        public static Dictionary<string, object> Import(bool activate = false, bool contours = false)
        {
            //Output dictionary definition
            Dictionary<string, object> getpanels_out = new Dictionary<string, object>();

            //Panel parameters
            Dictionary<string, Object> panels = new Dictionary<string, Object>();
            int[] panel_nums = null;
            double[][,] defpnt_coords = null;
            Dictionary<string, Object> conts = new Dictionary<string, Object>();
            int[] cont_nums = null;

            if (activate == true)
            {
                RobotToolkit.Panel.GetPanels(out panel_nums, out defpnt_coords);
                for (int i = 0; i < panel_nums.Length; i++)
                {
                    List<Point> defpnts = new List<Point>();
                    for (int j = 0; j < defpnt_coords[i].GetLength(0); j++)
                    {
                        defpnts.Add(Point.ByCoordinates(defpnt_coords[i][j, 0], defpnt_coords[i][j, 1], defpnt_coords[i][j, 2]));
                    }
                    
                    PolyCurve pcrv = PolyCurve.ByPoints(defpnts, true);
                    panels.Add(panel_nums[i].ToString(), PolySurface.ByPatch(pcrv));
                    pcrv.Dispose();
                }
                if (contours == true)
                {
                    RobotToolkit.Panel.GetContours(out cont_nums, out defpnt_coords);
                    for (int i = 0; i < cont_nums.Length; i++)
                    {
                        List<Point> defpnts = new List<Point>();
                        for (int j = 0; j < defpnt_coords[i].GetLength(0); j++)
                        {
                            defpnts.Add(Point.ByCoordinates(defpnt_coords[i][j, 0], defpnt_coords[i][j, 1], defpnt_coords[i][j, 2]));
                        }
                        conts.Add(cont_nums[i].ToString(), PolyCurve.ByPoints(defpnts, true));
                      }
                }
                getpanels_out.Add("PanelSurfaces", panels.Values);
                getpanels_out.Add("PanelNumbers", panel_nums);
                if (contours == true) getpanels_out.Add("ContourPolycurves", conts.Values);
                if (contours == true) getpanels_out.Add("ContourNumbers", cont_nums);
            }
            return getpanels_out;
        }

        /// <summary>
        /// Updates panel and contour geometry as surfaces from Robot. Note: edge curves are simplifed to polylines.
        /// BuroHappold
        /// </summary>
        /// <param name="surfaces">Input surfaces used to define updated geometry</param>
        /// <param name="panelNumbers"></param>
        /// <param name="activate">Set true to activate the node</param>
        /// <search>
        /// BH, robot, structure, panels
        /// </search>

        [MultiReturn(new[] { "PanelNumbers" })]
        public static Dictionary<string, object> UpdateGeometry(
            Surface[] surfaces,
            int[] panelNumbers,
            bool activate = false)
        {
            //Output dictionary definition
            Dictionary<string, object> updatepanels_out = new Dictionary<string, object>();

            //Panel parameters
            List<double[,]> defpnt_coords = new List<double[,]>();
            if (activate == true)
            {
                for (int i = 0; i < surfaces.Length; i++)
                {
                    Surface srf = surfaces[i];
                    double[,] coords = new double[srf.Vertices.Length, 3];
                    for (int j = 0; j < srf.Vertices.Length; j++)
                    {
                        Point pnt = (Point)srf.Vertices[j].PointGeometry;
                        coords[j, 0] = pnt.X;
                        coords[j, 1] = pnt.Y;
                        coords[j, 2] = pnt.Z;
                    }
                    defpnt_coords.Add(coords);
 }
                RobotToolkit.Panel.UpdateGeometry(panelNumbers.ToList(), defpnt_coords);
                RobotToolkit.General.RefreshView();
                updatepanels_out.Add("PanelNumbers", panelNumbers);

            }
            return updatepanels_out;
        }
    
    }
}

