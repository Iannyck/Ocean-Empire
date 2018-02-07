﻿ 
using CCC.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GoogleReader : MonoBehaviour
{
    public class Activity
    {
        public List<int> probabilities;
        public DateTime time;

        public Activity(int probWalk, int probRun, int probBicycle, DateTime time)
        {
            probabilities = new List<int>();
            probabilities.Add(probWalk);
            probabilities.Add(probRun);
            probabilities.Add(probBicycle);
            this.time = time;
        }

        public int GetActivityProbability(PrioritySheet.ExerciseTypes type)
        {
            return probabilities[(int)type];
        }

        public PrioritySheet.ExerciseTypes GetActivityByIndex(int i)
        {
            switch (i)
            {
                case 0:
                    return PrioritySheet.ExerciseTypes.walk;
                case 1:
                    return PrioritySheet.ExerciseTypes.run;
                case 2:
                    return PrioritySheet.ExerciseTypes.bicycle;
                default:
                    return 0;
            }
        }
    }

    // File Path
    const string filePath = "/data/user/0/com.UQAC.OceanEmpire/files/activities.txt";

    // Exemple d'une ligne dans le fichier
    const string exempleFile = "0|0|0|Fri Nov 17 14:34:14 EST 2017\n\r";

    public static bool LogLesInfoDesThread = false;

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
        MainThread.AddActionFromThread(delegate ()
        {
            onComplete.Invoke(result);
        });
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
                MainThread.AddActionFromThread(ResetActivitiesSave);
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
        // CODE EXECUTER QUAND ON EST SUR ANDROID
#if UNITY_ANDROID && !UNITY_EDITOR
        // Lecture du document sur un Thread
        ReadDocument(delegate(string output){
            string document = output; // output enregistre dans un seul string
            //Debug.Log("CUTTING THE FILE STRING");
            if(document == null)
            {
                onComplete.Invoke(null);
                return;
            }

        // Meme code que pour PC
        List<Activity> result = new List<Activity>();

        bool readingDate = false;
        string probWalk = "";
        string probRun = "";
        string probBicycle = "";
        string currentDateTime = "";

        int exerciceRead = 0;

        for (int i = 0; i < document.Length; i++)
        {
            char currentChar = document[i];
            if (currentChar == '|')
            {
                if(exerciceRead == 3)
                {
                    readingDate = true;
                    currentDateTime = "";
                }
                continue;
            }
            else if (currentChar == '\r')
            {
                readingDate = false;
                result.Add(new Activity(IntParseFast(probWalk), IntParseFast(probRun), IntParseFast(probBicycle), ConvertStringToDate(currentDateTime)));
                probWalk = "";
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
                switch (exerciceRead)
                {
                    case 0:
                        probWalk += currentChar;
                        break;
                    case 1:
                        probRun += currentChar;
                        break;
                    case 2:
                        probBicycle += currentChar;
                        break;
                    default:
                        break;
                }
                exerciceRead++;
                continue;
            }
        }
            //Debug.Log("ACTIVITIES LOADED");
            onComplete.Invoke(result);
        });
#else
        // CODE EXECUTER QUAND ON EST SUR PC
        string document = exempleFile;

        // On desire decoupe le string pour trouver tous les activites
        List<Activity> result = new List<Activity>();

        bool readingDate = false;
        string probWalk = "";
        string probRun = "";
        string probBicycle = "";
        string currentDateTime = "";

        int exerciceRead = 0;

        // Rappel structure d'un enregistrement : 0|0|0|Fri Nov 17 14:34:14 EST 2017\n\r
        //                                        marche|course|bicicle|date\FIN

        for (int i = 0; i < document.Length; i++)
        {
            char currentChar = document[i];
            if (currentChar == '|')
            {
                if(exerciceRead == 3)
                {
                    readingDate = true;
                    currentDateTime = "";
                }
                continue;
            }
            else if (currentChar == '\r')
            {
                readingDate = false;
                result.Add(new Activity(IntParseFast(probWalk), IntParseFast(probRun), IntParseFast(probBicycle), ConvertStringToDate(currentDateTime)));
                probWalk = "";
                probRun = "";
                probBicycle = "";
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
                switch (exerciceRead)
                {
                    case 0:
                        probWalk += currentChar;
                        break;
                    case 1:
                        probRun += currentChar;
                        break;
                    case 2:
                        probBicycle += currentChar;
                        break;
                    default:
                        break;
                }
                exerciceRead++;
                continue;
            }
        }

        onComplete.Invoke(result);
#endif
    }

    // Transforme un string qui a ete enregistre dans un fichier en un int
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

    // Transforme une date android enregistre dans le fichier sous le format string en une date Unity compatible
    private static DateTime ConvertStringToDate(string value)
    {
        char[] charArray = value.ToCharArray();
        string toDelete = " " + charArray[20] + charArray[21] + charArray[22];
        string modifiedValue = value.Replace(toDelete, "");
        return DateTime.ParseExact(modifiedValue, "ddd MMM dd HH:mm:ss yyyy", CultureInfo.InvariantCulture);
    }
}