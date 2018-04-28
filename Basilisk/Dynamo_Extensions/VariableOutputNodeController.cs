using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dynamo.Graph.Nodes
{
    /// <summary>
    /// This is a helper class that processess ouputs of VariableOutputNode.
    /// </summary>
    public abstract class VariableOutputNodeController
    {
        /*******************************************/
        /**** Template Methods                  ****/
        /*******************************************/

        protected abstract string GetOutputName(int index);

        protected abstract string GetOutputTooltip(int index);


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        protected VariableOutputNodeController(NodeModel model)
        {
            this.model = model;
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public virtual int GetOutputIndexFromModel()
        {
            return model.OutPortData.Count;
        }

        /*******************************************/

        protected virtual void RemoveOutputFromModel()
        {
            var count = model.OutPortData.Count;
            if (count > 0)
                model.OutPortData.RemoveAt(count - 1);
        }

        /*******************************************/

        protected virtual void AddOutputToModel()
        {
            var idx = GetOutputIndexFromModel();
            model.OutPortData.Add(new PortData(GetOutputName(idx), GetOutputTooltip(idx)));
        }

        /*******************************************/

        public void SetNumOutputs(int numOutputs)
        {
            // Ignore negative values, as those are impossible.
            if (numOutputs < 0)
            {
                return;
            }

            // While the current number of ports doesn't match
            // the desired number of ports, add or remove ports
            // as appropriate.  This is intentionally a "best effort"
            // operation, as the node may reject attempts to create
            // or remove too many ports.  As such, we ignore any
            // failures to add or remove ports.
            for (int current = model.OutPortData.Count; current != numOutputs;)
            {
                if (current < numOutputs)
                {
                    AddOutputToModel();
                    ++current;
                }
                else
                {
                    RemoveOutputFromModel();
                    --current;
                }
            }

            MarkNodeDirty();
        }

        /*******************************************/

        public void OnBuilt()
        {
            outputAmtLastBuild = model.OutPortData.Count;

            foreach (var idx in Enumerable.Range(0, model.OutPortData.Count))
                connectedLastBuild[idx] = model.HasOutput(idx);
        }

        /*******************************************/

        public static void SerializeOutputCount(XmlElement nodeElement, int amount)
        {
            nodeElement.SetAttribute("outputcount", amount.ToString());
        }

        /*******************************************/

        public void SerializeCore(XmlElement element, SaveContext context)
        {
            SerializeOutputCount(element, model.OutPortData.Count);
        }

        /*******************************************/

        public void DeserializeCore(XmlElement element, SaveContext context)
        {
            try
            {
                //base.DeserializeCore(element, context); //Base implementation must be called
                int amt = Convert.ToInt32(element.Attributes["outputcount"].Value);
                SetNumOutputs(amt);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            model.RegisterAllPorts();
        }

        /*******************************************/

        public void AddOutPort()
        {
            AddOutputToModel();
            model.RegisterAllPorts();
        }

        /*******************************************/

        public void RemoveOutPort()
        {
            RemoveOutputFromModel();
            model.RegisterAllPorts();
        }

        /*******************************************/

        public void SetOutPortCount(int value)
        {
            SetNumOutputs(value);
            model.RegisterAllPorts();
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private void MarkNodeDirty()
        {
            var dirty = model.OutPortData.Count != outputAmtLastBuild
                || Enumerable.Range(0, model.OutPortData.Count).Any(idx => connectedLastBuild[idx] == model.HasOutput(idx));

            if (dirty)
            {
                model.OnNodeModified();
            }
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private readonly NodeModel model;
        private int outputAmtLastBuild;
        private readonly Dictionary<int, bool> connectedLastBuild = new Dictionary<int, bool>();


        /*******************************************/
    }
}
