using System;
using BH.UI.Basilisk.Components;
using Dynamo.Controls;
using Dynamo.Wpf;

namespace BH.UI.Basilisk.Views
{
    public class ExplodeObjectView : INodeViewCustomization<ExplodeObjectNode> 
    {
        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public ExplodeObjectView() {}


        /*******************************************/
        /**** Interface Methods                 ****/
        /*******************************************/

        public void CustomizeView(ExplodeObjectNode model, NodeView nodeView)
        {
            model.DynamoEngine = nodeView.ViewModel.DynamoViewModel.Model.EngineController;
        }

        /*******************************************/

        public void Dispose()
        {
            
        }


        /*******************************************/
    }
}
