using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRigAttach : SaiMonoBehaviour
{
    //[SerializeField] private Transform aimLookMainFake;
    [SerializeField] private Transform leftHand;
    //[SerializeField] private MultiAimConstraint aimSpine1;
    //[SerializeField] private MultiAimConstraint aimSpine2;
    //[SerializeField] private MultiAimConstraint aimHead;
    //[SerializeField] private MultiAimConstraint weaponPose;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        //if (this.aimLookMainFake == null)
        //    this.aimLookMainFake = transform.Find("AimLookMainFake");

        if (this.leftHand == null)
            this.leftHand = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L");

        //if (this.aimSpine1 == null)
        //    this.aimSpine1 = transform.Find("------RigLayers-----/RigLayer_BodyAim/AimSpine1").GetComponent<MultiAimConstraint>();

        //if (this.aimSpine2 == null)
        //    this.aimSpine2 = transform.Find("------RigLayers-----/RigLayer_BodyAim/AimSpine2").GetComponent<MultiAimConstraint>();

        //if (this.aimHead == null)
        //    this.aimHead = transform.Find("------RigLayers-----/RigLayer_BodyAim/AimHead").GetComponent<MultiAimConstraint>();

        //if (this.weaponPose == null)
        //    this.weaponPose = transform.Find("------RigLayers-----/RigLayer_WeaponAim/WeaponPose").GetComponent<MultiAimConstraint>();
    }

    public void SetRig()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PlayerCtrl.PlayerWeapon.PlayerWeaponReload.LeftHand = this.leftHand;
        }

        //this.aimLookMainFake.parent = null;
    }

    private void FixedUpdate()
    {
        //    if (GameManager.HasInstance)
        //    {
        //        if (GameManager.Instance.PlayerCtrl.Character != null)
        //        {
        //            this.aimLookMainFake.transform.localPosition = GameManager.Instance.PlayerCtrl.PlayerAim.AimLookMain.transform.position;
        //        }
        //    }
    }
}
