using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAttack : PlayerWeaponAbstract
{
    private int comboCounter;
    private float lastClicked;
    private string MeleeCombatType;
    private AnimatorStateInfo state;
    private float delay;
    public bool IsAttack;

    public void HandleUpdateWeaponAttack()
    {
        this.SetTyleStringMelee();
        this.ResetComboState();
        //Debug.Log(MeleeCombatType);
    }

    public void Attack()
    {
        if (MeleeCombatType == "Unarmed_") delay = 0.4f;
        if (MeleeCombatType == "LongMelee_") delay = 0.7f;
        if (MeleeCombatType == "Knife_") delay = 0.5f;
        //Debug.Log(delay);
        if (Time.time - lastClicked < delay) return;
        this.IsAttack = true;
        if (this.comboCounter >= 3)
            this.comboCounter = 0;
        this.comboCounter++;

        this.comboCounter = Mathf.Clamp(this.comboCounter, 0, 3);
        this.lastClicked = Time.time;
        for (int i = 0; i < this.comboCounter; i++)
        {
            if (i == 0)
            {
                if (this.comboCounter == 1 /* this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement")*/)
                {
                    this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack", true);
                    this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + (i + 1), true);
                }
            }
            else
            {
                if (this.comboCounter >= (i + 1)  /*this.animator.GetCurrentAnimatorStateInfo(0).IsName(comboName + "_" + i)*/)
                {
                    this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + (i + 1), true);
                    this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + i, true);
                }
            }
        }
    }

    private void ResetComboState()
    {
        WeaponRaycast raycastWeapon = PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        if (raycastWeapon && !PlayerWeapon.PlayerWeaponManager.IsHolstering)
        {
            this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack", false);
            this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + 1, false);
            this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + 2, false);
            this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + MeleeCombatType + 3, false);
            return;
        }

        /* if (PlayerWeapon.PlayerCtrl.PlayerInput.MovementInput != Vector2.zero || PlayerWeapon.PlayerCtrl.PlayerLocomotion.IsJumping) // isMove or Jump 
         {
             PlayerWeapon.PlayerCtrl.Animator.SetLayerWeight(1, 0);
             PlayerWeapon.PlayerCtrl.Animator.SetLayerWeight(2, 1);
             state = this.PlayerWeapon.PlayerCtrl.Animator.GetCurrentAnimatorStateInfo(2);
         }
         else
         {
             PlayerWeapon.PlayerCtrl.Animator.SetLayerWeight(1, 1);
             PlayerWeapon.PlayerCtrl.Animator.SetLayerWeight(2, 0);
             state = this.PlayerWeapon.PlayerCtrl.Animator.GetCurrentAnimatorStateInfo(1);
         }*/
        state = this.PlayerWeapon.PlayerCtrl.Animator.GetCurrentAnimatorStateInfo(1);
        if (!state.IsTag("Attack")) return;
        string name = "Combo";
        for (int i = 0; i < 3; i++)
        {
            if (state.IsName(name + (i + 1)))
            {
                if (state.normalizedTime >= 0.9)
                {
                    this.comboCounter = 0;
                    this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack", false);
                    this.IsAttack = false;

                    for (int j = i + 1; j > 0; j--)
                    {
                        this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + "Unarmed_" + j, false);
                        this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + "LongMelee_" + j, false);
                        this.PlayerWeapon.PlayerCtrl.Animator.SetBool("Attack" + "Knife_" + j, false);
                    }
                }
            }
        }
    }

    private void SetTyleStringMelee()
    {
        Weapon weapon = PlayerWeapon.PlayerWeaponManager.GetActiveWeapon();
        WeaponRaycast gun = PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        if (gun != null && !PlayerWeapon.PlayerWeaponManager.IsHolstering) return;
        if (weapon == null || weapon && PlayerWeapon.PlayerWeaponManager.IsHolstering)
        {
            MeleeCombatType = "Unarmed" + "_";
        }
        else if (weapon && weapon.WeaponData.WeaponType == WeaponType.Melee && !PlayerWeapon.PlayerWeaponManager.IsHolstering)
        {
            MeleeCombatType = weapon.WeaponData.MeleeType.ToString() + "_";
        }
    }

}
