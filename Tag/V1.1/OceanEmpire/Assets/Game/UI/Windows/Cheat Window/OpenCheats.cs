using UnityEngine;
using UnityEngine.SceneManagement;


public class OpenCheats : LoadScene
{
    private void OnDrawGizmos()
    {
        RectTransform rt = transform as RectTransform;
        if (rt == null)
            return;

        Gizmos.color = Color.magenta;
        var rect = rt.rect;
        var wasMatrix = Gizmos.matrix;
        Gizmos.matrix = rt.localToWorldMatrix;
        Gizmos.DrawCube(rect.center, rect.size);

        Gizmos.matrix = wasMatrix;
    }
}
