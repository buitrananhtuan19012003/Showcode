using System;

public class EnemyStateMachine
{
    public IEnemyState[] States;
    public Enemy_AIController EnemyCtrl;
    public EnemyStateId CurrentState;

    public EnemyStateMachine(Enemy_AIController aiController)
    {
        this.EnemyCtrl = aiController;

        int numberState = Enum.GetNames(typeof(EnemyStateId)).Length;
        this.States = new IEnemyState[numberState];
    }

    public void RegisterState(IEnemyState state)
    {
        int index = (int)state.GetId();
        this.States[index] = state;
    }

    public IEnemyState GetState(EnemyStateId stateId)
    {
        int index = (int)stateId;
        return this.States[index];
    }

    public void Update()
    {
        this.GetState(this.CurrentState)?.Update();
    }

    public void FixedUpdate()
    {
        this.GetState(this.CurrentState)?.FixedUpdate();
    }

    public void ChangeState(EnemyStateId newState)
    {
        if (this.CurrentState == newState) return;

        this.GetState(this.CurrentState)?.Exit();
        this.CurrentState = newState;
        this.GetState(this.CurrentState).Enter();
    }
}
