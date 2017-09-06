using System;
using UnityEngine;

namespace CCC.Math
{
    public static class LinAlg2D
    {
        /// <summary>
        /// Retourne le point le plus pres de 'distantPoint' sur la droite y = ax + b
        /// <para>
        /// ATTENTION: a != 0 a != infinity
        /// </para>
        /// </summary>
        public static Vector2 GetClosestPointOnLine(float a, float b, Vector2 distantPoint)
        {
            //Nous cherchons la droite opposé (y = x/a + c) qui croise perpendiculerement la premiere droite
            float c = distantPoint.y - distantPoint.x / a;

            float x = (c - b) / (a - (1 / a));
            float y = a * x + b;
            return new Vector2(x, y);
        }

        public static Vector2 GetClosestPointOnLine(Vector2 distantPoint, Vector2 v, Vector2 vPoint)
        {
            Vector2 s = ProjectVector(distantPoint - vPoint, v);
            return vPoint + s;
        }

        public static Vector2 ProjectVector(Vector2 a, Vector2 b)
        {
            float x = Vector2.Dot(a, b) / Vector2.Dot(b, b);
            return x * b;
        }
    }
}
