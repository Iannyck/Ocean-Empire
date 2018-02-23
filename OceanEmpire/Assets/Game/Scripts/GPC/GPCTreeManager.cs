using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPComponents;

public class GPCTreeManager : MonoBehaviour
{
    [SerializeField] private Game gameSystem;
    [SerializeField] private SceneManager sceneManager;

    private ParallelAND tree;

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
                gameSystem.EndGame();
            }
        }
    }

    void CreateTree()
    {
        tree = new ParallelAND();
        tree.AddChild(new FuelCheck(sceneManager));
        tree.Launch();
    }
}
