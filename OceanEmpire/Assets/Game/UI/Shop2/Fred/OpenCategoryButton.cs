using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCategoryButton : MonoBehaviour
{
    public UIUpgradeDisplay categoryDisplay;
    public SceneInfo upgradeWindowScene;

    private bool isLoading = false;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenItem);
    }

    public void OpenItem()
    {
        if (isLoading)
            return;

        isLoading = true;
        Scenes.Load(upgradeWindowScene, (scene) =>
        {
            //...
            Debug.Log("Fill window content");
            isLoading = false;
        });
    }
}
