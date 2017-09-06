using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using UnityEngine.Events;
using CCC.Manager;
using System;

namespace CCC.Utility
{
    public class Saves
    {
        static public void ThreadSave(string path, object graph, Action onComplete = null)
        {
            Thread t = new Thread(new ThreadStart(() => ThreadSaveMethod(path, graph, onComplete)));
            t.Start();
        }

        static void ThreadSaveMethod(string path, object graph, Action onComplete = null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            bf.Serialize(file, graph);
            file.Close();
            
            if (MainThread.instance == null)
                UnityEngine.Debug.Log("MainThread.cs not in the scene.");
            lock (MainThread.instance)
            {
                MainThread.AddAction(onComplete);
            }
        }

        static public void ThreadLoad(string path, Action<object> onComplete)
        {
            Thread t = new Thread(new ThreadStart(delegate () { ThreadLoadMethod(path, onComplete);}));
            t.Start();
        }

        static void ThreadLoadMethod(string path, Action<object> onComplete)
        {
            //UnityEngine.Debug.Log("load about to start" + path);
            if (!Exists(path))
            {
                onComplete.Invoke(null);
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            //UnityEngine.Debug.Log("load started: " + path);
            FileStream file = File.Open(path, FileMode.Open);
            //UnityEngine.Debug.Log("load completed" + path);
            object obj = bf.Deserialize(file);
            file.Close();


            lock (MainThread.instance)
            {
                if (onComplete != null)
                    MainThread.AddAction(delegate ()
                    {
                        onComplete(obj);
                    });
            }
        }

        static public void InstantSave(string path, object graph)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            bf.Serialize(file, graph);
            file.Close();
        }

        static public object InstantLoad(string path)
        {
            if (!Exists(path))
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            object obj = bf.Deserialize(file);
            file.Close();
            return obj;
        }

        static public bool Exists(string path)
        {
            return File.Exists(path);
        }

        static public bool Delete(string path)
        {
            if (!Exists(path))
                return false;

            File.Delete(path);
            return true;
        }
    }
}
