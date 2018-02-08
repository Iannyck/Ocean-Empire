using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    [SerializeField] Camera shackCamera;

    public void OpenCalendar()
    {

        CalendarRootScene.OpenCalendar(
            () =>
            {
                //On load complete
                shackCamera.gameObject.SetActive(false);
            },
            () =>
            {
                //On entrance complete
                Scenes.UnloadAsync(gameObject.scene);
            });
    }
}
