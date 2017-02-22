using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHE = BHoM.Structural.Elements;



namespace CableNetDesign_Basilisk
{
    public static class RSSFamilyType 
    {
        public static void CreateRSSFamilyType(List<BHE.Bar> bars, string typeParentName)
        {
            CableNetDesign_Revit.RSSFamilyType.CreateRSSFamilyType(bars, typeParentName);
        }
    }
}
