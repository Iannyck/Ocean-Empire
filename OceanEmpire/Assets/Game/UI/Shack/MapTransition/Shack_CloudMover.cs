using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_CloudMover : MonoBehaviour
{
    public float horizontalSpeed;
    public Transform[] clouds;
    public float minX;
    public float maxX;

    private void Update()
    {
        if(horizontalSpeed > 0)
        {
            Vector3 pos ;
            for (int i = 0; i < clouds.Length; i++)
            {
                pos = clouds[i].position;

                // Move
                pos += Vector3.right * horizontalSpeed * clouds[i].localScale.y;

                // Wrap!
                if (pos.x > maxX)
                    pos.x -= maxX - minX;
                else if (pos.x < minX)
                    pos.x += maxX - minX;

                clouds[i].position = pos;
            }
        }
    }
}
