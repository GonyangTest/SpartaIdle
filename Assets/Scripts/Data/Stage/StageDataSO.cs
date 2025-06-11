using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> Stages = new List<StageData>();
}

[System.Serializable]
public class StageData
{
    public int StageNumber;
    public List<EnemySO> Enemies;
}