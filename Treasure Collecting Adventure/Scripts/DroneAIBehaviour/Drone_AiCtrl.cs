using UnityEngine;

public class Drone_AiCtrl : SaiMonoBehaviour
{
    [Header("REFERENCE")]
    [SerializeField] private DroneCtrl droneCtrl;

    [Space(10)]
    public DroneStateId InitState;
    public DroneStateMachine DroneSM;

    public DroneCtrl DroneCtrl { get => this.droneCtrl; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.droneCtrl == null)
            this.droneCtrl = transform.parent.GetComponent<DroneCtrl>();
    }

    private void Start()
    {
        this.DroneSM = new DroneStateMachine(this);
        this.DroneSM.RegisterState(new DroneState_Idle(this));
        this.DroneSM.RegisterState(new DroneState_Follow(this));
        this.DroneSM.RegisterState(new DroneState_Attack(this));
        this.DroneSM.ChangeState(this.InitState);
    }

    private void Update()
    {
        this.DroneSM.Update();
    }

    private void FixedUpdate()
    {
        //Debug.Log("CurState: " + this.DroneSM.CurrentState);
        this.DroneSM.FixedUpdate();
    }
}
