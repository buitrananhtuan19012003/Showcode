using System.Collections.Generic;
using UnityEngine;

public class TakeDamageCtrl : SaiMonoBehaviour
{
    [SerializeField] private HitBox colMainHitBox; //for melee damage
    [SerializeField] private List<HitBox> listRagdollHitBox; //for ranged damage
    [SerializeField] private bool isPlayer;

    private readonly string playerColMainTag = "PlayerCollider";
    private readonly string playerRagdollTag = "PlayerRagdoll";
    private readonly string enemyColMainTag = "EnemyCollider";
    private readonly string enemyRagdollTag = "EnemyRagdoll";

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.colMainHitBox == null)
        {
            this.colMainHitBox = GetComponent<HitBox>();
            this.gameObject.tag = this.isPlayer ? this.playerColMainTag : this.enemyColMainTag;
        }

        if (this.listRagdollHitBox.Count == 0)
            if (transform.Find("Root") != null)
            {
                foreach (HitBox hitBox in transform.Find("Root").GetComponentsInChildren<HitBox>())
                {
                    this.listRagdollHitBox.Add(hitBox);
                    hitBox.tag = this.isPlayer ? this.playerRagdollTag : this.enemyRagdollTag;
                }
            }
    }

    public void SetHealthObject(GameObject healthObject)
    {
        this.colMainHitBox.HealthObject = healthObject;
        foreach (HitBox hitBox in this.listRagdollHitBox)
        {
            hitBox.HealthObject = healthObject;
        }
    }
}
