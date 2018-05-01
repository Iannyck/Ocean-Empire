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
        if (isLoading || categoryDisplay.category.GetNextUpgradeDescription() == null)
            return;

        isLoading = true;
        Scenes.Load(upgradeWindowScene, (scene) =>
        {
            //...
            scene.FindRootObject<UpgradeWindow>().FillContent(
                categoryDisplay.category,
                categoryDisplay.image.sprite,
                categoryDisplay.UpdateDefaultContent);
            isLoading = false;
        });
    }
}
