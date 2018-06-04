using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.UI;

public class DisplayActivities : MonoBehaviour {

    public WindowAnimation anim;
    public Text display;

	public void Display()
    {
        anim.Open(delegate ()
        {
            display.text = "Check console adb logcat -s Unity";
            string originalText = "Hello World";
            Debug.Log("------------------");
            Debug.Log("Original : " + originalText);
            string cryptedText = GoogleReader.Encrypt(originalText);
            Debug.Log("Crypted : " + cryptedText);
            Debug.Log("Decrypted : " + GoogleReader.Decrypt(cryptedText));
            Debug.Log("------------------");
            GoogleReader.LoadRawDocument(delegate (string rawDocument)
            {
                Debug.Log(rawDocument);
                GoogleReader.LoadDocument(delegate (string document)
                {
                    Debug.Log(document);
                    Debug.Log("------------------");
                });
            });

            MessagePopup.DisplayMessage("Showing The Activities");
            string allActivities = "";
            List<GoogleActivities.ActivityReport> records = GoogleActivities.instance.records;
            Debug.Log("UNITY RECORDS COUNT : " + records.Count);
            for (int i = 0; i < records.Count; i++)
            {
                allActivities += records[i].best.type;
                allActivities += "|";
                allActivities += records[i].best.rate;
                allActivities += "|";
                allActivities += records[i].time;
                allActivities += "\n";
            }
            display.text = allActivities;
        });
    }

    public void Close()
    {
        anim.Close();
    }
}
