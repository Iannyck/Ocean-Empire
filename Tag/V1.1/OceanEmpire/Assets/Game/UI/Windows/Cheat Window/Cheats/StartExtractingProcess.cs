using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StartExtractingProcess : MonoBehaviour
{

    public void Init()
    {
        //#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            string dataToExtract = GetAllExtractableData();
            if (dataToExtract != null)
                GoogleReader.WriteData(dataToExtract);
        }
        catch (System.Exception e)
        {
            MessagePopup.DisplayMessage(e.Message);
        }
        //#endif
    }

    public static string GetAllExtractableData()
    {
        StringBuilder txt = new StringBuilder();
        txt.Append("ID,Name,Model,Support Gryo,Support Accelero\n")
            .Append(SystemInfo.deviceUniqueIdentifier).Append(',')
            .Append(SystemInfo.deviceName).Append(',')
            .Append(SystemInfo.deviceModel).Append(',')
            .Append(SystemInfo.supportsGyroscope).Append(',')
            .Append(SystemInfo.supportsAccelerometer).Append(',')
            .Append("\n\n")
            .Append(GoogleActivities.instance.GetAllData())
            .Append("\n\n")
            .Append(Logger.Instance.GetTotalLogWithHeader());
        return txt.ToString();
    }
}
