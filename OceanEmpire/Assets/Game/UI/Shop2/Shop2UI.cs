using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop2UI : MonoBehaviour
{
    public void QuitShop()
    {
        Scenes.UnloadAsync(gameObject.scene);
    }
}
