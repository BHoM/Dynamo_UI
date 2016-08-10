using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RevitServices.Persistence;

using Autodesk.Revit.DB;

namespace Revit
{
    /// <summary>
    /// A Revit shared parameter file
    /// </summary>
    public static class SharedParameterFile
    {
        /// <summary>
        /// Gets definition from shared parameter file.
        /// </summary>
        /// <param name="Name">Definition Name</param>
        /// <returns name="GetDefinition">Definition</returns>
        /// <search>
        /// Shared Parameter File, Get Definition, shared parameter file, get definition
        /// </search>
        public static Autodesk.Revit.DB.Definition GetDefinition(string Name)
        {
            if (DocumentManager.Instance.CurrentDBDocument != null && DocumentManager.Instance.CurrentUIDocument.Application != null && DocumentManager.Instance.CurrentUIDocument.Application.Application != null)
            {
                Autodesk.Revit.DB.DefinitionFile aDefintionFile = DocumentManager.Instance.CurrentUIDocument.Application.Application.OpenSharedParameterFile();
                if (aDefintionFile != null)
                {
                    List<Autodesk.Revit.DB.Definition> aDefinitions = new List<Autodesk.Revit.DB.Definition>();
                    foreach (DefinitionGroup aDefinitionGroup in aDefintionFile.Groups)
                        foreach (Autodesk.Revit.DB.Definition aDefinition in aDefinitionGroup.Definitions)
                            if (aDefinition.Name == Name)
                                return aDefinition;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Gets definitions from shared parameter file.
        /// </summary>
        /// <returns name="GetDefinitions">List of definitions</returns>
        /// <search>
        /// Shared Parameter File, Get Definitions, shared parameter file, get definitions
        /// </search>
        public static List<Autodesk.Revit.DB.Definition> GetDefinitions()
        {
            if (DocumentManager.Instance.CurrentDBDocument != null && DocumentManager.Instance.CurrentUIDocument.Application != null && DocumentManager.Instance.CurrentUIDocument.Application.Application != null)
            {
                Autodesk.Revit.DB.DefinitionFile aDefintionFile = DocumentManager.Instance.CurrentUIDocument.Application.Application.OpenSharedParameterFile();
                if(aDefintionFile != null)
                {
                    List<Autodesk.Revit.DB.Definition> aDefinitions = new List<Autodesk.Revit.DB.Definition>();
                    foreach (DefinitionGroup aDefinitionGroup in aDefintionFile.Groups)
                            aDefinitions.AddRange(aDefinitionGroup.Definitions.ToList());
                    return aDefinitions;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets definitions from shared parameter file.
        /// </summary>
        /// <param name="GroupName">Group Name</param>
        /// <returns name="GetDefinitions">List of definitions</returns>
        /// <search>
        /// Shared Parameter File, Get Definitions, shared parameter file, get definitions
        /// </search>
        public static List<Autodesk.Revit.DB.Definition> GetDefinitions(string GroupName)
        {
            if (DocumentManager.Instance.CurrentDBDocument != null && DocumentManager.Instance.CurrentUIDocument.Application != null && DocumentManager.Instance.CurrentUIDocument.Application.Application != null)
            {
                Autodesk.Revit.DB.DefinitionFile aDefintionFile = DocumentManager.Instance.CurrentUIDocument.Application.Application.OpenSharedParameterFile();
                if (aDefintionFile != null)
                    foreach (DefinitionGroup aDefinitionGroup in aDefintionFile.Groups)
                        if (aDefinitionGroup.Name == GroupName)
                            return aDefinitionGroup.Definitions.ToList();
            }
            return null;
        }

        /// <summary>
        /// Sets shared parameter file path.
        /// </summary>
        /// <param name="FileName">File Name</param>
        /// <returns name="SetSharedParameterFileName">Boolean Result</returns>
        /// <search>
        /// Shared Parameter File, Set shared parameter file path, shared parameter file, set shared parameter file path
        /// </search>
        private static bool Set(string FileName)
        {
            Autodesk.Revit.DB.Document aDocument = DocumentManager.Instance.CurrentDBDocument;
            if (DocumentManager.Instance.CurrentDBDocument != null && DocumentManager.Instance.CurrentUIDocument.Application != null && DocumentManager.Instance.CurrentUIDocument.Application.Application != null)
            {
                string aOldFileName = DocumentManager.Instance.CurrentUIDocument.Application.Application.SharedParametersFilename;
                DocumentManager.Instance.CurrentUIDocument.Application.Application.SharedParametersFilename = FileName;
                try
                {
                    if (DocumentManager.Instance.CurrentUIDocument.Application.Application.SharedParametersFilename == FileName)
                    {
                        DocumentManager.Instance.CurrentUIDocument.Application.Application.OpenSharedParameterFile();
                        return true;
                    }
                }
                catch
                {
                    DocumentManager.Instance.CurrentUIDocument.Application.Application.SharedParametersFilename = aOldFileName;
                }
            }
            return false;
        }
    }
}
