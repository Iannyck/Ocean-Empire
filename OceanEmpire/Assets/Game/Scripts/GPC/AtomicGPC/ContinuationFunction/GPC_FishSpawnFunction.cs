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

        public override GPCState Eval()
        {
            var parallState = parall.Eval();
            if (parallState == GPCState.FAILURE)
            {
                // NEW FISH SPAWN !!

                List<IGPComponent> fishGPCs = null;

                // Get ongoing fish GPCs
                if(parall.GetChildren().Count >= 2)
                {
                    var list = parall.GetChildren()[1] as ParallelAND;
                    if(list != null)
                    {
                        fishGPCs = list.GetOngoingChildren();
                    }
                }

                if (fishGPCs == null)
                    fishGPCs = new List<IGPComponent>();

                // Get new fish GPCs
                var pendingGPCs = sceneManager.Read<PendingFishGPC>("pending fish gpc");
                if(pendingGPCs != null)
                {
                    var pendingList = pendingGPCs.List;
                    int newBeginIndex = fishGPCs.Count;

                    // Update capacity
                    if(fishGPCs.Capacity < fishGPCs.Count + pendingList.Count)
                    {
                        fishGPCs.Capacity = fishGPCs.Count + pendingList.Count;
                    }
                    
                    // Add new GPCs
                    for (int i = 0; i < pendingList.Count; i++)
                    {
                        fishGPCs.Add(new Ignore(pendingList[i]));
                    }

                    // Clear pending list
                    pendingList.Clear();
                    
                    // Launch new GPCs
                    for (int i = newBeginIndex; i < fishGPCs.Count; i++)
                    {
                        fishGPCs[i].Launch();
                    }
                }
                Debug.Log("rebuild");

                // Rebuild tree
                Rebuild(fishGPCs);
                parall.Launch();
            }
            else if (parallState == GPCState.SUCCESS)
            {
                return GPCState.SUCCESS;
            }

            return GPCState.RUNNING;
        }

        public override void Abort()
        {
            parall.Abort();
        }

        public override void Launch()
        {
            parall.Launch();
        }

        public override void Reset()
        {
            parall.Reset();
        }

        void Rebuild(List<IGPComponent> fishList)
        {
            var spawnChecker = new GPC_FishSpawnCheck(sceneManager);
            var paralleleFish = new ParallelAND(fishList);
            parall = new ParallelAND(spawnChecker, paralleleFish);
        }
        void Rebuild()
        {
            var spawnChecker = new GPC_FishSpawnCheck(sceneManager);
            parall = new ParallelAND(spawnChecker);
        }
    }
}
