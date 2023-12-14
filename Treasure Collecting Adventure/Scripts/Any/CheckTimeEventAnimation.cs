using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTimeEventAnimation : MonoBehaviour
{
    private bool check;
    private float time;

    private void Update()
    {
        if (this.check)
        {
            this.time += Time.deltaTime;
        }
        else
        {
            if (this.time != 0)
                Debug.Log(time);
        }
    }

    public void OnAniStart()
    {
        this.check = true;
    }

    public void OnAniEnd()
    {
        this.check = false;
    }
}
