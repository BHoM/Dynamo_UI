using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Autodesk.DesignScript.Runtime;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Geometry;

namespace File
{
    /// <summary>
    /// Class containing functions to read/import data to Dynamo
    /// BuroHappold
    /// <class name="Read"></class>
    /// </summary>
    public class Read
    {
        internal Read() { }

        /// <summary>
        /// Reads in a list of files inside a specified folder, with specified file type and search string (if input)
        /// To return a specific file type use the syntax "*.xls"
        /// BuroHappold
        /// </summary>
        /// <param name="folderPath">String input for folder name/location</param>
        /// <param name="searchString">Optional string input for search. To return a specific file type use the syntax "*.xls"</param>
        /// <returns></returns>
        [MultiReturn(new[] { "FilePaths", "FileSize", "DateModified" })]
        public static Dictionary<string, object> GetFilePaths(String folderPath, string searchString = "")
        {
            string[] filePaths = Directory.GetFiles(folderPath, searchString, SearchOption.TopDirectoryOnly);
            List<string> fileSizes = new List<string>();
            List<string> dateModifieds = new List<string>();
            foreach(string filePath in filePaths)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                fileSizes.Add(fileInfo.Length.ToString());
                dateModifieds.Add(fileInfo.LastWriteTime.ToShortDateString());
            }

            //output
            return new Dictionary<string, object>
            {
                { "FilePaths", filePaths},
                { "FileSizes", fileSizes},
                { "DateModifieds", dateModifieds}
            };

        }
    }

}