using UnityEngine;

public class Enemy_AIController : EnemyAbstract
{
    [SerializeField] private AiConfig aiConfig;
    [SerializeField] private EnemyStateId initState;
    [SerializeField] private EnemyStateMachine enemySM;
    
    public AiConfig AiConfig { get => this.aiConfig; }
    public EnemyStateId InitState { get => this.initState; }
    public EnemyStateMachine EnemySM { get => this.enemySM; }

    private void Start()
    {
        this.enemySM = new EnemyStateMachine(this);
        this.enemySM.RegisterState(new EnemyState_Idle(this));
        this.enemySM.RegisterState(new EnemyState_Chase(this));
        this.enemySM.RegisterState(new EnemyState_Death(this));
    }

    private void Update()
    {
        this.enemySM.Update();
    }

    private void FixedUpdate()
    {
        //Debug.Log("CurState: " + this.EnemySM.CurrentState);
        this.enemySM.FixedUpdate();
    }
}
