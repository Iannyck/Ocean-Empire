using System;
using System.Collections.Generic;
using UnityEngine;

public class PalierContentManager : MonoBehaviour
{
    [Tooltip("Le nombre de palier actif sous la cible"), SerializeField]
    private int _activePalierRange = 5;
    [SerializeField] private PalierPlans _plans;

    [SerializeField] private List<PalierContent> Contents = new List<PalierContent>();
    private int _centerPalier = 0;

    public PalierPlans PalierPlans { get { return _plans; } private set { _plans = value; } }
    public event Action<PalierContent> OnNewPalierActivated;



    public PalierContent GetPalierContent(int index)
    {
        for (int i = 0; i < Contents.Count; i++)
        {
            if (Contents[i].Index == index)
                return Contents[i];
        }
        return null;
    }

    public int CenterPalier
    {
        get { return _centerPalier; }
        set
        {
            _centerPalier = value;
            int min = _centerPalier - _activePalierRange;
            int max = _centerPalier + _activePalierRange;

            // Trim excess
            for (int i = Contents.Count - 1; i >= 0; i--)
            {
                if (Contents[i].Index < min || Contents[i].Index > max)
                {
                    Contents.RemoveAt(i);
                }
            }

            int leftmostBound = Contents.Count > 0 ? Contents[0].Index : min;
            int rightmostBound = Contents.Count > 0 ? Contents.Last().Index : min;

            int addToLeft = leftmostBound - min;
            int addToRight = max - rightmostBound;
            // Add left
            for (int i = 1; i <= addToLeft; i++)
            {
                Contents.Insert(0, new PalierContent(leftmostBound - i));
            }

            // Add right
            for (int i = 1; i <= addToRight; i++)
            {
                Contents.Add(new PalierContent(rightmostBound + i));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0.5f, 0.5f);
        for (int i = 0; i < Contents.Count; i++)
        {
            _plans.DrawPalierWGizmos(Contents[i].Index);
        }
    }
}
