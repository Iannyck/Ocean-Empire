using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDetection : MonoBehaviour {

    public class Activity
    {
        public enum ActivityType
        {
            Walking = 0
        }

        public int probability;
        public DateTime time;
        public ActivityType type;

        public Activity(int probability,DateTime time,ActivityType type = ActivityType.Walking)
        {
            this.probability = probability;
            this.time = time;
            this.type = type;
        }
    }

    // File Path
    const string filePath = "/data/user/0/com.UQAC.OceanEmpire/files/activities.txt";

    // Debug
    const string exempleFile = "0|Fri Nov 17 14:34:14 EST 2017\n\r0|Fri Nov 17 14:40:00 EST 2017\n\r0|Fri Nov 17 14:45:14 EST 2017\n\r";

    public static string ReadDocument()
    {
        StreamReader reader = new StreamReader(filePath);
        string result = reader.ReadToEnd();
        reader.Close();
        return result;
    }

    public static void ResetActivitiesSave()
    {
        if (Directory.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static List<Activity> LoadActivities()
    {
        string document;
#if UNITY_ANDROID && !UNITY_EDITOR
        document = ReadDocument();
#else
        document = exempleFile;
#endif

        List<Activity> result = new List<Activity>(); 

        bool readingDate = false;
        string currentProbability = "";
        string currentDateTime = "";

        for (int i = 0; i < document.Length; i++)
        {
            char currentChar = document[i];
            if(currentChar == '|')
            {
                readingDate = true;
                currentDateTime = "";
                continue;
            } else if(currentChar == '\r'){
                readingDate = false;
                result.Add(new Activity(IntParseFast(currentProbability), ConvertStringToDate(currentDateTime)));
                currentProbability = "";
                continue;
            } else if (currentChar == '\n')
                continue;

            if (readingDate)
            {
                currentDateTime += currentChar;
                continue;
            } else
            {
                currentProbability += currentChar;
                continue;
            }
        }

        return result;
    }

    private static int IntParseFast(string value)
    {
        int result = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char letter = value[i];
            result = 10 * result + (letter - 48);
        }
        return result;
    }

    private static DateTime ConvertStringToDate(string value)
    {
        string modifiedValue = value.Replace(" EST", "");
        return DateTime.ParseExact(modifiedValue, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
    }
}
