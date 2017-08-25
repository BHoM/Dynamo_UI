//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BHB = BH.oM.Base;

//namespace Global
//{
//    public static class BHProject
//    {
//        /// <summary></summary>
//        public static object FillProject(object[] objects, string name = "")
//        {
//            BH.oM.Global.Project project = BH.oM.Global.Project.ActiveProject; // We need to have a proper project collection in the BHoM
//            project.Clear();
//            foreach (BHB.BHoMObject obj in objects)
//                project.AddObject(obj);
//            return project;
//        }
//    }
//}
