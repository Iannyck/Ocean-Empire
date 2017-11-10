using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageReception : MonoBehaviour {

	void Start ()
    {
        MasterManager.Sync(delegate ()
        {
            //MessageReceiver.instance.JavaMessage(MessageReceiver.instance.GetAndroidMessage());
        });
	}
}
