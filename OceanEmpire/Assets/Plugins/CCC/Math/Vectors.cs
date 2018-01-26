using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CCC.Math
{
    public static class Vectors
    {
        public static Vector2 RandomVector2(float minLength, float maxLength, float minAngle, float maxAngle)
        {
            float angle = Random.Range(minAngle, maxAngle);
            float length = Random.Range(minLength, maxLength);

            return angle.ToVector() * length;
        }

        public static Vector2 RandomVector2(float minLength, float maxLength)
        {
            return RandomVector2(minLength, maxLength, 0, 360);
        }
    }
}
