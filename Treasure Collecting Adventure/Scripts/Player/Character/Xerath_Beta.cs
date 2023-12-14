using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Xerath_Beta : SaiMonoBehaviour
{
    [SerializeField] private Character_Xerath character_Xerath;

    private bool check;
    private float time;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.character_Xerath == null)
            this.character_Xerath = transform.parent.GetComponent<Character_Xerath>();
    }

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
