using UnityEngine;
using CCC.UI.Animation;

public class AlexTestScript : MonoBehaviour {

    public Popup popup;
    public Sprite image;
    public string text = "LOL";
    public bool useOutLine = true;
    public Vector2 anchorMoveDelta = new Vector2(0, -200);
    public Vector3 imageScale = new Vector3(0.5f,0.5f,0.5f);
    public int textSize;
    public Color color;
    public Font font;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Popup.CreatePopup(popup,transform).ChangeAnimationSettings(anchorMoveDelta).ChangeImageSettings(imageScale, color).Animate(gameObject,image);
        else if (Input.GetKeyDown(KeyCode.Y))
            Popup.CreatePopup(popup, transform).ChangeAnimationSettings(anchorMoveDelta).ChangeTextSettings(textSize, color, font).Animate(gameObject, text, useOutLine);
    }
}
