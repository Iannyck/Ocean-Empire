using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_MeleeCapture : AbstractGPCBehaviour
    {
        private readonly SceneManager sceneManager;

        public GPC_MeleeCapture(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public override void Abort()
        {
            throw new System.NotImplementedException();
        }

        public override GPCState Eval()
        {
            throw new System.NotImplementedException();
        }

        public override void Launch()
        {
            throw new System.NotImplementedException();
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}