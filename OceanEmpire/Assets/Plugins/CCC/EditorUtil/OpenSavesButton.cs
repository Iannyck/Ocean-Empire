using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using System.IO;
using System.Diagnostics;

public class OpenSavesButton
{
    [InspectorButton()]
    void OpenSaveLocation()
    {
        if (!Application.isEditor)
            return;

        string path = Application.persistentDataPath.Replace('/', '\\');

        if (Directory.Exists(path))
        {
            Process.Start("explorer.exe", path);
        }
    }
}
