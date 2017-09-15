using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : PublicSingleton<Game>
{
    public MapInfo map;
    public int score;
    public float depthRecord;
}
