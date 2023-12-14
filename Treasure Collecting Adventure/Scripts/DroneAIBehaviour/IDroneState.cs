public interface IDroneState
{
    DroneStateId GetId();
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}
