using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Material
{
    public static class Material
    {
        public static BH.oM.Materials.Material CreateMaterial(string name)
        {
            return new BH.oM.Materials.Material(name);
        }
    }
}
