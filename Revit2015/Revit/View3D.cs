using Revit.Elements;
using RevitServices.Transactions;
using Revit.GeometryConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit
{
    /// <summary>
    /// A View3D
    /// </summary>
    public static class View3D
    {
        /// <summary>
        /// Creates perspective view
        /// </summary>
        /// <param name="Name">Name</param>
        /// <param name="ViewFamilyType">View Family Type</param>
        /// <param name="EyePosition">Eye Position</param>
        /// <param name="Up">Up Direction</param>
        /// <param name="Forward">Forward Direction</param>
        /// <returns name="Elements">Elements</returns>
        /// <search>
        /// View Elements, View, CreatePerspective, 3D View, 3dview, create perspective
        /// </search>
        public static Elements.Views.View3D CreatePerspective(string Name, Elements.Element ViewFamilyType, Autodesk.DesignScript.Geometry.Point EyePosition, Autodesk.DesignScript.Geometry.Vector Up, Autodesk.DesignScript.Geometry.Vector Forward)
        {
            Autodesk.Revit.DB.XYZ aEyePosition = new Autodesk.Revit.DB.XYZ(EyePosition.X, EyePosition.Y, EyePosition.Z);
            Autodesk.Revit.DB.XYZ aUp = new Autodesk.Revit.DB.XYZ(Up.X, Up.Y, Up.Z);
            Autodesk.Revit.DB.XYZ aForward = new Autodesk.Revit.DB.XYZ(Forward.X, Forward.Y, Forward.Z);

            Autodesk.Revit.DB.Document aDocument = ViewFamilyType.InternalElement.Document;
            TransactionManager.Instance.EnsureInTransaction(aDocument);
            Autodesk.Revit.DB.View3D aView3D = Autodesk.Revit.DB.View3D.CreatePerspective(aDocument, ViewFamilyType.InternalElement.Id);
            aView3D.Name = Name;
            Autodesk.Revit.DB.ViewOrientation3D aViewOrientation3D = new Autodesk.Revit.DB.ViewOrientation3D(aEyePosition, aUp, aForward);
            TransactionManager.Instance.TransactionTaskDone();

            return ElementWrapper.Wrap(aView3D, true);
        }

        /// <summary>
        /// Sets 3Dview section box
        /// </summary>
        /// <param name="View3D">View 3D</param>
        /// <param name="BoundingBox">Section Bounding Box</param>
        /// <returns name="View3D">View 3D</returns>
        /// <search>
        /// View Elements, View, SetSectionBox, 3D View, Set Section Box
        /// </search>
        public static Elements.Views.View3D SetSectionBox(Elements.Views.View3D View3D, Autodesk.DesignScript.Geometry.BoundingBox BoundingBox)
        {
            Autodesk.Revit.DB.BoundingBoxXYZ aBoundingBox = BoundingBox.ToRevitType(false);
            Autodesk.Revit.DB.View3D aView3D = View3D.InternalElement as Autodesk.Revit.DB.View3D;
            TransactionManager.Instance.EnsureInTransaction(View3D.InternalElement.Document);
            aView3D.SetSectionBox(aBoundingBox);
            Autodesk.Revit.DB.Parameter aParameter = aView3D.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.VIEWER_MODEL_CLIP_BOX_ACTIVE);
            aParameter.Set(1);
            TransactionManager.Instance.TransactionTaskDone();
            return View3D;
        }

        /// <summary>
        /// Sets 3Dview section box
        /// </summary>
        /// <param name="View3D">View 3D</param>
        /// <param name="Visible">Visible</param>
        /// <returns name="View3D">View 3D</returns>
        /// <search>
        /// View Elements, View, SetSectionBox, 3D View, Set Section Box
        /// </search>
        public static Elements.Views.View3D SetSectionBox(Elements.Views.View3D View3D, bool Visible)
        {
            TransactionManager.Instance.EnsureInTransaction(View3D.InternalElement.Document);
            Autodesk.Revit.DB.Parameter aParameter = View3D.InternalElement.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.VIEWER_MODEL_CLIP_BOX_ACTIVE);
            if (Visible)
                aParameter.Set(1);
            else
                aParameter.Set(0);
            TransactionManager.Instance.TransactionTaskDone();
            return View3D;
        }
    }
}
