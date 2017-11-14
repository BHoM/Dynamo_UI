using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;
using BHE = BH.oM.Environmental;

namespace Basilisk.Environmental
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/


        public static DynamoUnits.Location ToDesignScript(BHE.Elements.Location bhomlocation)
        {

            string placename = bhomlocation.Name;
            double latitude = bhomlocation.Latitude;
            double longitude = bhomlocation.Longitude;

            return DynamoUnits.Location.ByLatitudeAndLongitude(latitude, longitude, placename);

        }

    }
}