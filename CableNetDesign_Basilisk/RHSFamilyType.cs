using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BHoM.Structural.Elements;



namespace CableNetDesign_Basilisk
{
    public static class RHSFamilyType 
    {
        public static void CreateRHSFamilyType(List<BHE.Bar> bars, string typeParentName)
        {
            CableNetDesign_Revit.RHSFamilyType.CreateRHSFamilyType(bars, typeParentName);
        }
    }
}
