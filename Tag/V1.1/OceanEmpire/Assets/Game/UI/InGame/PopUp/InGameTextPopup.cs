using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameTextPopup : MonoBehaviour
{
    [SerializeField]
    private InGameTextPopup_Item popupPrefab;

    [SerializeField]
    private RectTransform parent;

    public void SpawnText(string message, Color textColor, Vector2 worldPosition)
    {
        InGameTextPopup_Item newPopup = popupPrefab.DuplicateGO(parent);
        newPopup.Setup(message, textColor, worldPosition);
    }
}
