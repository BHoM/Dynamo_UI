using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Grasshopper_Engine;
//using Grasshopper.Kernel;
using BHE = BHoM.Structural.Elements;

namespace CableNetDesign_Basilisk
{
//    class Test_Import
//    {
//    }
//}
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Grasshopper_Engine;
////using Grasshopper.Kernel;
////using BHE = BHoM.Structural.Elements;
////using Autodesk.Revit;
////using ADG = Autodesk.DesignScript.Geometry;
////using Autodesk.Revit.DB;

//namespace Revit_Alligator
//{


    public static class Test_Import
    {
       
          public static void  CreateBeam(BHoM.Structural.Elements.Bar bar, string typename)
        {
            CableNetDesign_Revit.ImportBeamTest.ImportBeam(bar, typename);
        
        }

    }
}
