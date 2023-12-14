using UnityEngine;

public class PlayerWeaponActive : PlayerWeaponAbstract
{
    private Transform crosshairTarget;
    private WeaponRaycast weaponRaycast;

    public bool IsFiring = false;
    public bool IscanFire;
    public Transform CrosshairTarget { get => this.crosshairTarget; set => this.crosshairTarget = value; }

    public void HandleUpdateFiring()
    {
        weaponRaycast = PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        SetIsCanFire();

        if (weaponRaycast == null) return;
        if (weaponRaycast.Weapon.WeaponData.ShotGunType != ShotGunType.Slowhand)
        {
            weaponRaycast.DelayPerShot();
        }
        if (IscanFire)
        {
            if (Input.GetMouseButtonDown(0) && weaponRaycast.Weapon.WeaponData.WeaponType != WeaponType.AssaultRifle 
                && weaponRaycast.Weapon.WeaponData.ItemName != "Deliverer")
            {
                PlayerWeapon.PlayerCtrl.PlayerLocomotion.IsWalking = true;
                weaponRaycast.FireBullet(crosshairTarget.position);
                if (weaponRaycast.Weapon.WeaponData.ShotGunType == ShotGunType.Slowhand)
                {
                   // DelayReloadPerShot();
                   PlayerWeapon.PlayerWeaponReload.DelayReloadPerShot();
                }
            }

            if (IsFiring && weaponRaycast.Weapon.WeaponData.WeaponType == WeaponType.AssaultRifle || IsFiring && weaponRaycast.Weapon.WeaponData.ItemName == "Deliverer")
            {
                weaponRaycast.UpdateFiring(crosshairTarget.position);
                PlayerWeapon.PlayerCtrl.PlayerLocomotion.IsWalking = true;
            }

            if (!IsFiring && weaponRaycast.Weapon.WeaponData.WeaponType == WeaponType.AssaultRifle ||!IsFiring && weaponRaycast.Weapon.WeaponData.ItemName == "Deliverer")
            {
                weaponRaycast.runtTimeFire = 0;
                weaponRaycast.recoil.ResetIndex();
            }
        }
        weaponRaycast.UpdateBullets();

    }

   /* public void DelayPerShot(float timedelay)
    {
        timedelta += Time.deltaTime;
        if (timedelta < timedelay) return;
        timedelta = 0;
        SetisDelay(false);
    }*/

   /* public void DelayReloadPerShot()
    {
        PlayerWeapon.PlayerCtrl.RigAnimator.SetTrigger("reload_pershot");
        //PlayerWeapon.PlayerWeaponReload.IsReload = true;
        timedelta = 0;
        SetisDelay(true);
    }*/

    public void SetIsCanFire()
    {
        if(weaponRaycast == null) return;
        if (!PlayerWeapon.PlayerWeaponManager.IsHolstering && !PlayerWeapon.PlayerWeaponReload.IsReload && !weaponRaycast.isDelay)
        {
            IscanFire = true;
        }
        else
        {
            IscanFire = false;
        }
    }
    public void SetisDelay(bool isDelay)
    {
        if (weaponRaycast)
        {
            weaponRaycast.isDelay = isDelay;
        }
    }
}
