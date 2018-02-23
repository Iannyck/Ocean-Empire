using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    public enum Type { Menu = 0, InGame = 1 }
    public Type type;

    public void OpenOptions()
    {
        switch (type)
        {
            case Type.Menu:
                MessagePopup.DisplayMessage("Not implemented");
                break;
            case Type.InGame:
                InGameOptions.OpenWindow();
                break;
        }
    }
}
