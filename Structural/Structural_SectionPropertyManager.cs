using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.DesignScript.Runtime;

using DynamoServices;

namespace Structural
{

    class SectionPropertyManager
    {
        internal SectionPropertyManager() { }
        private static int secPropID = 1;

        public static int GetNextUnusedID()
        {
            int next = secPropID;
            secPropID++;
            return next;
        }


    }
}