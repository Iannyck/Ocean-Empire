using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidGyro {

    public static void Activate()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        Input.gyro.enabled = true;
        #endif
    }

    public static void DeActivate()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        Input.gyro.enabled = false;
        #endif
    }

    public static Vector3 Accelerometer()
    {
        if (!Input.gyro.enabled)
            return Vector3.zero;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.acceleration;
        #else
        return Vector3.zero;
        #endif
    }

    public static Vector3 Acceleration()
    {
        if(!Input.gyro.enabled)
            return Vector3.zero;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.gyro.userAcceleration;
        #else
        return Vector3.zero;
        #endif
    }

    public static Quaternion Rotation()
    {
        if (!Input.gyro.enabled)
            return Quaternion.identity;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.gyro.attitude;
        #else
        return Quaternion.identity;
        #endif
    }

    public static Vector3 RotationRate()
    {
        if (!Input.gyro.enabled)
            return Vector3.zero;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.gyro.rotationRate;
        #else
        return Vector3.zero;
        #endif
    }

    public static Vector3 RotationRateUnbiased()
    {
        if (!Input.gyro.enabled)
            return Vector3.zero;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.gyro.rotationRateUnbiased;
        #else
        return Vector3.zero;
        #endif
    }

    public static float Interval()
    {
        if (!Input.gyro.enabled)
            return 0;
        #if UNITY_ANDROID && !UNITY_EDITOR
        return Input.gyro.updateInterval;
        #else
        return 0;
        #endif
    }

    // Ajouter des calculs potentiels ici
}
