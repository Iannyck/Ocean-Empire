using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Manages an dynamic list of PalierContents
/// </summary>
public class PalierManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PalierPlans plans;

    [Header("Settings")]
    [Tooltip("Le nombre de palier actif sous la cible")]
    [SerializeField] private int activePalierRange = 5;
    [Tooltip("Le nombre de palier inactif en surplus (en bas)")]
    [SerializeField] private int inactivePalierExtraRange = 1;

    [Header("Runtime Values")]
    [SerializeField, ReadOnly] private List<Palier> Paliers = new List<Palier>();
    [SerializeField, ReadOnly] private int centerPalier = 0;


    public PalierPlans PalierPlans { get { return plans; } private set { plans = value; } }
    public ReadOnlyCollection<Palier> GetPalier() { return Paliers.AsReadOnly(); }
    public event Action<Palier> OnPalierActivated;

    private int lastMin = 0;
    private int lastMax = -1;

    void Awake()
    {
        // We do this to auto-generate paliers
        CenterPalier = CenterPalier;
    }

    void OnValidate()
    {
        if (activePalierRange < 0)
            activePalierRange = 0;
    }

    public Palier GetPalier(int index)
    {
        if (index >= Paliers.Count || index < 0)
            return null;
        return Paliers[index];
    }

    public int CenterPalier
    {
        get { return centerPalier; }
        set
        {
            centerPalier = value;
            int min = Mathf.Max(centerPalier - activePalierRange, 0);
            int max = centerPalier + activePalierRange;
            int maxWInactive = max + inactivePalierExtraRange;

            // Deactivate left (if necessary)
            for (int i = lastMin; i < min; i++)
            {
                Paliers[i].Deactivate();
            }
            // Deactivate right (if necessary)
            for (int i = lastMax; i > max; i--)
            {
                Paliers[i].Deactivate();
            }

            // Add paliers (if necessary)
            Palier newPalier = null;
            for (int i = Paliers.Count; i <= maxWInactive; i++)
            {
                newPalier = new Palier(i);
                Paliers.Add(newPalier);
            }

            // Activate left (if necessary)
            for (int i = min; i < lastMin; i++)
            {
                Paliers[i].Activate();
                if (OnPalierActivated != null)
                    OnPalierActivated(Paliers[i]);
            }
            // Activate right (if necessary)
            for (int i = max; i > lastMax; i--)
            {
                Paliers[i].Activate();
                if (OnPalierActivated != null)
                    OnPalierActivated(Paliers[i]);
            }

            lastMax = max;
            lastMin = min;
        }
    }

    void OnDrawGizmosSelected()
    {
        Color activeColor = new Color(1, 1, 0.5f, 0.5f);
        Color inactiveColor = new Color(0.25f, 0.25f, 0.25f, 0.35f);
        for (int i = 0; i < Paliers.Count; i++)
        {
            Gizmos.color = Paliers[i].IsActive ? activeColor : inactiveColor;
            plans.DrawPalierWGizmos(Paliers[i].Index);
        }
    }
}
