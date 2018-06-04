using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Gameplay.Destruction
{
    [System.Serializable]
    public class Shape
    {
        [System.NonSerialized]
        public Triangle[] triangles;
        public Transform tr;
        [System.NonSerialized]
        private Bounds bounds;

        public Shape(Mesh mesh, MeshRenderer meshRenderer, Transform parent)
        {
            if (mesh.triangles.Length % 3 != 0)
            {
                Debug.LogError("Cannot building shape with uneven number of triangle points (should be a multiple of 3).");
                return;
            }

            bounds = meshRenderer.bounds;
            this.tr = parent;

            triangles = new Triangle[mesh.triangles.Length / 3];
            for (int i = 0; i < mesh.triangles.Length; i += 3)
            {
                triangles[i / 3] = new Triangle(mesh.vertices[mesh.triangles[i]], mesh.vertices[mesh.triangles[i + 1]], mesh.vertices[mesh.triangles[i + 2]], parent);
            }
        }

        public bool Intersects(Shape other, bool draw = false)
        {
            bounds.center = tr.position;
            other.bounds.center = other.tr.position;

            if (!bounds.Intersects(other.bounds))   //Les bounds ne se touche pas, on ne fait aucun calcul
            {
                if (draw)
                    DrawTriangles();
                return false;
            }

            bool intersect = false;
            foreach (Triangle triangle in triangles)
            {
                if (triangle.Intersects(other))
                {
                    intersect = true;
                    if (draw)
                        triangle.Draw(Color.red);
                }
                else
                {
                    if (draw)
                        triangle.Draw(Color.black);
                }
            }
            return intersect;
        }

        public void DrawTriangles()
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.Draw();
            }
        }
        public void DrawTriangles(Color color)
        {
            foreach (Triangle triangle in triangles)
            {
                triangle.Draw(color);
            }
        }
        public void DrawTriangles(int triangleIndex)
        {
            triangles[triangleIndex].Draw();
        }
        public void DrawTriangles(int triangleIndex, Color color)
        {
            triangles[triangleIndex].Draw(color);
        }
    }
}