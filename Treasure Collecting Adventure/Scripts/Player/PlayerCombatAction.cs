using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatAction : PlayerAbstract
{
    private CombatAction combatActionMouseL;
    private CombatAction combatActionMouseR;


    private void Update()
    {
        Debug.Log(this.combatActionMouseL.ToString());
    }
    public void HandleUpdateCombarAction()
    {
        SetConditionMouseL();
        SetConditionMouseR();
    }

    public void ActionMouseL()
    {
        switch (this.combatActionMouseL)
        {
            case CombatAction.None:
                break;
            case CombatAction.RangedWeapon:
                this.playerCtrl.PlayerWeapon.PlayerWeaponActive.IsFiring = true;
                break;

            case CombatAction.MeleeWeapon:
                this.playerCtrl.PlayerWeapon.PlayerWeaponAttack.Attack();
                this.playerCtrl.PlayerInput.AttackInput = false;
                break;

            case CombatAction.CharacterSpecific:
                Character character = this.playerCtrl.Character;
                if (character)
                {
                    character.ActionMouseL();
                    this.playerCtrl.PlayerInput.AttackInput = false;
                }
                break;

            default:
                //Debug.LogError("Not set combat action for Mouse-Left");
                break;
        }
    }

    public void ActionMouseR(bool useButton, bool InputButton)
    {
        if (useButton)
        {
            switch (this.combatActionMouseR)
            {
                case CombatAction.None:
                    break;
                case CombatAction.RangedWeapon:
                    playerCtrl.PlayerAim.SetIsAim(InputButton);
                    break;

                case CombatAction.MeleeWeapon:

                    break;

                case CombatAction.CharacterSpecific:
                    Character character = this.playerCtrl.Character;
                    if (character)
                    {
                        character.ActionMouseR(InputButton);
                    }
                    break;

                default:
                    //Debug.LogError("Not set combat action for Mouse-Right");
                    break;
            }
        }
        else
        {

        }

    }

    public void SpecialSkill()
    {
        Character character = this.playerCtrl.Character;
        if (character)
        {
            character.SpecialSkill();
        }
    }

    public void BattleSkill()
    {
        Character character = this.playerCtrl.Character;
        if (character)
        {
            character.BattleSkill();
        }
    }

    public void SetActionMouseLeft(CombatAction combatAction)
    {
        this.combatActionMouseL = combatAction;
    }

    public void SetActionMouseRight(CombatAction combatAction)
    {
        this.combatActionMouseR = combatAction;
    }

    public void SetConditionMouseL()
    {
        if (combatActionMouseL != CombatAction.CharacterSpecific)
        {
            // Weapon weapon = playerCtrl.PlayerWeapon.PlayerWeaponManager.GetActiveWeapon();
            WeaponRaycast gun = playerCtrl.PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
            if (gun && !playerCtrl.PlayerWeapon.PlayerWeaponManager.IsHolstering)
            {
                this.combatActionMouseL = CombatAction.RangedWeapon;
                this.combatActionMouseR = CombatAction.RangedWeapon;
            }
            if (gun == null || gun != null && playerCtrl.PlayerWeapon.PlayerWeaponManager.IsHolstering)
            {
                Debug.Log("melee");
                this.combatActionMouseL = CombatAction.MeleeWeapon;
                this.combatActionMouseR = CombatAction.MeleeWeapon;
            }
        }
    }

    public void SetConditionMouseR()
    {
        if (combatActionMouseR == CombatAction.CharacterSpecific) return;
        WeaponRaycast gun = playerCtrl.PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        if (gun && !playerCtrl.PlayerWeapon.PlayerWeaponManager.IsHolstering)
        {
            this.combatActionMouseR = CombatAction.RangedWeapon;
        }
        else
        {
            this.combatActionMouseR = CombatAction.None;
        }

    }
}
