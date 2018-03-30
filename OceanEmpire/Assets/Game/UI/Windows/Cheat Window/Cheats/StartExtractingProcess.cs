using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartExtractingProcess : MonoBehaviour {

	public void Init()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string[] dataToExtract = GoogleActivities.instance.GetAllData();
        if (dataToExtract != null)
            GoogleReader.WriteData(dataToExtract);
#endif
    }
}
