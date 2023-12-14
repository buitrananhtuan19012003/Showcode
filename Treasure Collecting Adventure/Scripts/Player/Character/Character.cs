using UnityEngine;

public class Character : SaiMonoBehaviour
{
    [Header("CHARACTER")]
    [SerializeField] protected CharacterDataSO characterData;
    [SerializeField] protected CharacterRigAttach characterRigAttach;
    [SerializeField] protected Transform characterTransform;
    [SerializeField] protected Transform centerPoint;
    [SerializeField] protected CharacterController characterController;
    [SerializeField] protected OnEventAnimator eventAnimator;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Animator rigAnimator;
    [SerializeField] protected Transform tps_LookAt;
    [SerializeField] protected Transform fps_Follow;
    [SerializeField] protected Transform[] weaponSheathSlots;
    [SerializeField] protected DealDamageCtrl dealDamageCtrl;
    [SerializeField] protected TakeDamageCtrl takeDamageCtrl;
    [SerializeField] protected RagdollController ragdollCtrl;
    [SerializeField] protected LeadTracker leadTracker;
    [SerializeField] protected AlliancePlayer_InfoScanner alliancePlayer_InfoScanner;

    [Space(10)]
    protected bool isReadySpecialSkill = true;
    protected bool isCoolingDownSpecicalSkill;
    protected float executionSpecialSkill;
    protected float cooldownSpecialSkill;
    protected float timerEX_SpecialSkill;
    protected float timerCD_SpecialSkill;

    protected bool isReadyBattleSkill = true;
    protected bool isCoolingdownBattleSkill;
    protected float cooldownBattleSkill;
    protected float timerCD_BattleSkill;

    protected bool isSpecialSkill = false;
    protected bool isMiss;
    protected float missCooldownTime = 0.75f;

    public CharacterDataSO CharacterData { get => this.characterData; }
    public CharacterRigAttach CharacterRigAttach { get => this.characterRigAttach; }
    public Transform CharacterTransform { get => this.characterTransform; }
    public Transform CenterPoint { get => this.centerPoint; }
    public CharacterController CharacterController { get => this.characterController; }
    public Animator Animator { get => this.animator; }
    public Animator RigAnimator { get => this.rigAnimator; }
    public Transform TPS_LookAt { get => this.tps_LookAt; }
    public Transform FPS_Follow { get => this.fps_Follow; }
    public OnEventAnimator EventAnimator { get => this.eventAnimator; }
    public Transform[] WeaponSheathSlots { get => this.weaponSheathSlots; }
    public DealDamageCtrl DealDamageCtrl { get => this.dealDamageCtrl; }
    public TakeDamageCtrl TakeDamageCtrl { get => this.takeDamageCtrl; }
    public RagdollController RagdollCtrl { get => this.ragdollCtrl; }
    public LeadTracker LeadTracker { get => this.leadTracker; }
    public AlliancePlayer_InfoScanner AlliancePlayer_InfoScanner { get => this.alliancePlayer_InfoScanner; }

    public bool IsSpecialSkill { get => this.isSpecialSkill; }
    public bool IsReadySpecialSkill { get => this.isReadySpecialSkill; }
    public bool IsCoolingDownSpecicalSkill { get => this.isCoolingDownSpecicalSkill; }
    public float ExecutionSpecialSkill { get => this.executionSpecialSkill; }
    public float CooldownSpecialSkill { get => this.cooldownSpecialSkill; }
    public float TimerEX_SpecialSkill { get => this.timerCD_SpecialSkill; }
    public float TimerCD_SpecialSkill { get => this.timerCD_SpecialSkill; }


    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadCharacterTransform();
        this.LoadCharacterRigAttach();
        this.LoadCharacterController();
        this.LoadAnimator();
        this.LoadRigAnimator();
        this.LoadTPSLookAt();
        this.LoadFPSFollow();
        this.LoadOnEventAnimator();
        this.LoadWeaponSheathSlots();

        this.LoadDealDamageCtrl();
        this.LoadTakeDamageCtrl();
        this.LoadRagdollCtrl();
        this.LoadCenterPoint();
        this.LoadLeadTracker();
        this.LoadAlliancePlayer_InfoScanner();
    }

    protected virtual void LoadDealDamageCtrl()
    {
        if (this.dealDamageCtrl == null)
            this.dealDamageCtrl = GetComponent<DealDamageCtrl>();
    }

    protected virtual void LoadTakeDamageCtrl()
    {
        if (this.takeDamageCtrl == null)
            this.takeDamageCtrl = GetComponent<TakeDamageCtrl>();
    }

    protected virtual void LoadRagdollCtrl()
    {
        if (this.ragdollCtrl == null)
        {
            this.ragdollCtrl = GetComponentInChildren<RagdollController>();
            this.ragdollCtrl.Animator = this.animator;
            this.ragdollCtrl.CharacterController = this.characterController;
        }
    }

    protected virtual void LoadCenterPoint()
    {
        if (this.centerPoint == null)
            this.centerPoint = transform.Find("Root/Hips/Spine_01/Spine_02");
    }

    protected virtual void LoadLeadTracker()
    {
        if (this.leadTracker == null)
            this.leadTracker = GetComponent<LeadTracker>();
    }
    protected virtual void LoadCharacterTransform()
    {
        if (this.characterTransform == null)
        {
            this.characterTransform = transform;
            Debug.LogWarning(gameObject.name + ": LoadCharacterTransform", gameObject);
        }
    }
    protected virtual void LoadCharacterRigAttach()
    {
        if (this.characterRigAttach == null)
        {
            this.characterRigAttach = GetComponent<CharacterRigAttach>();
            Debug.LogWarning(gameObject.name + ": LoadcharacterRigAttach", gameObject);
        }
    }
    protected virtual void LoadCharacterController()
    {
        if (this.characterController == null)
        {
            this.characterController = GetComponent<CharacterController>();
            Debug.LogWarning(gameObject.name + ": LoadCharacterController", gameObject);
        }
    } 
    protected virtual void LoadAnimator()
    {
        if (this.animator == null)
        {
            this.animator = GetComponent<Animator>();
            Debug.LogWarning(gameObject.name + ": LoadAnimator", gameObject);
        }
    }
    protected virtual void LoadRigAnimator()
    {
        if (this.rigAnimator == null)
        {
            this.rigAnimator = transform.Find("------RigLayers-----").GetComponent<Animator>();
            Debug.LogWarning(gameObject.name + ": LoadRigAnimator", gameObject);
        }
    }
    protected virtual void LoadTPSLookAt()
    {
        if (this.tps_LookAt == null)
        {
            this.tps_LookAt = transform.Find("TPS_LookAt");
            Debug.LogWarning(gameObject.name + ": LoadTPSLookAt", gameObject);
        }
    }
    protected virtual void LoadFPSFollow()
    {
        if (this.fps_Follow == null)
        {
            this.fps_Follow = transform.Find("------RigLayers-----").Find("WeaponHolder").Find("FPS_Follow");
            Debug.LogWarning(gameObject.name + ": LoadFPSFollow", gameObject);
        }
    }
    protected virtual void LoadOnEventAnimator()
    {
        if (this.eventAnimator == null)
        {
            this.eventAnimator = GetComponent<OnEventAnimator>();
            Debug.LogWarning(gameObject.name + ": LoadOnEventAnimator", gameObject);
        }
    }
    protected virtual void LoadWeaponSheathSlots()
    {
        if (this.weaponSheathSlots.Length != 3)
        {
            this.weaponSheathSlots = new Transform[3];
            Transform rigLayer_WeaponAim = this.rigAnimator.transform.Find("RigLayer_WeaponAim");
            this.weaponSheathSlots[0] = rigLayer_WeaponAim.Find("WeaponSlotLeft_Contains");
            this.weaponSheathSlots[1] = rigLayer_WeaponAim.Find("WeaponSlotRight_Contains");
            this.weaponSheathSlots[2] = rigLayer_WeaponAim.Find("WeaponSlotBack_Contains");
        }
    }

    protected virtual void LoadAlliancePlayer_InfoScanner()
    {
        if (this.alliancePlayer_InfoScanner == null)
            this.alliancePlayer_InfoScanner = GetComponent<AlliancePlayer_InfoScanner>();
    }

    protected virtual void Start()
    {

        if (this.characterData != null)
        {
            this.cooldownSpecialSkill = this.characterData.CooldownSkillTime;
            this.executionSpecialSkill = this.characterData.ExecutionSkillTime;
        }
    }

    protected virtual void Update()
    {
        if (this.isCoolingDownSpecicalSkill)
        {
            this.CoolingdownSpecialSkill();
        }
        if (this.isCoolingdownBattleSkill)
        {
            this.CoolingdownBattleSkill();
        }
        Debug.Log("IsSpecialSkill" + isSpecialSkill);
    }

    public virtual void ActionMouseL()
    {
        //for overrite
    }

    public virtual void ActionMouseR(bool inputButton)
    {
        //for overrite
    }

    public virtual void SpecialSkill()
    {
        //for overrite
    }

    public virtual void BattleSkill()
    {
        //for overrite
    }

    public virtual void SetActiveCharacter()
    {
        if (PlayerCtrl.HasInstance)
        {
            PlayerCtrl.Instance.SetCharacter(this);
        }
    }

    protected virtual void CoolingdownSpecialSkill()
    {
        this.timerCD_SpecialSkill += Time.deltaTime;
        if (this.timerCD_SpecialSkill < this.cooldownSpecialSkill) return;
        this.timerCD_SpecialSkill = 0;
        this.isReadySpecialSkill = true;
        this.isCoolingDownSpecicalSkill = false;
        this.isMiss = true;
    }

    protected virtual void CoolingdownBattleSkill()
    {
        this.timerCD_BattleSkill += Time.deltaTime;
        if (this.timerCD_BattleSkill < this.cooldownBattleSkill) return;

        this.timerCD_BattleSkill = 0;
        this.isReadyBattleSkill = true;
        this.isCoolingdownBattleSkill = false;
    }
}