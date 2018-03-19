using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPComponents;

public class GPCTreeManager : MonoBehaviour
{
    [SerializeField] private Game gameSystem;
    [SerializeField] private SceneManager sceneManager;

    private ParallelOR tree;

    private void Update()
    {
        //NOTE: la classe Game devrait contenir un element d'enum pour identifier sont gameState (et non plusieurs bools)
        if (gameSystem.gameStarted && gameSystem.gameRunning && !gameSystem.gameOver)
        {
            if (tree == null)
                CreateTree();

            var result = tree.Eval();

            if (result == GPCState.FAILURE)
            {
                Debug.LogWarning("The player lost the game. This might not be an intended behaviour. Disable this log otherwise.");
                gameSystem.EndGame();
            }
            else if (result == GPCState.SUCCESS)
            {
                print("endgame");
                gameSystem.EndGame();
            }
        }
    }

    void CreateTree()
    {
        // 1 - Fuel check. Fin de la partie lorsque le joueur n'a plus de fuel
        GPC_FuelCheck fuelCheck = new GPC_FuelCheck(sceneManager);

        // 2 - Periodic spawn de poisson
        GPC_FishSpawnFunction spawnFunc = new GPC_FishSpawnFunction(sceneManager);
        Continuation fishSpawning = new Continuation(spawnFunc);
        

        
        // Tree build
        tree = new ParallelOR(fuelCheck, fishSpawning);
        tree.Launch();
    }
}
