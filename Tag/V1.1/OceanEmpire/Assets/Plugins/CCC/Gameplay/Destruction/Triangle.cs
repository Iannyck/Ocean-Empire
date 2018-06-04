using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Gameplay.Destruction
{
    public class Triangle
    {
        Vector3[] points;

        public Vector3 worldA { get { return points[0]; } }
        public Vector3 worldB { get { return points[1]; } }
        public Vector3 worldC { get { return points[2]; } }

        public Transform parent;

        public Triangle(Vector3 a, Vector3 b, Vector3 c, Transform parent)
        {
            a.Scale(parent.lossyScale);
            b.Scale(parent.lossyScale);
            c.Scale(parent.lossyScale);
            points = new Vector3[] {
            parent.position + (parent.rotation * a),
            parent.position + (parent.rotation * b),
            parent.position + (parent.rotation * c)
        };
            this.parent = parent;
        }

        public bool Intersects(Shape other)
        {
            for (int i = 0; i < other.triangles.Length; i++)
            {
                if (Intersects(other.triangles[i]))
                    return true;
            }
            return false;
        }

        public bool Intersects(Triangle other)
        {
            if (RayTriangle_Intersection(worldA, worldB, other))
                return true;
            if (RayTriangle_Intersection(worldA, worldC, other))
                return true;
            if (RayTriangle_Intersection(worldB, worldC, other))
                return true;
            if (other.ReversedIntersect(this))
                return true;

            return false;
        }

        private bool ReversedIntersect(Triangle other)
        {
            if (RayTriangle_Intersection(worldA, worldB, other))
                return true;
            if (RayTriangle_Intersection(worldA, worldC, other))
                return true;
            if (RayTriangle_Intersection(worldB, worldC, other))
                return true;

            return false;
        }

        // ref: https://courses.cs.washington.edu/courses/cse457/07sp/lectures/triangle_intersection.pdf
        private static bool RayTriangle_Intersection(Vector3 p0, Vector3 p1, Triangle triangle)
        {
            Vector3 u, v, n;
            Vector3 worldA = triangle.worldA, worldB = triangle.worldB, worldC = triangle.worldC;
            float d;
            Vector3 dir;

            dir = (p1 - p0).normalized;              // ray direction vector

            u = worldB - worldA;            // v2 du triangle
            v = worldC - worldA;            // v1 du triangle
            n = Vector3.Cross(u, v);        // normale
            d = Vector3.Dot(worldA, n);      // d du plan du triangle 

            Vector3 point;

            if (Mathf.Abs(Vector3.Dot(n, dir)) < 0.03)    //Parallèle
            {
                Vector3 p0_WorldA = p0 - worldA;
                Vector3 p0p_WorldA = Vector3.ProjectOnPlane(p0_WorldA, n);
                // if (Vector3.Dot(dir, (p0 - worldA).normalized) < 0.99) //parallèle, mais loin
                if (p0p_WorldA.magnitude > 0.05)
                    return false;

                point = p0;
            }
            else                                            //Non parallèle
            {
                float t = (d - Vector3.Dot(n, p0)) / Vector3.Dot(n, dir);

                if (t < 0 || t > (p1 - p0).magnitude)        //n'atteint meme pas le plane
                    return false;

                point = p0 + (dir * t);
            }

            Vector3 w, qa, qb;
            w = worldC - worldB;
            qa = point - worldA;
            qb = point - worldB;

            if (Vector3.Dot(Vector3.Cross(u, qa), n) < 0)
                return false;
            if (Vector3.Dot(Vector3.Cross(w, qb), n) < 0)
                return false;
            if (Vector3.Dot(Vector3.Cross(qa, v), n) < 0)
                return false;

            return true;
        }

        public void Draw()
        {
            Draw(Color.black);
        }
        public void Draw(Color color)
        {
            Debug.DrawLine(worldA, worldB, color);
            Debug.DrawLine(worldA, worldC, color);
            Debug.DrawLine(worldB, worldC, color);
        }
    }
}