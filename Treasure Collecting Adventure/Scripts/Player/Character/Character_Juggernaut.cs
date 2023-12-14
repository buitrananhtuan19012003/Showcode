using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering;

public class Character_Juggernaut : Character
{
    [Header("JUGGERNAUT")]
    public int TitanID;
    //public float transformTime;
    [SerializeField] private GameObject Gun_JuggernautModel;
    [SerializeField] private WeaponRaycast Juggernaut_Raycast;
    [SerializeField] private Transform raycastOriginHand;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private ParticleSystem EnergyGlobe;
    [SerializeField] private ParticleSystem EnergyGlobeHand;
    [SerializeField] private ParticleSystem BomExplosion;
    [SerializeField] private Volume Volume;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LineRenderer lineHand;

    private LineRenderer lineMain;
    private float xScaleEnergyGl;
    private float yScaleEnergyGl;
    private float zScaleEnergyGl;

    private float timesetExplosion;
    private float timecharging;
    RaycastHit Hit;
    Vector3 HitPositionLine;
    bool Hit_Bool;
    bool isCharging;
    private int levelSkill;
    private float damageRangeExplosion;
    private int numShot;
    [Header("STAT")]
    [SerializeField] private int maxNumShot = 1;
    [SerializeField] private int powerSkill = 30;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.Volume == null)
            this.Volume = GetComponentInChildren<Volume>();
    }

    protected override void Awake()
    {
        base.Awake();
        this.line.enabled = false;
        this.lineHand.enabled = false;
        this.Volume.enabled = false;
        this.Juggernaut_Raycast = Gun_JuggernautModel.GetComponentInParent<WeaponRaycast>();
        this.Gun_JuggernautModel.SetActive(false);
        this.numShot = this.maxNumShot;
    }

    protected override void Update()
    {
        base.Update();

        //ExecutionPhase
        if (!this.isReadySpecialSkill && !this.isCoolingDownSpecicalSkill)
        {
            if (/*Juggernaut_Raycast.currentAmmo <= 0*/numShot <= 0)
            {
                //StopCoroutine(TransformationCoroutine());
                Invoke("RevertoForm", 0.1f);
                this.isCoolingDownSpecicalSkill = true;
            }
        }
        this.Juggernaut_Raycast.UpdateBullets();

        if (!IsSpecialSkill || numShot <= 0) return;
        this.HandleSkill();
        //
        this.HandleAfterFire();
        this.HandleRender();
        this.HandleChargingEnergy();

    }

    public override void SpecialSkill()
    {
        base.SpecialSkill();
        if (this.isReadySpecialSkill)
        {
            this.Transform();
            Invoke("RevertoForm", this.characterData.ExecutionSkillTime);
        }

    }
    public void Transform()
    {
        this.numShot = this.maxNumShot;
        this.isSpecialSkill = true;
        this.isReadySpecialSkill = false;
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.CharacterSpecific);
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseRight(CombatAction.CharacterSpecific);
        this.rigAnimator.SetBool("transform", true);
    }

    public void RevertoForm()
    {
        CancelInvoke("RevertoForm");
        this.isSpecialSkill = false;
        this.isCoolingDownSpecicalSkill = true;
        this.SetOriginalScaleParticle();
        Debug.Log("ErrorJuggerNaut");
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.None);
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseRight(CombatAction.None);
        this.RigAnimator.SetBool("transform", false);
        PlayerCtrl.Instance.PlayerCamera.MainCamera.cullingMask |= 1 << 6;
        this.Gun_JuggernautModel.gameObject.SetActive(false);
    }

    private void HandleSkill()
    {
        if (/*PlayerCtrl.Instance.PlayerInput.Mouse0_ButtonDown*/Input.GetMouseButtonDown(0))
        {
            if (PlayerCtrl.Instance.PlayerCamera.FPSCamera.gameObject.activeInHierarchy == true)
            {
                EnergyGlobe.Play();
            }
            else
            {
                EnergyGlobeHand.Play();
            }
            timesetExplosion = Time.time;
        }
        if (/*PlayerCtrl.Instance.PlayerInput.Mouse0_GetButton*/Input.GetMouseButton(0))
        {
            //if (UIManager.HasInstance)
            //{
            //    UI_ChargeSlider chargeSlider = UIManager.Instance.InGamePanel.AlwaysOnUI.UI_ChargeSlider;
            //    chargeSlider.Show(null);
            //    chargeSlider.SetSlider(this.timecharging, 12);
            //}

            ChargingEnergy();
        }

        if (/*PlayerCtrl.Instance.PlayerInput.Mouse0_ButtonUp*/Input.GetMouseButtonUp(0) /*|| timecharging >= (this.characterData.ExecutionSkillTime - 5)*/)
        {
            Fire();
            if (UIManager.HasInstance)
            {
                //UIManager.Instance.InGamePanel.AlwaysOnUI.UI_ChargeSlider.Hide();
            }
            Invoke("DeactiveLine", 0.1f);
        }
    }

    public void Fire()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySeStop(AUDIO.SE_CHARGING_JUGGERNAUT);
            AudioManager.Instance.PlaySE(AUDIO.SE_FIRE_LASER_JUGGERNAUT);
        }
        this.numShot--;
        this.Juggernaut_Raycast.FireBullet(PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponActive.CrosshairTarget.position);
        this.lineMain.enabled = true;
        this.isCharging = false;
        this.Volume.enabled = true;
    }


    private void HandleChargingEnergy()
    {
        if (isCharging)
        {
            PlayerCtrl.Instance.PlayerLocomotion.IsSprinting = false;
            PlayerCtrl.Instance.PlayerLocomotion.IsWalking = true;
            PlayerCtrl.Instance.PlayerInput.InteractInput = false;
        }
        else if (!isCharging)
        {
            this.SetOriginalScaleParticle();
        }
    }

    private void SetOriginalScaleParticle()
    {
        xScaleEnergyGl = 0;
        yScaleEnergyGl = 0;
        zScaleEnergyGl = 0;
        EnergyGlobe.gameObject.transform.localScale = Vector3.zero;
        EnergyGlobeHand.gameObject.transform.localScale = Vector3.zero;
        if (EnergyGlobe.isPlaying)
        {
            EnergyGlobe.Stop();
            EnergyGlobe.Clear();
        }
        if (EnergyGlobeHand.isPlaying)
        {
            EnergyGlobeHand.Stop();
            EnergyGlobeHand.Clear();
        }
    }

    private void ChargingEnergy()
    {
        timecharging = Time.time - timesetExplosion;
        if (timecharging == 0)
        {
            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySE(AUDIO.SE_CHARGING_JUGGERNAUT);
            }
        }
        isCharging = true;
        if (timecharging >= 0 && timecharging < 3)
        {
            levelSkill = 1;
            damageRangeExplosion = 1.4f;
            BomExplosion.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (timecharging >= 3 && timecharging < 6)
        {
            levelSkill = 2;
            damageRangeExplosion = 4.1f;
            BomExplosion.gameObject.transform.localScale = new Vector3(3, 3, 3);
        }
        else if (timecharging >= 6 && timecharging < 12)
        {
            levelSkill = 3;
            damageRangeExplosion = 6.5f;
            BomExplosion.gameObject.transform.localScale = new Vector3(4.5f, 4.5f, 4.5f);
        }
        else if (timecharging >= 12)
        {
            levelSkill = 4;
            damageRangeExplosion = 10.7f;
            BomExplosion.gameObject.transform.localScale = new Vector3(7f, 7f, 7f);
        }


        //if (timecharging >= 12) return;
        xScaleEnergyGl += (float)0.02 * Time.deltaTime;
        yScaleEnergyGl += (float)0.02 * Time.deltaTime;
        zScaleEnergyGl += (float)0.02 * Time.deltaTime;
        EnergyGlobe.gameObject.transform.localScale = new Vector3(xScaleEnergyGl, yScaleEnergyGl, zScaleEnergyGl);
        EnergyGlobeHand.gameObject.transform.localScale = new Vector3(xScaleEnergyGl * 4, yScaleEnergyGl * 4, zScaleEnergyGl * 4);
    }

    public void EnableBomExplosion()
    {
        float power;
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_SHOCKWAVE_EXPLOSION);
        }
        BomExplosion.gameObject.transform.position = Hit.point;
        BomExplosion.Play();
        if (levelSkill == 1)
        {
            power = this.powerSkill * 1.2f;
            this.powerSkill = Mathf.RoundToInt(power);
        }
        else if (levelSkill == 2)
        {
            power = this.powerSkill * 2f;
            this.powerSkill = Mathf.RoundToInt(power);
            Debug.Log(this.powerSkill);
        }
        else if (levelSkill == 3)
        {
            power = this.powerSkill * 2.8f;
            this.powerSkill = Mathf.RoundToInt(power);
        }
        else if (levelSkill == 4)
        {
            power = this.powerSkill * 3.6f;
            this.powerSkill = Mathf.RoundToInt(power);
        }
        Collider[] colliders = Physics.OverlapSphere(Hit.point, damageRangeExplosion);
        foreach (Collider collider in colliders)
        {
            var Enemy = collider.GetComponent<HitBox>();

            if (Enemy && Enemy.CompareTag("EnemyCollider"))
            {
                Enemy.OnHit(this.powerSkill);
            }
        }
    }
    private void HandleAfterFire()
    {
        Vector3 crossHair = PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponActive.CrosshairTarget.position;
        Vector3 direction = crossHair - Juggernaut_Raycast.raycastOrigin.position;
        Ray ray = new Ray(Juggernaut_Raycast.raycastOrigin.position, direction);
        Hit_Bool = Physics.Raycast(ray, out Hit, 1000f);
        HitPositionLine = Hit_Bool ? Hit.point : /*ifNotHitpoint * 1000f;*/crossHair;
        if (lineMain)
        {
            lineMain.SetPosition(0, Juggernaut_Raycast.raycastOrigin.position);
            lineMain.SetPosition(1, HitPositionLine);
        }
        float distance;
        if (Hit_Bool)
        {
            distance = (Hit.point - transform.position).magnitude;
            if (Input.GetMouseButtonUp(0))
            {
                if (distance < 20f)
                {
                    Invoke("EnableBomExplosion", 0.01f);
                }
                else if (distance >= 20f)
                {
                    Invoke("EnableBomExplosion", 0.2f);
                }
            }
        }
    }

    private void HandleRender()
    {
        if (PlayerCtrl.Instance.PlayerCamera.FPSCamera.gameObject.activeInHierarchy == true)
        {
            PlayerCtrl.Instance.PlayerCamera.MainCamera.cullingMask &= ~(1 << 6);
            this.lineMain = this.line;
            Gun_JuggernautModel.gameObject.SetActive(true);
            this.RigAnimator.SetBool("aim_character", true);
            Juggernaut_Raycast.raycastOrigin = raycastOrigin;
            if (!EnergyGlobe.isPlaying && isCharging)
            {
                EnergyGlobe.Play();
            }
        }
        else
        {
            PlayerCtrl.Instance.PlayerCamera.MainCamera.cullingMask |= 1 << 6;
            this.lineMain = this.lineHand;
            Gun_JuggernautModel.gameObject.SetActive(false);
            this.RigAnimator.SetBool("aim_character", false);
            Juggernaut_Raycast.raycastOrigin = raycastOriginHand;
            if (!EnergyGlobeHand.isPlaying && isCharging)
            {
                EnergyGlobeHand.Play();
            }
        }
    }


    public void DeactiveLine()
    {
        lineMain.enabled = false;
        Volume.enabled = false;
    }
    public override void ActionMouseL()
    {
        base.ActionMouseL();
        //this.Animator.SetTrigger("shoot");
    }

    public override void ActionMouseR(bool inputButton)
    {
        // if (isCharging) return;
        //PlayerCtrl.Instance.PlayerAim.IsAim = inputButton;
    }
}
