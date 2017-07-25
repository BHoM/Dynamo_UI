using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BHoM.Geometry;

namespace Geometry
{
    public static class BHMesh
    {
        public static BHG.Mesh FromDSMesh(DSG.Mesh Mesh)
        {           
            List<DSG.Point> dsPts = Mesh.VertexPositions.ToList();
            List<BHG.Point> ptList = new List<BHG.Point>();

            foreach (DSG.Point dsPt in dsPts)
            {
                BHG.Point pt = BHPoint.FromDSPoint(dsPt);
                ptList.Add(pt);
            }
            BHG.Group<BHG.Point> vertices = new BHG.Group<BHG.Point>(ptList);

            List<BHG.Face> faces = new List<BHG.Face>();
            for (int i = 0; i < Mesh.FaceIndices.Count(); i++)
            {
                int[] arr;

                if (Mesh.FaceIndices[i].Count == 4)
                {
                    arr = new int[4] { (int)(Mesh.FaceIndices[i].A), (int)(Mesh.FaceIndices[i].B), (int)(Mesh.FaceIndices[i].C), (int)(Mesh.FaceIndices[i].D) };                   
                }
                else
                {
                    arr = new int[3] { (int)(Mesh.FaceIndices[i].A), (int)(Mesh.FaceIndices[i].B), (int)(Mesh.FaceIndices[i].C) };
                }
                BHG.Face face = new BHG.Face(arr);
                faces.Add(face);
            }
            
            return new BHG.Mesh(vertices,faces);
        }+
    }
}
