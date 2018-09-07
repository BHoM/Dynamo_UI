using BH.oM.UI;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Dynamo
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static PortData ToPortData(this ParamInfo info)
        {
            if (info.HasDefaultValue)
            {
                object defaultValue = info.DefaultValue;
                AssociativeNode defaultNode = null;

                if (defaultValue == null)
                    defaultNode = AstFactory.BuildNullNode();
                else
                {
                    switch (defaultValue.GetType().FullName)
                    {
                        case "System.Boolean":
                            defaultNode = AstFactory.BuildBooleanNode((bool)defaultValue);
                            break;
                        case "System.Double":
                            defaultNode = AstFactory.BuildDoubleNode((double)defaultValue);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                            defaultNode = AstFactory.BuildIntNode((int)defaultValue);
                            break;
                        case "System.String":
                            defaultNode = AstFactory.BuildStringNode((string)defaultValue);
                            break;
                        default:
                            break;
                    }
                }

                if (defaultNode == null)
                {
                    BH.Engine.Reflection.Compute.RecordError("Port " + info.Name + " failed to assigned it default value of " + defaultValue.ToString());
                    return new PortData(info.Name, info.Description);
                }   
                else
                    return new PortData(info.Name, info.Description, defaultNode);
            }
            else
            {
                return new PortData(info.Name, info.Description);
            }
        }

        /***************************************************/
    }
}
