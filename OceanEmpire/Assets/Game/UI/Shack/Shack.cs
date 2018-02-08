using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack : MonoBehaviour
{
    public const string SCENENAME = "Shack";

    public void OpenCalendar()
    {

        CalendarRootScene.OpenCalendar(
            () =>
            {
                //On load complete
            },
            () =>
            {
                //On entrance complete
                Scenes.UnloadAsync(gameObject.scene);
            });
    }
}
