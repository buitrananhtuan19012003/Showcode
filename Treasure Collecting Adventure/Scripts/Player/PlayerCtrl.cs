using UnityEngine;

public class PlayerCtrl : BaseManager<PlayerCtrl>
{
    public PlayerManager PlayerManager;
    public PlayerInput PlayerInput;
    public PlayerLocomotion PlayerLocomotion;
    public PlayerCamera PlayerCamera;
    public PlayerAnimation PlayerAnimation;
    public PlayerWeapon PlayerWeapon;
    public PlayerAim PlayerAim;
    public PlayerInteract PlayerInteract;
    public PlayerInfoScanner PlayerInfoScanner;
    public PlayerCombatAction PlayerCombatAction;
    public PlayerHealth PlayerHealth;
    public PlayerCharacter PlayerCharacter;
    public PlayerCoin PlayerCoin;

    [Header("TARGET PLAYER")]
    public Character Character;
    public Transform PlayerTransform;
    public CharacterController CharacterController;
    public Animator Animator;
    public Animator RigAnimator;

    //protected override void LoadComponent()
    //{
    //    base.LoadComponent();

    //    this.LoadPlayerManager();
    //    this.LoadPlayerInput();
    //    this.LoadPlayerLocomotion();
    //    this.LoadPlayerCamera();
    //    this.LoadPlayerAnimation();
    //    this.LoadPlayerWeapon();
    //    this.LoadPlayerAim();
    //    this.LoadPlayerInteract();
    //    this.LoadPlayerInfoScanner();
    //    this.LoadPlayerCombat();
    //    this.LoadPlayerHealth();
    //    this.LoadPlayerCharacter();
    //    this.LoadPlayerCoin();

    //    this.LoadPlayerTransform();
    //    this.LoadCharacterController();
    //    this.LoadAnimator();
    //    this.LoadRigAnimator();

    //    this.PlayerCamera.SetCameraTarget();
    //    if (this.Character != null)
    //    {
    //        this.PlayerWeapon.PlayerWeaponManager.WeaponSheathSlots = this.Character.WeaponSheathSlots;
    //    }
    //}

    private void OnDisable()
    {
        this.PlayerLocomotion.SeOnDisableLocomation();
    }

    protected virtual void LoadPlayerManager()
    {
        if (this.PlayerManager == null)
        {
            this.PlayerManager = GetComponentInChildren<PlayerManager>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerManager", gameObject);
        }
    }
    protected virtual void LoadPlayerInput()
    {
        if (this.PlayerInput == null)
        {
            this.PlayerInput = GetComponentInChildren<PlayerInput>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerInput", gameObject);
        }
    }
    protected virtual void LoadPlayerLocomotion()
    {
        if (this.PlayerLocomotion == null)
        {
            this.PlayerLocomotion = GetComponentInChildren<PlayerLocomotion>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerLocomotion", gameObject);
        }
    }
    protected virtual void LoadPlayerCamera()
    {
        if (this.PlayerCamera == null)
        {
            this.PlayerCamera = GetComponentInChildren<PlayerCamera>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerCamera", gameObject);
        }
    }
    protected virtual void LoadPlayerAnimation()
    {
        if (this.PlayerAnimation == null)
        {
            this.PlayerAnimation = GetComponentInChildren<PlayerAnimation>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerAnimation", gameObject);
        }
    }
    protected virtual void LoadPlayerWeapon()
    {
        if (this.PlayerWeapon == null)
        {
            this.PlayerWeapon = GetComponentInChildren<PlayerWeapon>();
            Debug.LogWarning(gameObject.name + ": LoadPLayerWeapon", gameObject);
        }
    }
    protected virtual void LoadPlayerAim()
    {
        if (this.PlayerAim == null)
        {
            this.PlayerAim = GetComponentInChildren<PlayerAim>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerAim", gameObject);
        }
    }
    protected virtual void LoadPlayerInteract()
    {
        if (this.PlayerInteract == null)
        {
            this.PlayerInteract = GetComponentInChildren<PlayerInteract>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerInteract", gameObject);
        }
    }
    protected virtual void LoadPlayerInfoScanner()
    {
        if (this.PlayerInfoScanner == null)
        {
            this.PlayerInfoScanner = GetComponentInChildren<PlayerInfoScanner>();
            Debug.LogWarning(gameObject.name + "LoadPlayerInfoScanner", gameObject);
        }
    }
    protected virtual void LoadPlayerCombat()
    {
        if (this.PlayerCombatAction == null)
        {
            this.PlayerCombatAction = GetComponentInChildren<PlayerCombatAction>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerCombat", gameObject);
        }
    }
    protected virtual void LoadPlayerHealth()
    {
        if (this.PlayerHealth == null)
        {
            this.PlayerHealth = GetComponentInChildren<PlayerHealth>();
            Debug.LogWarning(gameObject.name + "LoadPlayerHealth", gameObject);
        }
    }
    protected virtual void LoadPlayerCharacter()
    {
        if (this.PlayerCharacter == null)
        {
            this.PlayerCharacter = GetComponentInChildren<PlayerCharacter>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerCharacter", gameObject);
        }
    }

    protected virtual void LoadPlayerCoin()
    {
        if (this.PlayerCoin == null)
        {
            this.PlayerCoin = GetComponentInChildren<PlayerCoin>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerCoin", gameObject);
        }
    }

    protected virtual void LoadPlayerTransform()
    {
        if (this.Character != null)
        {
            this.PlayerTransform = this.Character.CharacterTransform;
            Debug.LogWarning(gameObject.name + ": LoadPlayerTransform", gameObject);
        }
    }
    protected virtual void LoadCharacterController()
    {
        if (this.Character != null)
        {
            this.CharacterController = this.Character.CharacterController;
            Debug.LogWarning(gameObject.name + ": LoadCharacterController", gameObject);
        }
    }
    protected virtual void LoadAnimator()
    {
        if (this.Character != null)
        {
            this.Animator = this.Character.Animator;
            Debug.LogWarning(gameObject.name + ": LoadAnimator", gameObject);
        }
    }
    protected virtual void LoadRigAnimator()
    {
        if (this.Character != null)
        {
            this.RigAnimator = this.Character.RigAnimator;
            Debug.LogWarning(gameObject.name + ": LoadRigAnimator", gameObject);
        }
    }

    public void SetCharacter(Character character)
    {
        this.Character = character;

        this.LoadPlayerTransform();
        this.LoadCharacterController();
        this.LoadAnimator();
        this.LoadRigAnimator();
        this.SetUI();

        //this.PlayerInput.SetInput();
        this.PlayerLocomotion.SetOnEventAnimator();
        this.PlayerCamera.SetCameraTarget();
        this.PlayerWeapon.PlayerWeaponManager.WeaponSheathSlots = character.WeaponSheathSlots;
        this.PlayerWeapon.PlayerWeaponManager.LoadWeapon();

        this.Character.TakeDamageCtrl.SetHealthObject(this.PlayerHealth.gameObject);
    }

    public void SetUI()
    {
        if (this.Character.CharacterData == null) return;

        if (UIManager.HasInstance)
        {
            //UIManager.Instance.InGamePanel.AlwaysOnUI.UI_Skill.SetSkill(this.Character.CharacterData.SpecialSkillIcon);
        }
    }

    public void ResetPlayer()
    {
        gameObject.SetActive(false);
        this.Character = null;
        this.PlayerTransform = null;
        this.CharacterController = null;
        this.Animator = null;
        this.RigAnimator = null;
    }

    public void ResetBool()
    {
        this.PlayerAim.IsAim = false;
        this.PlayerWeapon.PlayerWeaponReload.IsReload = false;
        this.PlayerCombatAction.SetActionMouseLeft(CombatAction.None);
        this.PlayerCombatAction.SetActionMouseRight(CombatAction.None);
    }
}