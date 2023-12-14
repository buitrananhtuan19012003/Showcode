using UnityEngine;

public class HitBox : SaiMonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject healthObject;

    public GameObject HealthObject { get => this.healthObject; set => this.healthObject = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.rb == null )
            this.rb = GetComponent<Rigidbody>();
    }

    //public void Setup(Rigidbody rb, GameObject healthGameObject)
    //{
    //    this.rb = rb;
    //    this.healthObject = healthGameObject;
    //}

    public void OnHit(int damage)
    {
        if (this.healthObject == null) return;

        this.healthObject.GetComponent<IHealth>().TakeDamage(damage);
    }

    public void OnHit(int damage, Vector3 force)
    {
        if (this.healthObject == null || this.rb == null) return;

        this.healthObject.GetComponent<IHealth>().TakeDamage(damage, force, this.transform.position, this.rb);
    }

    public bool IsSetup()
    {
        return this.rb != null && this.healthObject != null;
    }
}