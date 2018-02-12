using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [ReadOnly]
    public Transform followAnchor;

    public Transform arrowHolder;
    public float maxLength = 4;

    private Transform tr;

    private void Awake()
    {
        tr = transform;
    }

    public void UpdatePosition(Vector2 position)
    {

        Vector2 delta = (Vector2)followAnchor.position - position;
        float angle = delta.ToAngle();
        float dist = delta.magnitude;

        if(dist > maxLength)
        {
            dist = maxLength;
            position = (Vector2)followAnchor.position - (delta.normalized * maxLength);
        }

        arrowHolder.localScale = new Vector3(ArrowXScale(dist), ArrowYScale(dist), 1);

        tr.rotation = Quaternion.Euler(0, 0, angle);
        tr.position = position;
    }

    private float ArrowXScale(float distance)
    {
        return (distance*1.75f + 0.5f - arrowHolder.localPosition.x).Raised(0);
    }
    private float ArrowYScale(float distance)
    {
        return (distance - arrowHolder.localPosition.x - 0.15f).Clamped(0, 1);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
