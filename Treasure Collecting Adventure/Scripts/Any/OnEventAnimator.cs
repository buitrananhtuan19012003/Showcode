using UnityEngine;

public class OnEventAnimator : MonoBehaviour
{
    public delegate void AnimatorMoveDelegate();
    public event AnimatorMoveDelegate OnAnimatorMoveEvent;

    private void OnAnimatorMove()
    {
        if(OnAnimatorMoveEvent != null)
        {
            OnAnimatorMoveEvent();
        }
    }
}
