using CCC.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ActivityDetection : MonoBehaviour
{
    public class Activity
    {
        public enum ActivityType
        {
            Walking = 0
        }

        public int probability;
        public DateTime time;
        public ActivityType type;

        public Activity(int probability, DateTime time, ActivityType type = ActivityType.Walking)
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

    const bool LogLesInfoDesThread = false;

    public static void ReadDocument(Action<string> onComplete = null)
    {
        if (LogLesInfoDesThread)
            Debug.Log("UL Launching File Read - ThreadName: " + Thread.CurrentThread.Name + "   ThreadID: " + Thread.CurrentThread.ManagedThreadId);
        Thread t = new Thread(new ThreadStart(() => ThreadReadDocument(onComplete)));
        t.Start();
    }

    public static void ThreadReadDocument(Action<string> onComplete = null)
    {
        string result = null;
        if (LogLesInfoDesThread)
            Debug.Log("UL Reading File - ThreadName: " + Thread.CurrentThread.Name + "   ThreadID: " + Thread.CurrentThread.ManagedThreadId);
        if (File.Exists(filePath))
        {
            StreamReader reader = new StreamReader(filePath);
            if (reader.BaseStream.CanRead)
            {
                result = reader.ReadToEnd();
                //Debug.Log("UNITY FILE : " + result);
            }
            reader.Close();
        }
        if (MainThread.instance == null)
            Debug.Log("MainThread.cs not in the scene.");

        lock (MainThread.instance)
        {
            MainThread.AddAction(delegate ()
            {
                onComplete.Invoke(result);
            });
        }
    }

    public static void ResetActivitiesSave()
    {
        Thread t = new Thread(new ThreadStart(() =>
        {
            //Debug.Log("DELETING FILE");
            using (File.Create(filePath)) ;
            FileInfo info = new FileInfo(filePath);
            if (info.Length > 0)
            {
                if (MainThread.instance == null)
                    Debug.Log("MainThread.cs not in the scene.");

                lock (MainThread.instance)
                {
                    MainThread.AddAction(ResetActivitiesSave);
                }
            }
            else
            {
                MessagePopup.DisplayMessageFromThread("File Deleted - Good job !");
            }
        }));
        t.Start();
    }

    public static void LoadActivities(Action<List<Activity>> onComplete = null)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        ReadDocument(delegate(string output){
            string document = output;
            //Debug.Log("CUTTING THE FILE STRING");
            if(document == null)
            {
                onComplete.Invoke(null);
                return;
            }

            List<Activity> result = new List<Activity>();

            bool readingDate = false;
            string currentProbability = "";
            string currentDateTime = "";

            for (int i = 0; i < document.Length; i++)
            {
                char currentChar = document[i];
                if (currentChar == '|')
                {
                    readingDate = true;
                    currentDateTime = "";
                    continue;
                }
                else if (currentChar == '\r')
                {
                    readingDate = false;
                    result.Add(new Activity(IntParseFast(currentProbability), ConvertStringToDate(currentDateTime)));
                    currentProbability = "";
                    continue;
                }
                else if (currentChar == '\n')
                    continue;

                if (readingDate)
                {
                    currentDateTime += currentChar;
                    continue;
                }
                else
                {
                    currentProbability += currentChar;
                    continue;
                }
            }
            //Debug.Log("ACTIVITIES LOADED");
            onComplete.Invoke(result);
        });
#else
        string document = exempleFile;

        List<Activity> result = new List<Activity>();

        bool readingDate = false;
        string currentProbability = "";
        string currentDateTime = "";

        for (int i = 0; i < document.Length; i++)
        {
            char currentChar = document[i];
            if (currentChar == '|')
            {
                readingDate = true;
                currentDateTime = "";
                continue;
            }
            else if (currentChar == '\r')
            {
                readingDate = false;
                result.Add(new Activity(IntParseFast(currentProbability), ConvertStringToDate(currentDateTime)));
                currentProbability = "";
                continue;
            }
            else if (currentChar == '\n')
                continue;

            if (readingDate)
            {
                currentDateTime += currentChar;
                continue;
            }
            else
            {
                currentProbability += currentChar;
                continue;
            }
        }

        onComplete.Invoke(result);
#endif
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
        char[] charArray = value.ToCharArray();
        string toDelete = " " + charArray[20] + charArray[21] + charArray[22];
        string modifiedValue = value.Replace(toDelete, "");
        return DateTime.ParseExact(modifiedValue, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
    }
}
