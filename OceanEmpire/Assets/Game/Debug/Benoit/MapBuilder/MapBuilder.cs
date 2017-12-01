using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FullInspector;

public class MapBuilder : BaseBehavior {

    public SpriteRenderer WaterTop;
    public SpriteRenderer waterLayer;
    public SpriteRenderer Bottom;

    public Color PalestColor;
    public Color DarkestColor;

    public int amountOfTints;
    [SerializeField, ReadOnly]
    private List<Color> tints;
    private bool colorPrimed = false;

    public float StartPosition;
    public float FinishPostion;

    public float VerticalOffset;
    public float maxLateralOffset;

    private List<SpriteRenderer> BackgroundWater;
    public Transform parent;

#if UNITY_EDITOR
    [InspectorButton()]
    public void BuildWater()
    {
        if (BackgroundWater != null && BackgroundWater.Count != 0)
            ClearWater();
        BackgroundWater = new List<SpriteRenderer>();
        PrimeColors();

        int numberOfLayer = Mathf.CeilToInt((StartPosition - FinishPostion ) / VerticalOffset);
        int layerPerTint = Mathf.CeilToInt((float)(numberOfLayer) / (amountOfTints - 1));
        if (layerPerTint % 2 == 0)
            layerPerTint++;

        int LayerInTintCoutner = 0;
        int currentTint = 0;

        SpawnTop();

        for (int i = 1; i < numberOfLayer; ++i)
        {

            float x = GetLateralOffset();
            float y = StartPosition - i * VerticalOffset;
            Vector2 position = new Vector2(x, y);

            SpriteRenderer newLayer = Instantiate(waterLayer.gameObject, position, Quaternion.identity, parent).GetComponent<SpriteRenderer>();
            // int tintIterator = ( ((StartPosition - y) / (StartPosition - FinishPostion)) * (amountOfTints - 1) + (i+1) % 2).RoundedToInt().Capped(amountOfTints -1);

            
            if (LayerInTintCoutner >= layerPerTint)
            {
                LayerInTintCoutner = 0;
                currentTint++;
            }
            LayerInTintCoutner++;
            int tint = currentTint + LayerInTintCoutner % 2;

            newLayer.color = tints[tint];
            newLayer.sortingOrder = i + 10;

            BackgroundWater.Add(newLayer);

        }

        SpawnBot();

    }



    [InspectorButton()]
    public void ClearWater()
    {
        if (BackgroundWater == null)
            return;
        for (int i = 0; i < BackgroundWater.Count; ++i)
        {
            if(BackgroundWater[i] != null)
                DestroyImmediate(BackgroundWater[i].gameObject);
        }

        BackgroundWater.Clear();
    }

#endif

    private float GetLateralOffset()
    {
        return Random.Range(-maxLateralOffset, maxLateralOffset);
    }

    private void PrimeColors()
    {
        if (tints != null)
            tints.Clear();

        tints = new List<Color>();

        for (int i = 0; i < amountOfTints; ++i)
        {
            float lerpValue = (float)i / (amountOfTints - 1);
            print(lerpValue.ToString());
            Color newColot = Color.Lerp(PalestColor, DarkestColor, lerpValue);
            tints.Add(newColot);
        }
    }

    private void SpawnTop()
    {
        Vector2 position = new Vector2(0, StartPosition);
        SpriteRenderer newLayer = Instantiate(WaterTop.gameObject, position, Quaternion.identity, parent).GetComponent<SpriteRenderer>();
        newLayer.color = tints[0];
        newLayer.sortingOrder = -10;
        BackgroundWater.Add(newLayer);
    }

    private void SpawnBot()
    {
        Vector2 position = new Vector2(0, FinishPostion);
          SpriteRenderer newLayer = Instantiate(Bottom.gameObject, position, Quaternion.identity, parent).GetComponent<SpriteRenderer>();
        newLayer.sortingOrder = Mathf.CeilToInt((StartPosition - FinishPostion) / VerticalOffset) + 10;
        BackgroundWater.Add(newLayer);
    }



}
