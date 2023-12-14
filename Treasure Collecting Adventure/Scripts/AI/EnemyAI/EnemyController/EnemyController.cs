using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : SaiMonoBehaviour
{
    [SerializeField] protected EnemyDataSO enemyData;
    [SerializeField] protected Animator animator;
    [SerializeField] protected CharacterController characterController;
    [SerializeField] protected NavMeshAgent navMeshAgent;
    [SerializeField] protected Transform centerPoint;
    [SerializeField] protected EnemyDealDamageCtrl enemyDealDamageCtrl;
    [SerializeField] protected TakeDamageCtrl takeDamageCtrl;
    [SerializeField] protected GraphicEffect graphicEffect;
    [SerializeField] protected DetectTarget detectTarget;
    [SerializeField] protected Enemy_AIController enemyAiCtrl;
    [SerializeField] protected EnemyLocomotion enemyLocomotion;
    [SerializeField] protected EnemyHealth enemyHealth;
    [SerializeField] protected EnemyDebuffs enemyDebuffs;
    [SerializeField] protected RagdollController ragdollCtrl;
    [SerializeField] protected ParticleSystem fxBlood;

    public IInfoScanner CurInfoScanTarget;//For LookAt
    public Vector3 FollowPos;   //For SetDestination NavMeshAgent
    protected bool checkDetect = true;

    public EnemyDataSO EnemyData { get => this.enemyData; }
    public Animator Animator { get => this.animator; }
    public CharacterController CharacterController { get => this.characterController; }
    public NavMeshAgent NavMeshAgent { get => this.navMeshAgent; }
    public Transform CenterPoint { get => this.centerPoint; }
    public GraphicEffect GraphicEffect { get => this.graphicEffect; }
    public DetectTarget DetectTarget { get => this.detectTarget; }
    public Enemy_AIController EnemyAiCtrl { get => this.enemyAiCtrl; }
    public EnemyLocomotion EnemyLocomotion { get => this.enemyLocomotion; }
    public EnemyHealth EnemyHealth { get => this.enemyHealth; }
    public EnemyDebuffs EnemyDebuffs { get => this.enemyDebuffs; }
    public RagdollController RagdollCtrl { get => this.ragdollCtrl; }
    public ParticleSystem FxBlood { get => this.fxBlood; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.animator == null)
            this.animator = GetComponent<Animator>();

        if (this.characterController == null)
            this.characterController = GetComponent<CharacterController>();

        if (this.navMeshAgent == null)
            this.navMeshAgent = GetComponent<NavMeshAgent>();

        if (this.centerPoint == null)
            //this.centerPoint = transform.Find("CenterPoint");
            this.centerPoint = transform.Find("Root/Hips/Spine_01/Spine_02");

        if (this.graphicEffect == null)
            this.graphicEffect = GetComponentInChildren<GraphicEffect>();
        if (this.graphicEffect != null)
        {
            if (this.graphicEffect.SkinnedMeshRenderer == null)
                this.graphicEffect.SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            if (this.graphicEffect.ListMeshRenderer.Count != meshRenderers.Length)
            {
                for (int i = 0; i < meshRenderers.Length; i++)
                {
                    this.graphicEffect.ListMeshRenderer.Add(meshRenderers[i]);
                }
            }
        }

        if (this.detectTarget == null)
            this.detectTarget = GetComponentInChildren<DetectTarget>();

        if (this.enemyAiCtrl == null)
            this.enemyAiCtrl = GetComponentInChildren<Enemy_AIController>();

        if (this.enemyLocomotion == null)
            this.enemyLocomotion = GetComponentInChildren<EnemyLocomotion>();

        if (this.enemyHealth == null)
            this.enemyHealth = GetComponentInChildren<EnemyHealth>();

        if (this.enemyDebuffs == null)
            this.enemyDebuffs = GetComponentInChildren<EnemyDebuffs>();

        if (this.ragdollCtrl == null)
        {
            this.ragdollCtrl = GetComponentInChildren<RagdollController>();
            this.ragdollCtrl.Animator = this.animator;
            this.ragdollCtrl.CharacterController = this.characterController;
        }

        if (this.enemyDealDamageCtrl == null)
            this.enemyDealDamageCtrl = GetComponent<EnemyDealDamageCtrl>();

        if (this.takeDamageCtrl == null)
        {
            this.takeDamageCtrl = GetComponent<TakeDamageCtrl>();
            this.takeDamageCtrl.SetHealthObject(this.enemyHealth.gameObject);
        }

        if (this.fxBlood == null)
            this.fxBlood = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {
        if (this.enemyData != null)
        {
            transform.gameObject.name = this.enemyData.EnemyName;
            this.enemyHealth.SetHealth(this.EnemyData.Health);
            this.enemyDealDamageCtrl.DealDamageBox.Damage = this.EnemyData.Damage;
            this.navMeshAgent.speed = this.EnemyData.Speed;
            this.detectTarget.DetectionRange = this.enemyData.DetectionRange;
        }
    }

    protected virtual void Update()
    {
        if (this.enemyHealth.IsDeath())
            return;

        IInfoScanner infoScanner = this.detectTarget.FindClosest(FactionType.Alliance);
        if (infoScanner != null)
        {
            if (this.CurInfoScanTarget != infoScanner)
            {
                this.CurInfoScanTarget = infoScanner;
            }

            if (infoScanner is AlliancePlayer_InfoScanner)
            {
                Character character = this.CurInfoScanTarget.GetTransform().GetComponent<Character>();
                if (character != null)
                    character.LeadTracker.Add(this);
                else //XERATH
                    this.CurInfoScanTarget.GetTransform().GetComponentInParent<Character>().LeadTracker.Add(this);
            }
            if (infoScanner is AllianceCompanion_InfoScanner)
            {
                this.CurInfoScanTarget.GetTransform().GetComponent<LeadTracker>().Add(this);
            }
        }
        else
        {
            if (this.CurInfoScanTarget != null)
            {
                if (infoScanner is AlliancePlayer_InfoScanner)
                {
                    Character character = this.CurInfoScanTarget.GetTransform().GetComponent<Character>();
                    if (character != null)
                        character.LeadTracker.Remove(this);
                    else //XERATH
                        this.CurInfoScanTarget.GetTransform().GetComponentInParent<Character>().LeadTracker.Remove(this);
                }
                if (infoScanner is AllianceCompanion_InfoScanner)
                {
                    this.CurInfoScanTarget.GetTransform().GetComponent<LeadTracker>().Remove(this);
                }
                this.CurInfoScanTarget = null;
            }
        }

        if (this.CurInfoScanTarget != null && this.checkDetect)
        {
            this.checkDetect = false;
            this.animator.SetTrigger(Random.Range(0, 2) == 0 ? "Detect1" : "Detect2");
            transform.LookAt(new Vector3(this.CurInfoScanTarget.GetCenterPoint().position.x, 
                transform.position.y, this.CurInfoScanTarget.GetCenterPoint().position.z));

            this.PlayDetectSound();
        }
        if (this.CurInfoScanTarget == null)
        {
            this.checkDetect = true;
        }
    }


    public void DropWeapon()
    {
        Transform weapon = this.enemyDealDamageCtrl.DealDamageBox.transform;
        if (weapon)
        {
            this.enemyDealDamageCtrl.DealDamageBox.enabled = false;
            weapon.GetComponent<Collider>().enabled = true;
            weapon.GetComponent<Collider>().isTrigger = false;
            weapon.AddComponent<Rigidbody>();
            weapon.SetParent(null);
            weapon.gameObject.layer = LayerMask.NameToLayer("TerrainOnly");
        }
    }

    public virtual void ResetEnemy()
    {
        if (!this.enemyHealth.IsDeath()) return;

        this.enemyHealth.ResetHealth();
        this.enemyDebuffs.ResetDebuffs();

        this.navMeshAgent.enabled = true;
        this.detectTarget.enabled = true;
        this.ragdollCtrl.DisableRagdoll();
        this.enemyDealDamageCtrl.DealDamageBox.ResetWeapon();
        this.enemyAiCtrl.EnemySM.ChangeState(EnemyStateId.Idle);
        this.fxBlood.gameObject.SetActive(false);
    }

    public virtual void SetDeath()
    {
        this.navMeshAgent.enabled = false;
        this.detectTarget.enabled = false;
        this.ragdollCtrl.EnableRagdoll();
        this.DropWeapon();
        this.enemyAiCtrl.EnemySM.ChangeState(EnemyStateId.Death);
        this.fxBlood.gameObject.SetActive(true);

        this.PlayDeathSound();
    }

    public virtual void PlayDetectSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DETECT_JAYHI);
        }
    }

    public virtual void PlayDeathSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DIE_DIE01);
        }
    }
}