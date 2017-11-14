using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BH.oM.Environmental;

namespace Basilisk.Environmental
{
    public static partial class Create
    {

        public static BHE.Elements.Location Location(string name, double latitude, double longitude, double elevation)
        {

            return new BHE.Elements.Location(name, latitude, longitude, elevation);

        }

    }
}
