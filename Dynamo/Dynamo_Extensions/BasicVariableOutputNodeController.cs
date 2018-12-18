using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamo.Graph.Nodes
{
    public class BasicVariableOutputNodeController : VariableOutputNodeController
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BasicVariableOutputNodeController(VariableOutputNode node) : base(node)
        {
            model = node;
        }


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public void RemoveOutputBase()
        {
            base.RemoveOutputFromModel();
        }

        /*******************************************/

        public void AddOutputBase()
        {
            base.AddOutputToModel();
        }

        /*******************************************/

        public int GetOutputIndexBase()
        {
            return base.GetOutputIndexFromModel();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        protected override string GetOutputName(int index)
        {
            return model.GetOutputName(index);
        }

        /*******************************************/

        protected override string GetOutputTooltip(int index)
        {
            return model.GetOutputTooltip(index);
        }

        /*******************************************/

        protected override void AddOutputToModel()
        {
            model.AddOutput();
        }

        /*******************************************/

        protected override void RemoveOutputFromModel()
        {
            model.RemoveOutput();
        }

        /*******************************************/

        public override int GetOutputIndexFromModel()
        {
            return model.GetOutputIndex();
        }


        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private readonly VariableOutputNode model;


        /*******************************************/
    }
}
