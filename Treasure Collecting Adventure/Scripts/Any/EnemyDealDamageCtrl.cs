using UnityEngine;

public class EnemyDealDamageCtrl : SaiMonoBehaviour
{
    [SerializeField] protected DealDamageBox dealDamageBox;

    public DealDamageBox DealDamageBox { get => this.dealDamageBox; }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.dealDamageBox == null)
            this.dealDamageBox = GetComponentInChildren<DealDamageBox>();
        //this.dealDamageBox = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L/Shoulder_L/Elbow_L/Hand_L/UnarmedFist_L").GetComponent<DealDamageBox>();

    }

    public void EnableDealDamageCollider(int isEnable)
    {
        if (isEnable == 1)
        {
            this.dealDamageBox.SetActiveDeal(true);
            this.dealDamageBox.Col.enabled = true;
        }
        else
        {
            this.dealDamageBox.Col.enabled = false;
        }
    }
}
