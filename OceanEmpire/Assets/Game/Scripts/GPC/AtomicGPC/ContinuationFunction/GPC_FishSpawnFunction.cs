using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_FishSpawnFunction : ContinuationFunction
    {
        private readonly SceneManager sceneManager;
        private ParallelAND parall;

        public GPC_FishSpawnFunction(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
            
            Rebuild();
        }

        public override void Abort()
        {

        }

        public override GPCState Eval()
        {
            var parallState = parall.Eval();
            if (parallState == GPCState.FAILURE)
            {
                //New spawn
            }
            else if (parallState == GPCState.SUCCESS)
            {
                return GPCState.SUCCESS;
            }

            return GPCState.RUNNING;
        }

        public override void Launch()
        {

        }

        public override void Reset()
        {

        }

        void Rebuild(List<IGPComponent> fishList)
        {
            var spawnChecker = new GPC_FishSpawnCheck(sceneManager);
            var paralleleFish = new ParallelAND(fishList);
            parall = new ParallelAND(spawnChecker);
        }
        void Rebuild()
        {
            var spawnChecker = new GPC_FishSpawnCheck(sceneManager);
            parall = new ParallelAND(spawnChecker);
        }
    }
}
