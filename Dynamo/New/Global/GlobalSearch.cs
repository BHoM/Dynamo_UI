using BH.UI.Dynamo.Components;
using BH.UI.Dynamo.Templates;
using BH.UI.Global;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Models;
using Dynamo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Dynamo.Models.DynamoModel;

namespace BH.UI.Dynamo.Global
{
    public static class GlobalSearchMenu
    {
        public static DynamoModel DynamoModel { get; set; } = null;


        /*******************************************/
        /**** Public Methods                    ****/
        /*******************************************/

        public static void Activate()
        {
            if (!m_Activated && Application.Current != null)
            {
                Window window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                GlobalSearch.Activate(window);
                GlobalSearch.ItemSelected += GlobalSearch_ItemSelected;
                m_Activated = true;
            }
        }


        /*******************************************/
        /**** Private Methods                   ****/
        /*******************************************/

        private static void GlobalSearch_ItemSelected(object sender, oM.UI.ComponentRequest request)
        {
            CallerComponent node = null;

            switch (request.CallerType.Name)
            {
                case "ComputeCaller":
                    node = new ComputeComponent();
                    break;
                case "ConvertCaller":
                    node = new ConvertComponent();
                    break;
                case "CreateObjectCaller":
                    node = new CreateObjectComponent();
                    break;
                case "ModifyCaller":
                    node = new ModifyComponent();
                    break;
                case "QueryCaller":
                    node = new QueryComponent();
                    break;
            }
            
            if (node != null && DynamoModel != null)
            {
                CreateNodeCommand command = new CreateNodeCommand(node, 0, 0, true, false);
                DynamoModel.ExecuteCommand(command);
                node.Caller.SetItem(request.SelectedItem);
                node.RefreshComponent();
            }
            
        }

        /*******************************************/
        /**** Private Fields                    ****/
        /*******************************************/

        private static bool m_Activated = false;

        /*******************************************/
    }
}
