using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerAbstract
{
    public float snappedValue;
    public void UpdateValuesAnimation(string name, float value)
    {
        float snappedValue;

        if (value > 0 && value < 0.55f)
        {
            snappedValue = 0.5f;
        }
        else if (value >= 0.55f)
        {
            snappedValue = 1;
        }
        else if (value < 0 && value > -0.55f)
        {
            snappedValue = -0.5f;
        }
        else if (value <= -0.55f)
        {
            snappedValue = -1;
        }
        else
        {
            snappedValue = 0;
        }

        this.playerCtrl.Animator.SetFloat(name, snappedValue, 0.1f, Time.deltaTime);

    }

    public void UpdateAnimatorValuesMoveState(float value, bool isSprinting, bool isWalking)
    {
        // snappedValue;
        if (value > 0 && value < 0.55f)
        {
            snappedValue = 0.5f;
        }
        else if (value >= 0.55f)
        {
            snappedValue = 1;
        }
        else if (value < 0 && value > -0.55f)
        {
            snappedValue = -0.5f;
        }
        else if (value <= -0.55f)
        {
            snappedValue = -1;
        }
        else
        {
            snappedValue = 0;
        }

        if (isSprinting)
        {
            snappedValue = 2;
        }
        if (isWalking)
        {
            snappedValue = 0.5f;
        }
        this.playerCtrl.Animator.SetFloat("MoveState", snappedValue, 0.1f, Time.deltaTime);
    }


}
