using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    public bool facingRight;
    public Rigidbody2D rb;

    private SpriteRenderer sprRenderer;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(rb != null)
        {

        }
    }
}
