using UnityEngine;

public static class VectorExtensions
{
    public static float ToAngle(this Vector2 v)
    {
        return CCC.Math.Vectors.VectorToAngle(v);
    }
    public static Vector2 Clamped(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y));
    }
    public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z));
    }
    public static Vector2 Rounded(this Vector2 v)
    {
        return new Vector2(v.x.Rounded(), v.y.Rounded());
    }
    public static Vector2 Rotate(this Vector2 v, float angle)
    {
        return v.RotateRad(angle * Mathf.Deg2Rad);
    }
    public static Vector2 RotateRad(this Vector2 v, float radians)
    {
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public static Vector2 FlippedX(this Vector2 v)
    {
        return new Vector2(-v.x,v.y);
    }

    public static Vector2 FlippedY(this Vector2 v)
    {
        return new Vector2(v.x, -v.y);
    }

    public static Vector2 MovedTowards(this Vector2 v, Vector2 target, float maxDistanceDelta)
    {
        return Vector2.MoveTowards(v, target, maxDistanceDelta);
    }
    public static Vector3 MovedTowards(this Vector3 v, Vector3 target, float maxDistanceDelta)
    {
        return Vector3.MoveTowards(v, target, maxDistanceDelta);
    }

    /// <summary>
    /// Multiplie les parametre par ceux du 'scale'
    /// </summary>
    public static Vector2 Scaled(this Vector2 v, Vector2 scale)
    {
        return new Vector2(v.x * scale.x, v.y * scale.y);
    }

    /// <summary>
    /// Divise les parametre par ceux du 'invertedScale'
    /// </summary>
    public static Vector2 ScaledInv(this Vector2 v, Vector2 invertedScale)
    {
        return new Vector2(v.x / invertedScale.x, v.y / invertedScale.y);
    }
}
