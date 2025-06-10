using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour, IDamageable
{
    public NavMeshAgent NavMeshAgent {get; private set;}
    public ForceReceiver ForceReceiver {get; private set;}
    private EnemyStateMachine stateMachine;
    public GameObject HealthBar;
    public Health Health {get; private set;}

    [SerializeField] private Transform _target;
    public Transform Target => _target; // Public 접근자 추가

    [field:SerializeField] public EnemySO Data {get; private set;}
    [field:Header("Animations")]
    [field:SerializeField] public AnimationData AnimationData {get; private set;}

    public Animator Animator {get; private set;}
    public CharacterController Controller {get; private set;}

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<Health>();

        stateMachine = new EnemyStateMachine(this);

        CreateHealthBar();
    }

    public void Start()
    {

        Health.Initialize(Data);
        Health.OnDeath += Death;
        CreateHealthBar();

        // NavMeshAgent 초기화 확인
        if (NavMeshAgent == null)
        {
            Debug.LogError($"{gameObject.name}: NavMeshAgent 컴포넌트가 없습니다!");
            return;
        }
        // Player는 다른 Agent를 피하지 않도록 설정
        NavMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public bool SetDestination(Vector3 destination)
    {
        if (NavMeshAgent == null || !NavMeshAgent.enabled || !NavMeshAgent.isOnNavMesh)
        {
            return false;
        }

        // 목적지가 NavMesh 위에 있는지 확인
        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            NavMeshAgent.SetDestination(hit.position);
            Debug.Log($"목적지 설정: {hit.position}");
            return true;
        }
        else
        {
            Debug.LogWarning($"목적지 {destination}가 NavMesh 위에 없습니다!");
            return false;
        }
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void CreateHealthBar()
    {
        HealthBar = Instantiate(HealthBar, transform);
    }

    public void DestroyHealthBar()
    {
        Destroy(HealthBar);
    }

    protected void OnDrawGizmos()
    {
        // if(stateMachine.IsChasing)
        // {
        //     Gizmos.color = Color.red;
        // }
        // else{
        //     Gizmos.color = Color.green;
        // }
        // Gizmos.DrawLine(transform.position, Target.position);

        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    public void TakeDamage(int amount)
    {
        int damage = amount - Data.StatData.BaseDefense;
        if(damage < 0)
            damage = 0;

        Health.TakeDamage(damage);
    }

    public void Death()
    {
        stateMachine.ChangeState(stateMachine.DeadState);

        if(Data.RewardData.DropItemDatas.Count > 0)
        {
            foreach(var itemData in Data.RewardData.DropItemDatas)
            {
                int random = Random.Range(0, 100);
                if(random < itemData.DropRate)
                {
                    InventoryManager.Instance.AddItem(new ItemInstance(itemData.Item, 1));
                }
            }
        }

        if(Data.RewardData.Gold > 0)
        {
            CurrencyManager.Instance.AddGold(Data.RewardData.Gold);
        }

        if(Data.RewardData.Exp > 0)
        {
            PlayerManager.Instance.AddExp(Data.RewardData.Exp);
        }   
    }
}
