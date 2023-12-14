using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbstract : SaiMonoBehaviour
{
    [SerializeField] protected PlayerCtrl playerCtrl;

    public PlayerCtrl PlayerCtrl => this.playerCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadPlayerCtrl();
    }

    protected virtual void LoadPlayerCtrl()
    {
        if (this.playerCtrl == null)
        {
            this.playerCtrl = transform.parent.GetComponent<PlayerCtrl>();
            Debug.LogWarning(gameObject.name + " : LoadPlayerCtrl", gameObject);
        }
    }
}
