using System.Collections.Generic;
using UnityEngine;

public class DealDamageCtrl : SaiMonoBehaviour
{
    [SerializeField] protected DealDamageBox dealDamageUnarmed;
    [SerializeField] protected List<DealDamageBox> listDealDamageMelee = new List<DealDamageBox>();

    public List<DealDamageBox> ListDealDamageMelee { get => this.listDealDamageMelee; set => this.listDealDamageMelee = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.dealDamageUnarmed == null)
            this.dealDamageUnarmed = GetComponentInChildren<DealDamageBox>();
        //this.dealDamageBox = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L/UnarmedFist_L").GetComponent<DealDamageBox>();

    }

    public void EnableDealDamageCollider(int isEnable)
    {
        if (!PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponManager.IsHolstering)
        {
            Weapon melee = PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponManager.GetActiveWeapon();
            if (melee && melee.WeaponData.WeaponType == WeaponType.Melee)
            {
                this.dealDamageUnarmed.gameObject.SetActive(false);

                for (int i = 0; i < this.listDealDamageMelee.Count; i++)
                {
                    if (this.listDealDamageMelee[i] == null)
                    {
                        this.listDealDamageMelee.RemoveAt(i);
                        i--;
                    }
                }

                if (isEnable == 1)
                {
                    foreach (var weapon in this.listDealDamageMelee)
                    {
                        weapon.SetActiveDeal(true);
                    }
                    return;
                }
                else
                {
                    foreach (var weapon in this.listDealDamageMelee)
                    {
                        weapon.SetActiveDeal(false);
                    }
                    return;
                }
            }
        }

        this.dealDamageUnarmed.gameObject.SetActive(true);

        if (isEnable == 1)
        {
            this.dealDamageUnarmed.SetActiveDeal(true);
        }
        else
        {
            this.dealDamageUnarmed.SetActiveDeal(false);
        }
    }
}
