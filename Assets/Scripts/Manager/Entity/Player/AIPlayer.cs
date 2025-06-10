using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPlayer : MonoBehaviour, IDamageable
{
    
    public PlayerManager PlayerManager {get; private set;}
    public NavMeshAgent NavMeshAgent {get; private set;}
    public ForceReceiver ForceReceiver {get; private set;}
    private PlayerStateMachine stateMachine;
    public GameObject HealthBar;
    public Health Health {get; private set;}

    [SerializeField] private Transform _target;
    public Transform Target => _target; // Public 접근자 추가

    [field:SerializeField] public PlayerSO Data {get; private set;}
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

        stateMachine = new PlayerStateMachine(this);

    }

    public void Start()
    {
        PlayerManager = GameManager.Instance.PlayerManager;
        PlayerManager.Player = this;
        PlayerManager.OnTotalStatChanged(new BuffBonus());

        Health.Initialize(Data);
        CreateHealthBar();

        // NavMeshAgent 초기화 확인
        if (NavMeshAgent == null)
        {
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

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destination, out hit, 5f, NavMesh.AllAreas))
        {
            NavMeshAgent.SetDestination(hit.position);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetAnimatorSpeed(float agentSpeed, float speed)
    {
        Animator.speed = agentSpeed / speed;
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    public void TakeDamage(int damage)
    {
        Health.TakeDamage(damage);
    }
}
