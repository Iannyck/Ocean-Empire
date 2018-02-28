using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableUnit : RevivableUnit
{
    [System.NonSerialized] public PoolableUnit originalCopy;
}
