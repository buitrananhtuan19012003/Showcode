using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerShotRaycast : WeaponRaycast
{
    private float timedelta;
    public override void FireBullet(Vector3 target)
    {
        base.FireBullet(target);
        if (Weapon.WeaponData.ShotGunType == ShotGunType.Slowhand) return;
        this.isDelay = true;
    }

    public override void DelayPerShot()
    {
        if (Weapon.WeaponData.ShotGunType == ShotGunType.Slowhand) return;
        timedelta += Time.deltaTime;
        if (timedelta < Weapon.WeaponData.TimeDelayPerShot) return;
        timedelta = 0;
        isDelay = false;
    }



}
