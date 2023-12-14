using System.Collections.Generic;
using UnityEngine;

public class DealDamageBox : SaiMonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool isPlayerWeapon = true;
    [SerializeField] private Collider col;
    [SerializeField] private Transform originParent;

    private HashSet<Collider> hitCols = new HashSet<Collider>();
    private Vector3 originPos;
    private Quaternion originRot;
    private string originLayer;

    public int Damage { get => this.damage; set => this.damage = value; }
    public Collider Col { get => this.col; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.col == null)
            this.col = GetComponent<Collider>();

        if (this.originParent == null)
            this.originParent = transform.parent;
    }

    private void Start()
    {
        this.col.enabled = false;
        this.originPos = transform.localPosition;
        this.originRot = transform.localRotation;
        this.originLayer = gameObject.layer.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCollider") && this.isPlayerWeapon)
        {
            Debug.Log("Hit enemy: " + other.gameObject.name + "_ -" + this.damage);
        }
        if (other.CompareTag("PlayerCollider") && !this.isPlayerWeapon)
        {
            Debug.Log("Hit alliance: " + other.gameObject.name + "_ -" + this.damage);
        }
        if (other.CompareTag(this.isPlayerWeapon ? "EnemyCollider" : "PlayerCollider") && !this.hitCols.Contains(other))
        {
            this.hitCols.Add(other);
            other.GetComponentInChildren<HitBox>().OnHit(this.damage);
        }
    }

    public void SetActiveDeal(bool isActive)
    {
        if (isActive)
            this.hitCols.Clear();
        this.col.enabled = isActive;
    }

    public void ResetWeapon()
    {
        transform.SetParent(this.originParent);
        transform.localPosition = this.originPos;
        transform.localRotation = this.originRot;
        gameObject.layer = LayerMask.NameToLayer(this.originLayer);
    }
}
