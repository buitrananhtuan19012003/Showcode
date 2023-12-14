using UnityEngine;

public class PickupWeapons : PickupObject
{
    protected Weapon weaponPrefab;

    public Weapon Weapon { get => this.weaponPrefab; }

    public void Pickup(PlayerWeaponManager weaponManager)
    {
        if (weaponManager)
        {
            bool canAdd = weaponManager.AddWeapon(this.weaponPrefab);
            if (canAdd)
            {
                /*PlayerWeapon playerWeapon = weaponManager.GetComponentInParent<PlayerWeapon>();
                playerWeapon.PlayerCtrl.Rigcontroller.Play("equip_laser");*/
                Destroy(gameObject);
            }
        }
    }

    public void SetWeapon(Weapon weapon) //Spawner Call
    {
        this.weaponPrefab = weapon;
        transform.name = this.weaponPrefab.WeaponData.ItemName;

        this.GenerateModelObject();
    }

    public void SetWeapon(GameObject weaponObject)
    {
        this.weaponPrefab = weaponObject.GetComponent<Weapon>();
        transform.name = this.weaponPrefab.WeaponData.ItemName;

        if (this.weaponPrefab == null) return;

        this.weaponPrefab.transform.SetParent(this.virtualObject.transform);
        this.weaponPrefab.transform.localPosition = Vector3.zero;
        this.weaponPrefab.transform.localRotation = Quaternion.identity;
        this.weaponPrefab.transform.localScale = Vector3.one;

        Collider collider = this.weaponPrefab.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        this.weaponPrefab.gameObject.SetActive(true);
    }

    protected override void GenerateModelObject()
    {
        base.GenerateModelObject();
        if (this.weaponPrefab == null) return;

        GameObject model = Instantiate(weaponPrefab.WeaponData.Model);
        model.transform.SetParent(this.virtualObject.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;
        model.transform.localScale = Vector3.one;

        Collider collider = model.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
}


