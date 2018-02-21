using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System;
using CCC.Threading;

namespace CCC.Serialization
{
    public class Saves
    {
        static public void ThreadSave(string path, object graph, Action onComplete = null)
        {
            MainThread.SpawnIfNotSpawned();
            Thread t = new Thread(new ThreadStart(() => ThreadSaveMethod(path, graph, onComplete)));
            t.Start();
        }

        static void ThreadSaveMethod(string path, object graph, Action onComplete = null)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.OpenOrCreate);
            bf.Serialize(file, graph);
            file.Close();

            MainThread.AddActionFromThread(onComplete);

        }

        static public void ThreadLoad(string path, Action<object> onComplete)
        {
            MainThread.SpawnIfNotSpawned();
            Thread t = new Thread(new ThreadStart(delegate () { ThreadLoadMethod(path, onComplete); }));
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

            object obj = null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            try
            {
                obj = bf.Deserialize(file);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Failed to deserialize the following file:\n" + path + "\n\nError:\n" + e.Message);
            }
            file.Close();


            if (onComplete != null)
                MainThread.AddActionFromThread(delegate ()
                {
                    onComplete(obj);
                });
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

            object obj = null;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);

            try
            {
                obj = bf.Deserialize(file);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError("Failed to deserialize the following file:\n" + path + "\n\nError:\n" + e.Message);
            }

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
