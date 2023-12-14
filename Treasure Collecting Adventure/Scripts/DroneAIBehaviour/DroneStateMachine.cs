using System;

public class DroneStateMachine
{
    public IDroneState[] States;
    public Drone_AiCtrl DroneCtrl;
    public DroneStateId CurrentState;

    public DroneStateMachine(Drone_AiCtrl aiController)
    {
        this.DroneCtrl = aiController;

        int numberState = Enum.GetNames(typeof(DroneStateId)).Length;
        this.States = new IDroneState[numberState];
    }

    public void RegisterState(IDroneState state)
    {
        int index = (int)state.GetId();
        this.States[index] = state;
    }

    public IDroneState GetState(DroneStateId stateId)
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

    public void ChangeState(DroneStateId newState)
    {
        if (this.CurrentState == newState) return;

        this.GetState(this.CurrentState)?.Exit();
        this.CurrentState = newState;
        this.GetState(this.CurrentState).Enter();
    }
}
