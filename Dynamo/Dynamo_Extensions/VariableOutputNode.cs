using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Dynamo.Core;
using Dynamo.Graph.Workspaces;
using Dynamo.Graph.Nodes;
using Dynamo.Graph;

namespace Dynamo.Graph.Nodes
{
    /// <summary>
    /// Base class for nodes that have dynamic outgoing ports.
    /// E.g. list.create.
    /// </summary>
    public abstract class VariableOutputNode : NodeModel
    {
        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        public abstract string GetOutputTooltip(int index);

        public abstract string GetOutputName(int index);


        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public BasicVariableOutputNodeController VariableOutputController { get; set; }


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        protected VariableOutputNode()
        {
            VariableOutputController = new BasicVariableOutputNodeController(this);
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public virtual void RemoveOutput()
        {
            VariableOutputController.RemoveOutputBase();
        }

        /*******************************************/

        public virtual void AddOutput()
        {
            VariableOutputController.AddOutputBase();
        }

        /*******************************************/

        public virtual int GetOutputIndex()
        {
            return VariableOutputController.GetOutputIndexBase();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override void OnBuilt()
        {
            VariableOutputController.OnBuilt();
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            VariableOutputController.SerializeCore(element, context);
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement nodeElement, SaveContext context)
        {
            base.DeserializeCore(nodeElement, context);
            VariableOutputController.DeserializeCore(nodeElement, context);
        }

        /*******************************************/
    }
}
