using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : Singleton<EnemyDatabase>
{
    [SerializeField] private List<EnemySO> _enemyDatabase;
    private Dictionary<int, EnemySO> _enemyDictionary = new Dictionary<int, EnemySO>();

    protected override void Awake()
    {
        base.Awake();
        LoadEnemyDatabase();
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        _enemyDictionary.Clear();
        foreach (var enemy in _enemyDatabase)
        {
            if (enemy != null && !_enemyDictionary.ContainsKey(enemy.EnemyID))
            {
                _enemyDictionary.Add(enemy.EnemyID, enemy);
            }
        }

    }

    public EnemySO GetEnemyByID(int enemyID)
    {
        _enemyDictionary.TryGetValue(enemyID, out EnemySO enemy);
        if (enemy == null)
        {
            Debug.LogWarning($"몬스터를 찾을 수 없습니다: {enemyID}");
        }
        return enemy;
    }

    public void AddItemToDatabase(EnemySO enemy)
    {
        if (enemy != null && !_enemyDictionary.ContainsKey(enemy.EnemyID))
        {
            _enemyDatabase.Add(enemy);
            _enemyDictionary.Add(enemy.EnemyID, enemy);
        }
    }

    public void LoadEnemyDatabase()
    {
        _enemyDatabase = new List<EnemySO>(Resources.LoadAll<EnemySO>("Entity/Enemy"));
    }

    public AIEnemy SpawnEnemy(Vector3 position, EnemySO enemyDataSO)
    {
        if(enemyDataSO.EntityPrefab == null)
        {
            return null;
        }

        AIEnemy enemy = Instantiate(enemyDataSO.EntityPrefab, position, Quaternion.identity).GetComponent<AIEnemy>();
        return enemy;
    }

    public GameObject SpawnEnemy(Vector3 position, Quaternion quaternion, EnemySO enemyDataSO)
    {
        if(enemyDataSO.EntityPrefab == null)
        {
            return null;
        }

        GameObject enemy = Instantiate(enemyDataSO.EntityPrefab, position, quaternion);
        return enemy;
    }
}