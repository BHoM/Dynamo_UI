using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using BHE = BHoM.Structural.Elements;

namespace CableNetDesign_Basilisk
{
    public static class RafterFamilyType
    {
        public static void CreateRafterFamilyType(List<BHE.RafterBeam> BHoMRafters, string typeParentName)

        {   
             CableNetDesign_Revit.RafterFamilyType.CreateRafterFamilyType(BHoMRafters, typeParentName);
        }
    }
}
