using UnityEngine;

public class PlayerWeaponReload : PlayerWeaponAbstract
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private GameObject magazineHand;

    private bool isReload;

    public bool IsReload { get => this.isReload; set => this.isReload = value; }
    public Transform LeftHand { get => this.leftHand; set => this.leftHand = value; }

    private void Start()
    {
       this.AddAnimationEvenReload();
    }

    public void AddAnimationEvenReload()
    {
        if (this.PlayerWeapon.PlayerCtrl.Character == null) return;

        PlayerRigAnimationEvents animationEvents = this.PlayerWeapon.PlayerCtrl.RigAnimator.GetComponent<PlayerRigAnimationEvents>();
        if (animationEvents != null)
        {
            //animationEvents.AnimationEvent.AddListener(OnAnimationEvent);
        }
    }

    public void HanldeUpdateWeaponReload()
    {
        SetReloadWeapon(false);
    }

    public void SetReloadWeapon(bool button)
    {
        WeaponRaycast weapon = PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        if (weapon)
        {
            if (!button && weapon.currentAmmo <= 0)
            {
                if(weapon.Weapon.WeaponData.WeaponType == WeaponType.Pistol)
                {
                    this.PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_Pistol");
                }
                else
                {
                    this.PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_weapon");
                }
                isReload = true;
            }
            if (button)
            {
                if (weapon.Weapon.WeaponData.WeaponType == WeaponType.Pistol)
                {
                    this.PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_Pistol");
                }
                else
                {
                    this.PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_weapon");
                }
                isReload = true;
            }
        }
    }

    public void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magazine":
                DetachMagazine();
                break;
            case "drop_magazine":
                DropMagazine();
                break;
            case "refill_magazine":
                RefillMagazine();
                break;
            case "attach_magazine":
                Debug.Log("ANIMATIONEVENT");
                AttachMagazine();
                break;
            case "exitdelay_shotgun":
                this.ChangeIsReload();
                break;
        }
    }

    public void DetachMagazine()
    {
        WeaponRaycast weapon = this.PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        magazineHand.transform.localPosition = Vector3.zero;
        weapon.magazine.SetActive(false);
    }

    public void DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, leftHand.transform.position, leftHand.transform.rotation);
        droppedMagazine.transform.localScale = Vector3.one;
        droppedMagazine.AddComponent<Rigidbody>();
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }

    public void RefillMagazine()
    {
        magazineHand.SetActive(true);
    }

    public void AttachMagazine()
    {
        WeaponRaycast weapon = this.PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);
        weapon.currentAmmo = weapon.maxAmmo;
        if (weapon.Weapon.WeaponData.WeaponType == WeaponType.Pistol)
        {
            this.PlayerWeapon.PlayerCtrl.RigAnimator.ResetTrigger("reload_Pistol");
        }
        else
        {
            this.PlayerWeapon.PlayerCtrl.RigAnimator.ResetTrigger("reload_weapon");
        }
        Invoke("ChangeIsReload", 0.15f);
    }

    public void ChangeIsReload()
    {
        isReload = false;
    }

    public void DelayReloadPerShot()
    {
        PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_pershot");
        isReload = true;
    }

}
