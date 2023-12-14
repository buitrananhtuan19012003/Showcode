using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAbstract : SaiMonoBehaviour
{
    public PlayerWeapon PlayerWeapon;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerWeapon();
    }

    protected virtual void LoadPlayerWeapon()
    {
        if (this.PlayerWeapon == null)
        {
            this.PlayerWeapon = transform.parent.GetComponent<PlayerWeapon>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerWeapon", gameObject);
        }
    }
}
