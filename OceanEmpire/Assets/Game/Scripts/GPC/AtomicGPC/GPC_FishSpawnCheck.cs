using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_FishSpawnCheck : AbstractGPCBehaviour
    {
        SceneManager sceneManager;

        PendingFishGPC pendingFishGPC;

        public GPC_FishSpawnCheck(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            pendingFishGPC = sceneManager.Read<PendingFishGPC>("pending fish gpc");
        }

        public override void Abort()
        {

        }

        public override GPCState Eval()
        {
            if(pendingFishGPC != null)
            {
                if (pendingFishGPC.List.Count > 0)
                    return GPCState.FAILURE;
            }
            return GPCState.RUNNING;
        }

        public override void Launch()
        {

        }

        public override void Reset()
        {

        }
    }
}
