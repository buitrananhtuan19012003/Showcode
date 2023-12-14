using UnityEngine;

public class PlayerWeapon : PlayerAbstract
{
    public PlayerWeaponManager PlayerWeaponManager;
    public PlayerWeaponActive PlayerWeaponActive;
    public PlayerWeaponReload PlayerWeaponReload;
    public PlayerWeaponAttack PlayerWeaponAttack;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerWeaponManager();
        this.LoadPlayerWeaponActive();
        this.LoadPlayerWeaponReload();
        this.LoadPlayerWeaponAttack();
    }

    protected virtual void LoadPlayerWeaponManager()
    {
        if (this.PlayerWeaponManager == null)
        {
            this.PlayerWeaponManager = GetComponentInChildren<PlayerWeaponManager>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerWeaponManager", gameObject);
        }
    }
    protected virtual void LoadPlayerWeaponActive()
    {
        if (this.PlayerWeaponActive == null)
        {
            this.PlayerWeaponActive = GetComponentInChildren<PlayerWeaponActive>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerWeaponActive", gameObject);
        }
    }
    protected virtual void LoadPlayerWeaponReload()
    {
        if (this.PlayerWeaponReload == null)
        {
            this.PlayerWeaponReload = GetComponentInChildren<PlayerWeaponReload>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerWeaponReload", gameObject);
        }
    }
    protected virtual void LoadPlayerWeaponAttack()
    {
        if (this.PlayerWeaponAttack == null)
        {
            this.PlayerWeaponAttack = GetComponentInChildren<PlayerWeaponAttack>();
            Debug.LogWarning(gameObject.name + ": LoadPlayerWeaponAttack", gameObject);
        }
    }
}
