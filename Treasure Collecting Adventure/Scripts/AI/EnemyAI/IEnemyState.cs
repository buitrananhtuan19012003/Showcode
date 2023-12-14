public interface IEnemyState
{
    EnemyStateId GetId();
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}
