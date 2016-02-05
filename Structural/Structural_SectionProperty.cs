using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using Autodesk.DesignScript.Runtime;
using BHoM;

namespace Structural
{

    /// <summary>
    /// Structural tools
    /// BuroHappold
    /// <class name="BHGeometryTools">Geometry tools for Dynamo</class>
    /// </summary>
    public class SectionProperty
    {
        internal SectionProperty() { }
        /// <summary>
        /// Create a BHoM structural section property
        /// BuroHappold
        /// </summary>
        /// <param name="type"></param>
        /// <param name="number"></param>
        /// <param name="description"></param>
        /// <param name="D"></param>
        /// <param name="B"></param>
        /// <param name="tw"></param>
        /// <param name="tf"></param>
        /// <param name="zOffset"></param>
        /// <param name="cutbackS"></param>
        /// <param name="cutbackE"></param>
        /// <param name="taperPosition1"></param>
        /// <param name="taperPosition2"></param>
        /// <param name="tag1"></param>
        /// <param name="tag2"></param>
        /// <returns></returns>
        [RegisterForTrace]
        [MultiReturn(new[] { "SectionProperty", "Bool" })]
        public static Dictionary<string, object> ByType(
            string type,
            [DefaultArgument("0")] int number,
            [DefaultArgument("\"Unassigned\"")] string description,
            [DefaultArgument("0")] double D,
            [DefaultArgument("0")] double B,
            [DefaultArgument("0")] double tw,
            [DefaultArgument("0")] double tf,
            [DefaultArgument("0")] double zOffset,
            [DefaultArgument("0")] double cutbackS,
            [DefaultArgument("0")] double cutbackE,
            [DefaultArgument("-")] double taperPosition1,
            [DefaultArgument("-")] double taperPosition2,
            [DefaultArgument("\"Unassigned\"")] string tag1,
            [DefaultArgument("\"Unassigned\"")] string tag2)

        {
            Dictionary<string, object> secProp_out = new Dictionary<string, object>();
            int secProp_number = (number == 0) ? Structural.SectionPropertyManager.GetNextUnusedID() : number;
            BHoM.Structural.SectionProperty secProp = new BHoM.Structural.SectionProperty(type, number, description, D, B, tw, tf, zOffset, cutbackS, cutbackE, taperPosition1, taperPosition2, tag1, tag2);

            secProp_out.Add("SectionProperty", secProp);
            return secProp_out;
        }
        /// <summary>
        /// Deconstructs a BHoM Section Property
        /// BuroHappold
        /// </summary>
        /// <param name="secProp"></param>
        /// <returns></returns>
        /// <search>BH, sectionproperty, deconstruct</search>
        [RegisterForTrace]
        [MultiReturn(new[] { "Name", "Number", "Type", "Description", "FamilyTypeName", "D","B","tw","tf", "zOffset", "CutbackS", "CutbackE","TaperPosition1","TaperPosition2", "Tag1", "Tag2" })]
        public static Dictionary<string, object> Deconstruct(BHoM.Structural.SectionProperty secProp)
        {
            return new Dictionary<string, object>
            {
                {"Name", secProp.Name},
                {"Number", secProp.Index},
                {"Type", secProp.Type},
                {"Description", secProp.Description},
                {"FamilyTypeName", secProp.RevitFamilyTypeName},
                {"D", secProp.D},
                {"B", secProp.B},
                {"tw", secProp.tw},
                {"tf", secProp.tf},
                {"zOffset", secProp.ZOffsetS},
                {"CutbackS", secProp.CutbackS},
                {"CutbackE", secProp.CutbackE},
                {"TaperPosition1", secProp.TaperIntermediatePos[0]},
                {"TaperPosition2", secProp.TaperIntermediatePos[1]},
                {"Tag1", secProp.Tag1},
                {"Tag2", secProp.Tag2},
            };
        }
    }
}
