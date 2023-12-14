using UnityEngine;

public class PlayerManager : PlayerAbstract
{
    private void Update()
    {
        if (this.playerCtrl.Character == null) return;

        this.playerCtrl.PlayerInput.HandleUpdateAllInput();
        this.playerCtrl.PlayerLocomotion.HanldeUpdateAllMovement();
        this.playerCtrl.PlayerCamera.HandleUpdateCamera();
        this.playerCtrl.PlayerAim.HandleUpdateAim();
        this.playerCtrl.PlayerCombatAction.HandleUpdateCombarAction();

        this.playerCtrl.PlayerWeapon.PlayerWeaponManager.HandleUpdateWeaponManager();
        this.playerCtrl.PlayerWeapon.PlayerWeaponReload.HanldeUpdateWeaponReload();
        this.playerCtrl.PlayerWeapon.PlayerWeaponAttack.HandleUpdateWeaponAttack();

        if (this.playerCtrl.PlayerWeapon.PlayerWeaponActive != null)
        {
            this.playerCtrl.PlayerWeapon.PlayerWeaponActive.HandleUpdateFiring();
        }
    }

    private void LateUpdate()
    {
        if (this.playerCtrl.Character == null) return;

    }

    private void FixedUpdate()
    {
        if (this.playerCtrl.Character == null) return;

        this.playerCtrl.PlayerLocomotion.HanldeAllMovementFix();
        //this.playerCtrl.PlayerWeapon.PlayerWeaponActive.HandleFiring();
    }
}
