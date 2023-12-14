using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

//public class AnimationEvent : UnityEvent<string>
//{

//}
public class PlayerRigAnimationEvents : MonoBehaviour
{
    public AnimationEvent AnimationEvent = new AnimationEvent();
    public void OnAnimationEvent(string eventName)
    {
        //AnimationEvent.Invoke(eventName);
    }

    public void DetachMagazine()
    {
        PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponReload.DetachMagazine();
    }

    public void DropMagazine()
    {
        PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponReload.DropMagazine();
    }

    public void RefillMagazine()
    {
        PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponReload.RefillMagazine();
    }

    public void  AttachMagazine()
    {
        PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponReload.AttachMagazine();
    }

    public void Exitdelay_ShotgunSlowhand()
    {
        PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponReload.ChangeIsReload();
    }
}