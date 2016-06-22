using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material
{
    public static class Material
    {
        public static BHoM.Materials.Material CreateMaterial(string name)
        {
            return new BHoM.Materials.Material(name);
        }
    }
}
