using Autodesk.Revit.DB;

namespace Revit
{
    /// <summary>
    /// A parameter definition
    /// </summary>
    public static class Definition
    {
        /// <summary>
        /// Gets definition name.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="Name">Definition name</returns>
        /// <search>
        /// Definition name, definition name
        /// </search>
        public static string Name(Autodesk.Revit.DB.Definition Definition)
        {
            return Definition.Name;
        }

        /// <summary>
        /// Gets definition unit type name.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="UnitType">Unit Type</returns>
        /// <search>
        /// Definition unit type name, definition parameter unit type name
        /// </search>
        public static string UnitType(Autodesk.Revit.DB.Definition Definition)
        {
            return LabelUtils.GetLabelFor(Definition.UnitType);
        }

        /// <summary>
        /// Gets parameter type name.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="ParameterType">Parameter Type</returns>
        /// <search>
        /// Definition parameter type name, definition parameter type name
        /// </search>
        public static string ParameterType(Autodesk.Revit.DB.Definition Definition)
        {
            return LabelUtils.GetLabelFor(Definition.ParameterType);
        }

        /// <summary>
        /// Gets definition GUID. Return null if definition is not external definition.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="GUID">External definition GUID</returns>
        /// <search>
        /// Definition GUID, definition guid
        /// </search>
        public static string GUID(Autodesk.Revit.DB.Definition Definition)
        {
            return (Definition as ExternalDefinition).GUID.ToString();
        }

        /// <summary>
        /// Gets definition Owner Group. Return null if definition is not external definition.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="OwnerGroup">Owner Group</returns>
        /// <search>
        /// Definition GUID, definition guid
        /// </search>
        public static string OwnerGroup(Autodesk.Revit.DB.Definition Definition)
        {
            return (Definition as ExternalDefinition).OwnerGroup.Name;
        }

        /// <summary>
        /// Checks if definition is external definition. Return false if definition is not external definition.
        /// </summary>
        /// <param name="Definition">Definition</param>
        /// <returns name="IsExternalDefinition">True value if definition is external definition</returns>
        /// <search>
        /// Is External Definition, is external definition
        /// </search>
        public static bool IsExternalDefinition(Autodesk.Revit.DB.Definition Definition)
        {
            return Definition is ExternalDefinition;
        }
    }
}
