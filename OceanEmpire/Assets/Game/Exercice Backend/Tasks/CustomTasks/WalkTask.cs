<<<<<<< .merge_file_a06976
﻿using System.Collections;
=======
﻿using System;
using System.Collections;
>>>>>>> .merge_file_a02312
using System.Collections.Generic;
using UnityEngine;

public class WalkTask : Task
{
    public float minutesOfWalk;
    public TimeSpan timeOfWalk;

    public override ExerciseType GetExerciseType()
    {
        return ExerciseType.Walk;
    }

    public WalkTask(float minutesOfWalk)
    {
        this.minutesOfWalk = minutesOfWalk;
        timeOfWalk = new TimeSpan(0, (int)minutesOfWalk, 0);
    }

    public static WalkTask Build(float difficulty)
    {
        return new WalkTask(2);
    }
}
