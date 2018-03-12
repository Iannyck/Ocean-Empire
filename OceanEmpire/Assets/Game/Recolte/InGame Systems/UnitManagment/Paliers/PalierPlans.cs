using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalierPlans : MonoBehaviour
{
    [SerializeField] private float _palierThickness = 2;
    [SerializeField] private float _top = 0;
    public float PalierThickness { get { return _palierThickness; } private set { _palierThickness = value; } }
    public float Top { get { return _top; } set { _top = value; } }

    [SerializeField, Header("Gizmos")] private bool drawPaliers = true;
    [SerializeField] private int drawCount = 20;


    public int GetClosestPalier(float height)
    {
        height = Mathf.Clamp(height - Top, float.NegativeInfinity, 0);
        return -Mathf.CeilToInt(height / _palierThickness);
    }
    public float GetPalierCenter(int index)
    {
        return GetPalierTop(index) - (0.5f * _palierThickness);
    }
    public float GetPalierBottom(int index)
    {
        return GetPalierTop(index + 1);
    }
    public float GetPalierTop(int index)
    {
        return -index * _palierThickness + Top;
    }

    #region Gizmos
    void OnDrawGizmosSelected()
    {
        if (drawPaliers)
        {
            Gizmos.color = new Color(1, .5f, 1, .5f);

            for (int i = 0; i < drawCount; i++)
            {
                DrawPalierWGizmos(i);
            }
        }
    }

    public void DrawPalierWGizmos(int index)
    {
        Vector3 center = new Vector3(0, GetPalierCenter(index), 0);
        Vector3 size = new Vector3(5, PalierThickness - 0.1f, 0.1f);
        Gizmos.DrawCube(center, size);
    }
    #endregion
}
