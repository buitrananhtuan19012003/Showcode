using System.Collections;
using UnityEngine;

public class Character_Xerath : Character
{
    [Header("XERATH")]
    [SerializeField] private int aplhaFistDamage = 7;
    [SerializeField] private Xerath_Beta xerath_Beta;
    [SerializeField] private Xerath_Alpha xerath_Alpha;

    [Space(10)]
    [SerializeField] private GameObject betaObj;
    [SerializeField] private CharacterController b_CharacterController;
    [SerializeField] private Animator b_Animator;
    [SerializeField] private Animator b_RigAnimator;
    [SerializeField] private Transform b_TPS_LookAt;
    [SerializeField] private Transform b_FPS_Follow;
    [SerializeField] private OnEventAnimator b_OnEventAnimator;
    [SerializeField] private DealDamageCtrl b_DealDamageCtrl;
    [SerializeField] private TakeDamageCtrl b_TakeDamageCtrl;
    [SerializeField] private RagdollController b_RagdollCtrl;
    [SerializeField] private Transform b_CenterPoint;
    [SerializeField] private LeadTracker b_LeadTracker;
    [SerializeField] private AlliancePlayer_InfoScanner b_AlliancePlayerInfoScanner;

    [SerializeField] private GameObject alphaObj;
    [SerializeField] private CharacterController a_CharacterController;
    [SerializeField] private Animator a_Animator;
    [SerializeField] private Animator a_RigAnimator;
    [SerializeField] private Transform a_TPS_LookAt;
    [SerializeField] private Transform a_FPS_Follow;
    [SerializeField] private OnEventAnimator a_OnEventAnimator;
    [SerializeField] private DealDamageCtrl a_DealDamageCtrl;
    [SerializeField] private TakeDamageCtrl a_TakeDamageCtrl;
    [SerializeField] private RagdollController a_RagdollCtrl;
    [SerializeField] private Transform a_CenterPoint;
    [SerializeField] private LeadTracker a_LeadTracker;
    [SerializeField] private AlliancePlayer_InfoScanner a_AlliancePlayerInfoScanner;

    [Space(10)]
    [SerializeField] private Vector3 b_CameraOffset = new Vector3(0.75f, 0.12f, 0);
    [SerializeField] private Vector3 a_CameraOffset = new Vector3(1f, 0.12f, -0.25f);
    [SerializeField] private ParticleSystem transformFX;
    [SerializeField] private ParticleSystem timeoutFX;

    [SerializeField] private bool isBeta = true;

    public int AlphaFistDamage { get => this.aplhaFistDamage; }
    public OnEventAnimator B_OnEventAnimator { get => this.b_OnEventAnimator; }
    public OnEventAnimator A_OnEventAnimator { get => this.a_OnEventAnimator; }

    protected override void LoadComponent()
    {
        this.LoadXerath();
        base.LoadComponent();
    }
    private void LoadXerath()
    {
        if (this.xerath_Beta == null)
            this.xerath_Beta = GetComponentInChildren<Xerath_Beta>();

        if (this.xerath_Alpha == null)
            this.xerath_Alpha = GetComponentInChildren<Xerath_Alpha>();

        if (this.betaObj == null)
            this.betaObj = transform.Find("Xerath_Beta").gameObject;

        if (this.betaObj != null && this.b_CharacterController == null)
            this.b_CharacterController = this.betaObj.GetComponent<CharacterController>();

        if (this.betaObj != null && this.b_Animator == null)
            this.b_Animator = this.betaObj.GetComponent<Animator>();

        if (this.betaObj != null && this.b_RigAnimator == null)
            this.b_RigAnimator = this.betaObj.transform.Find("RigLayer").GetComponent<Animator>();

        if (this.betaObj != null && this.b_TPS_LookAt == null)
            this.b_TPS_LookAt = this.betaObj.transform.Find("TPS_LookAt");

        if (this.betaObj != null && this.b_FPS_Follow == null)
            this.b_FPS_Follow = this.betaObj.transform.Find("RigLayer").Find("WeaponHolder").Find("FPS_Follow");

        if (this.betaObj != null && this.b_OnEventAnimator == null)
            this.b_OnEventAnimator = this.betaObj.GetComponent<OnEventAnimator>();

        if (this.betaObj != null && this.b_DealDamageCtrl == null)
            this.b_DealDamageCtrl = this.betaObj.GetComponent<DealDamageCtrl>();

        if (this.betaObj != null && this.b_TakeDamageCtrl == null)
            this.b_TakeDamageCtrl = this.betaObj.GetComponent<TakeDamageCtrl>();

        if (this.betaObj != null && this.b_RagdollCtrl == null)
            this.b_RagdollCtrl = this.betaObj.GetComponentInChildren<RagdollController>();

        if (this.betaObj != null && this.b_LeadTracker == null)
            this.b_LeadTracker = this.betaObj.GetComponent<LeadTracker>();

        if (this.betaObj != null && this.b_CenterPoint == null)
            this.b_CenterPoint = this.betaObj.transform.Find("Root/Hips/Spine_01/Spine_02");

        if (this.betaObj != null && this.b_AlliancePlayerInfoScanner == null)
            this.b_AlliancePlayerInfoScanner = this.betaObj.GetComponent<AlliancePlayer_InfoScanner>();


        if (this.alphaObj == null)
            this.alphaObj = transform.Find("Xerath_Alpha").gameObject;

        if (this.alphaObj != null && this.a_CharacterController == null)
            this.a_CharacterController = this.alphaObj.GetComponent<CharacterController>();

        if (this.alphaObj != null && this.a_Animator == null)
            this.a_Animator = this.alphaObj.GetComponent<Animator>();

        if (this.alphaObj != null && this.a_RigAnimator == null)
            this.a_RigAnimator = this.alphaObj.transform.Find("RigLayer").GetComponent<Animator>();

        if (this.alphaObj != null && this.a_TPS_LookAt == null)
            this.a_TPS_LookAt = this.alphaObj.transform.Find("TPS_LookAt");

        if (this.alphaObj != null && this.a_FPS_Follow == null)
            this.a_FPS_Follow = this.alphaObj.transform.Find("RigLayer").Find("WeaponHolder").Find("FPS_Follow");

        if (this.alphaObj != null && this.a_OnEventAnimator == null)
            this.a_OnEventAnimator = this.alphaObj.GetComponent<OnEventAnimator>();

        if (this.alphaObj != null && this.a_DealDamageCtrl == null)
            this.a_DealDamageCtrl = this.alphaObj.GetComponent<DealDamageCtrl>();

        if (this.alphaObj != null && this.a_TakeDamageCtrl == null)
            this.a_TakeDamageCtrl = this.alphaObj.GetComponent<TakeDamageCtrl>();

        if (this.alphaObj != null && this.a_RagdollCtrl == null)
            this.a_RagdollCtrl = this.alphaObj.GetComponentInChildren<RagdollController>();

        if (this.alphaObj != null && this.a_LeadTracker == null)
            this.a_LeadTracker = this.alphaObj.GetComponent<LeadTracker>();

        if (this.alphaObj != null && this.a_CenterPoint == null)
            this.a_CenterPoint = this.alphaObj.transform.Find("Root/Hips/Spine_01/Spine_02");

        if (this.alphaObj != null && this.a_AlliancePlayerInfoScanner == null)
            this.a_AlliancePlayerInfoScanner = this.alphaObj.GetComponent<AlliancePlayer_InfoScanner>();
    }

    protected override void LoadCharacterTransform()
    {
        if (this.isBeta && this.characterTransform != this.betaObj.transform)
        {
            this.characterTransform = this.betaObj.transform;
        }
        else if (!this.isBeta && this.characterTransform != this.alphaObj.transform)
        {
            this.characterTransform = this.alphaObj.transform;
        }
    }
    protected override void LoadCharacterController()
    {
        if (this.isBeta && this.characterController != this.b_CharacterController)
        {
            this.characterController = this.b_CharacterController;
        }
        else if (!this.isBeta && this.characterController != this.a_CharacterController)
        {
            this.characterController = this.a_CharacterController;
        }
    }
    protected override void LoadAnimator()
    {
        if (this.isBeta && this.animator != this.b_Animator)
        {
            this.animator = this.b_Animator;
        }
        else if (!this.isBeta && this.characterController != this.a_Animator)
        {
            this.animator = this.a_Animator;
        }
    }
    protected override void LoadRigAnimator()
    {
        if (this.isBeta && this.rigAnimator != this.b_RigAnimator)
        {
            this.rigAnimator = this.b_RigAnimator;
        }
        else if (!this.isBeta && this.rigAnimator != this.a_RigAnimator)
        {
            this.rigAnimator = this.a_RigAnimator;
        }
    }
    protected override void LoadTPSLookAt()
    {
        if (this.isBeta && this.tps_LookAt != this.b_TPS_LookAt)
        {
            this.tps_LookAt = this.b_TPS_LookAt;
        }
        else if (!this.isBeta && this.rigAnimator != this.a_TPS_LookAt)
        {
            this.tps_LookAt = this.a_TPS_LookAt;
        }
    }
    protected override void LoadFPSFollow()
    {
        if (this.isBeta && this.fps_Follow != this.b_FPS_Follow)
        {
            this.fps_Follow = this.b_FPS_Follow;
        }
        else if (!this.isBeta && this.fps_Follow != this.a_FPS_Follow)
        {
            this.fps_Follow = this.a_FPS_Follow;
        }
    }
    protected override void LoadOnEventAnimator()
    {
        if (this.isBeta && this.eventAnimator != this.b_OnEventAnimator)
        {
            this.eventAnimator = this.b_OnEventAnimator;
        }
        else if (!this.isBeta && this.eventAnimator != this.a_OnEventAnimator)
        {
            this.eventAnimator = this.a_OnEventAnimator;
        }
    } 
    protected override void LoadDealDamageCtrl()
    {
        if (this.isBeta && this.dealDamageCtrl != this.b_DealDamageCtrl)
        {
            this.dealDamageCtrl = this.b_DealDamageCtrl;
        }
        else if (!this.isBeta && this.dealDamageCtrl != this.a_DealDamageCtrl)
        {
            this.dealDamageCtrl = this.a_DealDamageCtrl;
        }
    }    
    protected override void LoadTakeDamageCtrl()
    {
        if (this.isBeta && this.takeDamageCtrl != this.b_TakeDamageCtrl)
        {
            this.takeDamageCtrl = this.b_TakeDamageCtrl;
        }
        else if (!this.isBeta && this.takeDamageCtrl != this.a_TakeDamageCtrl)
        {
            this.takeDamageCtrl = this.a_TakeDamageCtrl;
        }
    }  
    protected override void LoadRagdollCtrl()
    {
        if (this.isBeta && this.ragdollCtrl != this.b_RagdollCtrl)
        {
            this.ragdollCtrl = this.b_RagdollCtrl;
        }
        else if (!this.isBeta && this.ragdollCtrl != this.a_RagdollCtrl)
        {
            this.ragdollCtrl = this.a_RagdollCtrl;
        }
    }  
    protected override void LoadLeadTracker()
    {
        if (this.isBeta && this.leadTracker != this.b_LeadTracker)
        {
            this.leadTracker = this.b_LeadTracker;
        }
        else if (!this.isBeta && this.leadTracker != this.a_LeadTracker)
        {
            this.leadTracker = this.a_LeadTracker;
        }
    }
    protected override void LoadCenterPoint()
    {
        if (this.isBeta && this.centerPoint != this.b_CenterPoint)
        {
            this.centerPoint = this.b_CenterPoint;
        }
        else if (!this.isBeta && this.centerPoint != this.a_CenterPoint)
        {
            this.centerPoint = this.a_CenterPoint;
        }
    }

    protected override void LoadAlliancePlayer_InfoScanner()
    {
        if (this.isBeta && this.alliancePlayer_InfoScanner != this.b_AlliancePlayerInfoScanner)
        {
            this.alliancePlayer_InfoScanner = this.b_AlliancePlayerInfoScanner;
        }
        else if (!this.isBeta && this.alliancePlayer_InfoScanner != this.a_AlliancePlayerInfoScanner)
        {
            this.alliancePlayer_InfoScanner = this.a_AlliancePlayerInfoScanner;
        }
    }

    protected override void Start()
    {
        base.Start();

        if (PlayerCtrl.HasInstance)
        {
            this.b_CameraOffset = PlayerCtrl.Instance.PlayerCamera.TPSCamera.GetComponent<CinemachineCameraOffset>().m_Offset;
            //this.a_OnEventAnimator.OnAnimatorMoveEvent += PlayerCtrl.Instance.PlayerLocomotion.HandleAnimatorMoveEvent;
        }
    }

    private void OnEnable()
    {
        this.SetForm(this.isBeta);
    }

    public override void ActionMouseL()
    {
        base.ActionMouseL();
        this.xerath_Alpha.Acttack();
    }

    public override void SpecialSkill()
    {
        base.SpecialSkill();
        if (this.isReadySpecialSkill)
        {
            StartCoroutine(this.TransformationCoroutine());
        }
    }

    public override void SetActiveCharacter()
    {
        base.SetActiveCharacter();
        if (!this.isBeta)
        {
            PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.CharacterSpecific);
        }
        else
        {
            //if transform to Xerath_Alpha, change action mouse left to Alpha's unarmed attack
            PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.None);
        }
    }

    private IEnumerator TransformationCoroutine()
    {
        this.isReadySpecialSkill = false;
        this.b_Animator.SetTrigger("Transformation");
        this.isSpecialSkill = true;
        yield return new WaitForSeconds(0.5f);

        this.Evolution();
        yield return new WaitForSeconds(this.characterData.ExecutionSkillTime - 1f);

        this.PlayTimeoutFX(this.alphaObj.transform);
        yield return new WaitForSeconds(0.5f);

        this.Devolution();
    }

    private void Evolution()
    {
        this.ChangerCameraOffset(false);
        this.SetForm(false);
        this.SetActiveCharacter();
        this.a_Animator.SetTrigger("Transformation");

        this.PlayTransformFX(this.alphaObj.transform.position);
    }

    private void Devolution()
    {
        this.isSpecialSkill = false;
        this.isCoolingDownSpecicalSkill = true;
        this.ChangerCameraOffset(true);
        this.SetForm(true);
        this.SetActiveCharacter();
        if (!PlayerWeaponManager.Instance.originalHolster)
        {
            Weapon weapon = PlayerWeaponManager.Instance.GetActiveWeapon();
            if (weapon == null) return;
            PlayerWeaponManager.Instance.SetAnimationEquip(weapon);
        }
        else if (PlayerWeaponManager.Instance.originalHolster)
        {
            Weapon weapon = PlayerWeaponManager.Instance.GetActiveWeapon();
            if (weapon == null) return;
            PlayerWeaponManager.Instance.SetAnimationHolsterIsnotSpecial(weapon);
        }
    }

    private void PlayTransformFX(Vector3 position)
    {
        this.transformFX.transform.position = position;
        this.transformFX.Play();
    }

    private void PlayTimeoutFX(Transform parent)
    {
        Vector3 offset = new Vector3(0f, 1f, 0f);
        this.timeoutFX.transform.SetParent(parent);
        this.timeoutFX.transform.position = parent.position + offset;
        this.timeoutFX.Play();
    }

    private void SetForm(bool isBeta)
    {
        if (isBeta)
        {
            this.betaObj.transform.position = this.alphaObj.transform.position;
            this.betaObj.transform.rotation = this.alphaObj.transform.rotation;
        }
        else
        {
            this.alphaObj.transform.position = this.betaObj.transform.position;
            this.alphaObj.transform.rotation = this.betaObj.transform.rotation;
        }

        this.betaObj.SetActive(isBeta);
        this.alphaObj.SetActive(!isBeta);

        this.isBeta = isBeta;

        this.LoadCharacterTransform();
        this.LoadCharacterController();
        this.LoadOnEventAnimator();
        this.LoadAnimator();
        this.LoadRigAnimator();
        this.LoadTPSLookAt();
        this.LoadFPSFollow();
        this.LoadOnEventAnimator();
        this.LoadDealDamageCtrl();
        this.LoadTakeDamageCtrl();
        this.LoadRagdollCtrl();
        this.LoadLeadTracker();
        this.LoadCenterPoint();
        this.LoadAlliancePlayer_InfoScanner();

        if (PlayerCtrl.HasInstance)
        {
            CutoutObject cutoutObject = PlayerCtrl.Instance.PlayerCamera.MainCamera.GetComponent<CutoutObject>();
            if (cutoutObject != null)
            {
                cutoutObject.TargetObject = this.centerPoint;
            }
        }
    }

    private void ChangerCameraOffset(bool isBeta)
    {
        CinemachineCameraOffset cinemachineCameraOffset = PlayerCtrl.Instance.PlayerCamera.TPSCamera.GetComponent<CinemachineCameraOffset>();
        if (cinemachineCameraOffset == null) return;
        cinemachineCameraOffset.m_Offset = isBeta ? this.b_CameraOffset : this.a_CameraOffset;
    }
}