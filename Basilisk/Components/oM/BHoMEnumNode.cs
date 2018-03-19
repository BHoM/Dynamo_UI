using Dynamo.Graph.Nodes;
using BH.UI.Basilisk.Templates;
using System;
using System.Collections.Generic;
using ProtoCore.AST.AssociativeAST;
using System.Xml;
using Dynamo.Graph;

namespace BH.UI.Basilisk.Components
{
    [NodeName("Enum")]
    [NodeDescription("Creates an enum to choose from the context menu")]
    [NodeCategory("Basilisk.oM")]
    [IsDesignScriptCompatible]
    public class BHoMEnumNode : NodeModel
    {
        /*******************************************/
        /**** Properties                        ****/
        /*******************************************/

        public Type EnumType { get; set; } = null;

        public string EnumValue { get; set; } = "";


        /*******************************************/
        /**** Constructors                      ****/
        /*******************************************/

        public BHoMEnumNode()
        {
            OutPortData.Add(new PortData("  ", "selected enum"));
            RegisterAllPorts();
        }


        /*******************************************/
        /**** Override Methods                  ****/
        /*******************************************/

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            // Make sure the enum has been assigned
            if (EnumType == null || EnumValue == "")
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };


            // Create the Build assignment for outpout 0
            List<AssociativeNode> arguments = new List<AssociativeNode> { AstFactory.BuildStringNode(EnumType.AssemblyQualifiedName), AstFactory.BuildStringNode(EnumValue) };
            AssociativeNode functionCall = AstFactory.BuildFunctionCall("BH.UI.Basilisk.Methods.Create", "BHoMEnum", arguments);
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }

        /*******************************************/

        protected override void SerializeCore(XmlElement element, SaveContext context)
        {
            base.SerializeCore(element, context);
            var xmlDoc = element.OwnerDocument;

            if (EnumType != null)
            {
                var typeName = xmlDoc.CreateElement("EnumType");
                typeName.SetAttribute("value", EnumType.AssemblyQualifiedName);
                element.AppendChild(typeName);

                var methodName = xmlDoc.CreateElement("EnumValue");
                methodName.SetAttribute("value", EnumValue);
                element.AppendChild(methodName);
            }
        }

        /*******************************************/

        protected override void DeserializeCore(XmlElement element, SaveContext context)
        {
            base.DeserializeCore(element, context);

            Type type = null;
            string methodName = "";
            List<Type> paramTypes = new List<Type>();

            foreach (XmlNode node in element.ChildNodes)
            {
                switch (node.Name)
                {
                    case "EnumType":
                        if (node.Attributes != null && node.Attributes["value"] != null && node.Attributes["value"].Value != null)
                            EnumType = Type.GetType(node.Attributes["value"].Value);
                        break;
                    case "EnumValue":
                        if (node.Attributes != null && node.Attributes["value"] != null)
                            EnumValue = node.Attributes["value"].Value;
                        break;
                }
            }
        }

        /***************************************************/
    }
}
