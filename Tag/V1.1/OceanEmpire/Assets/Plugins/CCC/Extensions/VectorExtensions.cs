using UnityEngine;

public static class VectorExtensions
{
    public static float ToAngle(this Vector2 v)
    {
        if (v.x < 0)
            return Mathf.Atan(v.y / v.x) * Mathf.Rad2Deg + 180;
        return Mathf.Atan(v.y / v.x) * Mathf.Rad2Deg;
    }
    public static Vector2 SwapXAndY(this Vector2 v)
    {
        float wasX = v.x;
        v.x = v.y;
        v.y = wasX;
        return v;
    }
    public static Vector2 Clamped(this Vector2 v, Vector2 min, Vector2 max)
    {
        return new Vector2(Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y));
    }
    public static Vector2 Capped(this Vector2 v, Vector2 max)
    {
        return new Vector2(v.x.Capped(max.x), v.y.Capped(max.y));
    }
    public static Vector2 Raised(this Vector2 v, Vector2 min)
    {
        return new Vector2(v.x.Raised(min.x), v.y.Raised(min.y));
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

    public static Vector2 MovedTowards(this Vector2 v, Vector2 target, float maxDistanceDelta)
    {
        return Vector2.MoveTowards(v, target, maxDistanceDelta);
    }
    public static Vector3 MovedTowards(this Vector3 v, Vector3 target, float maxDistanceDelta)
    {
        return Vector3.MoveTowards(v, target, maxDistanceDelta);
    }

    public static Quaternion ToEulerRotation(this Vector3 v)
    {
        return Quaternion.Euler(v);
    }

    public static Vector2 Lerpped(this Vector2 v, Vector2 target, float t)
    {
        return Vector2.Lerp(v, target, t);
    }
    public static Vector3 Lerpped(this Vector3 v, Vector3 target, float t)
    {
        return Vector3.Lerp(v, target, t);
    }

    /// <summary>
    /// Multiplie les parametre par ceux du 'scale'
    /// </summary>
    public static Vector2 Scaled(this Vector2 v, Vector2 scale)
    {
        v.x *= scale.x;
        v.y *= scale.y;
        return v;
    }

    /// <summary>
    /// Divise les parametre par ceux du 'invertedScale'
    /// </summary>
    public static Vector2 ScaledInv(this Vector2 v, Vector2 invertedScale)
    {
        v.x /= invertedScale.x;
        v.y /= invertedScale.y;
        return v;
    }
    /// <summary>
    /// Multiplie les parametre par ceux du 'scale'
    /// </summary>
    public static Vector3 Scaled(this Vector3 v, Vector3 scale)
    {
        v.x *= scale.x;
        v.y *= scale.y;
        v.z *= scale.z;
        return v;
    }

    /// <summary>
    /// Divise les parametre par ceux du 'invertedScale'
    /// </summary>
    public static Vector3 ScaledInv(this Vector3 v, Vector3 invertedScale)
    {
        v.x /= invertedScale.x;
        v.y /= invertedScale.y;
        v.z /= invertedScale.z;
        return v;
    }

    public static Vector2 Abs(this Vector2 v)
    {
        v.x = v.x.Abs();
        v.y = v.y.Abs();
        return v;
    }

    public static Vector3 Abs(this Vector3 v)
    {
        v.x = v.x.Abs();
        v.y = v.y.Abs();
        v.z = v.z.Abs();
        return v;
    }
}
