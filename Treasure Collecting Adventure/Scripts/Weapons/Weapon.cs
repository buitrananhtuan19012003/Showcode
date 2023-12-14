using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;
    [SerializeField] protected WeaponSlot[] weaponSlot;

    public WeaponSlot[] WeaponSlot { get => this.weaponSlot; set => this.weaponSlot = value; }
    public WeaponDataSO WeaponData { get => this.weaponData; protected set { this.weaponData = value; } }

    protected virtual void LeftMouseAction()
    {
        //for override
    }

    protected virtual void RightMouseAction()
    {
        //for override
    }
}
