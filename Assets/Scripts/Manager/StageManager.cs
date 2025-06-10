using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    public int CurrentStage { get; private set; }

    public void StartStage()
    {
        CurrentStage = 1;
    }

    public void NextStage()
    {
        CurrentStage++;
    }
    
}
