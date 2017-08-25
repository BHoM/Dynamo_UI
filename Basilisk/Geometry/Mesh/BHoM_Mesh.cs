using System;
using System.Collections.Generic;
using System.Linq;
using DSG = Autodesk.DesignScript.Geometry;
using BHG = BH.oM.Geometry;

namespace Geometry
{
    public static class BHMesh
    {
        public static BHG.Mesh FromDSMesh(DSG.Mesh Mesh)
        {           
            List<BHG.Point> vertices = Mesh.VertexPositions.Select(x =>  BHPoint.FromDSPoint(x)).ToList();   

            List<BHG.Face> faces = new List<BHG.Face>();
            for (int i = 0; i < Mesh.FaceIndices.Count(); i++)
            {
                BHG.Face face;

                if (Mesh.FaceIndices[i].Count == 4)
                    face = new BHG.Face((int)(Mesh.FaceIndices[i].A), (int)(Mesh.FaceIndices[i].B), (int)(Mesh.FaceIndices[i].C), (int)(Mesh.FaceIndices[i].D));                 
                else
                    face = new BHG.Face((int)(Mesh.FaceIndices[i].A), (int)(Mesh.FaceIndices[i].B), (int)(Mesh.FaceIndices[i].C));

                faces.Add(face);
            }
            
            return new BHG.Mesh(vertices,faces);
        }
    }
}
