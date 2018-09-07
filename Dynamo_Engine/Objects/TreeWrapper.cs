using Autodesk.DesignScript.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Dynamo.Objects
{
    [IsVisibleInDynamoLibrary(false)]
    public class TreeWrapper
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public List<List<object>> Items { get; set; } = null;


        /***************************************************/
    }
}
