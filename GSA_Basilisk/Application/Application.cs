using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSAA = BH.Adapter.GSA;
using ADR = Autodesk.DesignScript.Runtime;

namespace Basilisk.GSA
{
    public static class GSA
    {
        public static GSAA.GSAAdapter GSAApplication([ADR.DefaultArgument("null")] string filePath)
        {
            GSAA.GSAAdapter app = new GSAA.GSAAdapter(filePath);
            return app;
        }

    }
}