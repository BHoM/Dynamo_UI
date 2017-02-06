using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BHoM.Structural.Elements;



namespace CableNetDesign_Basilisk
{
    public static class TRCbleFamilyType 
    {
        public static void CreateTRCableFamilyType(List<BHE.Bar> bars, string typeParentName)
        {
            CableNetDesign_Revit.TRCableFamilyType.CreateTRCableFamilyType(bars, typeParentName);
        }
    }
}
