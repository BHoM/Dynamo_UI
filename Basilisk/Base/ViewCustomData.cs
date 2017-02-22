using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHB = BHoM.Base;
using Autodesk.DesignScript.Runtime;

namespace Base
{
    public static class ViewCustomData
    {
        [MultiReturn(new[] {"Key", "Value" })]
        public static Dictionary<string,List<object>> GetCustomData(object objs = null)
        {
            string[] columnheader = { "Key", "Value" };
            Dictionary<string, List<object>> output = new Dictionary<string, List<object>>();

            BHB.BHoMObject bObj = objs as BHB.BHoMObject;


            output[columnheader[0]] = bObj.CustomData.Keys.Select(x => x as object).ToList();
            output[columnheader[1]] = bObj.CustomData.Values.ToList();


            //List<string> keys = new List<string>();
            //List<object> values = new List<object>();

            //foreach (BHB.BHoMObject obj in objs.Cast<BHB.BHoMObject>().ToList())
            //{
            //    foreach (KeyValuePair<string, object> keyval in obj.CustomData)
            //    {
            //        values.Add(keyval.Value);
            //        keys.Add(keyval.Key);
            //    }
            //}
            //output.Add(keys, values);

            return output;
        }
    }
}
