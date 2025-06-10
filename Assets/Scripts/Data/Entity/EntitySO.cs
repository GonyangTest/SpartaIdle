using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EntityHealthData
{
    [field:SerializeField] public int MaxHealth {get; private set;}
    [field:SerializeField] public float InvulnerabilityDuration {get; private set;}


}

[CreateAssetMenu(fileName = "Entity", menuName = "ScriptableObject/Entity")]
public class EntitySO : ScriptableObject
{
    [field:SerializeField] public GameObject EntityPrefab {get; private set;}
    [field:SerializeField] public EntityHealthData HealthData {get; private set;}
}
