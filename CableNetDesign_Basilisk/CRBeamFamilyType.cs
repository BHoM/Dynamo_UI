using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BHoM.Structural.Elements;



namespace CableNetDesign_Basilisk
{
    public static class CRBeamFamilyType 
    {
        public static void CreateCRBeamFamilyType(List<BHE.CompressionRingBeam> crBeams, string typeParentName)
        {
            CableNetDesign_Revit.CRBeamFamilyType.CreateCRBeamFamilyType(crBeams, typeParentName);
        }
    }
}
