using System;
using BH.UI.Dynamo.Components;
using Dynamo.Controls;
using Dynamo.Wpf;

namespace BH.UI.Dynamo.Views
{
    public class PullView : INodeViewCustomization<PullNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public PullView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(PullNode model, NodeView nodeView)
        {
            model.DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;
            model.RegisterAdapter();
        }

        /*******************************************/

        public void Dispose()
        {
            
        }

        /*******************************************/
    }
}
