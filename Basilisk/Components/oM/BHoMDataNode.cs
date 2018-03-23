using Dynamo.Graph.Nodes;
using BH.UI.Basilisk.Templates;
using System;
using System.Collections.Generic;
using ProtoCore.AST.AssociativeAST;
using System.Xml;
using Dynamo.Graph;

namespace BH.UI.Basilisk.Components
{
    [NodeName("BHoMData")]
    [NodeDescription("Create BHoM Reference Data")]
    [NodeCategory("Basilisk.oM")]
    [IsDesignScriptCompatible]
    public class BHoMDataNode : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public string FileName { get; set; } = "";

        public string ItemName { get; set; } = "";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMDataNode()
        {
            OutPortData.Add(new PortData("  ", "selected data"));
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Make sure the enum has been assigned
            if (FileName == "" || ItemName == null)
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };


            // Create the Build assignment for outpout 0
            List<AssociativeNode> arguments = new List<AssociativeNode> { AstFactory.BuildStringNode(FileName), AstFactory.BuildStringNode(ItemName) };
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.Basilisk.Methods.Create", "BHoMData", arguments);
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            var fileName = xmlDoc.CreateElement("FileName");
            fileName.SetAttribute("value", FileName);
            element.AppendChild(fileName);

            var itemName = xmlDoc.CreateElement("ItemName");
            itemName.SetAttribute("value", ItemName);
            element.AppendChild(itemName);
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            List<Type> paramTypes = new List<Type>();

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "FileName":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                            FileName = node.Attributes["value"].Value;
                        break;
                    case "ItemName":
                        if (node.Attributes != null && node.Attributes["value"] != null)
                            ItemName = node.Attributes["value"].Value;
                        break;
                }
            }
        }

        /***************************************************/
    }
}
