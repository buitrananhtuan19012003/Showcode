using UnityEngine;

public class PickupObject : SaiMonoBehaviour
{
    [SerializeField] protected Transform virtualObject;

    protected Collider col;
    protected Rigidbody rb;
    

    protected override void LoadComponent()
    {
        base.LoadComponent();
        this.LoadVirtualObjectTransform();
        this.LoadCollider();
        //this.LoadRigidbody();
    }

    protected virtual void LoadVirtualObjectTransform()
    {
        if (this.virtualObject == null)
        {
            this.virtualObject = transform.Find("VirtualObject").transform;
            Debug.LogWarning(gameObject.name + ": LoadVirtualObjectTransform", gameObject);
        }
    }
    protected virtual void LoadCollider()
    {
        if (this.col == null)
        {
            this.col = GetComponent<Collider>();
            Debug.LogWarning(gameObject.name + ": LoadCollider", gameObject);
        }
    }
    protected virtual void LoadRigidbody()
    {
        if (this.rb == null)
        {
            this.rb = GetComponent<Rigidbody>();
            Debug.LogWarning(gameObject.name + ": LoadRigidbody", gameObject);
        }
    }

    protected virtual void GenerateModelObject()
    {

    }
}
