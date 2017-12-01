using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CCC.Math
{
    public static class Vectors
    {
        public static Vector2 AngleToVector(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
        public static float VectorToAngle(Vector2 v)
        {
            if (v.x < 0)
                return Mathf.Atan(v.y / v.x) * Mathf.Rad2Deg + 180;
            return Mathf.Atan(v.y / v.x) * Mathf.Rad2Deg;
        }

        public static Vector2 RandomVector2(float minAngle, float maxAngle, float minLength, float maxLength)
        {
            float angle = Random.Range(minAngle, maxAngle);
            float length = Random.Range(minLength, maxLength);

            return AngleToVector(angle) * length;
        }

        public static Vector2 RandomVector2(float minLength, float maxLength)
        {
            float angle = Random.Range(0, 360);
            float length = Random.Range(minLength, maxLength);

            return AngleToVector(angle) * length;
        }
    }
}
