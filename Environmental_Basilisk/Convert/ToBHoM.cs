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

        public static BHE.Elements.Location ToBHoM(DynamoUnits.Location dynamoLocation)
        {

            string placename = dynamoLocation.Name;
            double latitude = dynamoLocation.Latitude;
            double longitude = dynamoLocation.Longitude;

            return new BHE.Elements.Location(placename, latitude, longitude, 0);

        }

    }
}