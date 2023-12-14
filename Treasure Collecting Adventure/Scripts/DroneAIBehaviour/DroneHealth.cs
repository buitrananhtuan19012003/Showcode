using UnityEngine;

public class DroneHealth : SaiMonoBehaviour, IHealth
{
    [SerializeField] private DroneCtrl droneCtrl;
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.droneCtrl == null)
            this.droneCtrl = GetComponentInParent<DroneCtrl>();
    }

    private void OnEnable()
    {
        this.SetupHealth();
    }

    public void SetupHealth()
    {
        this.currentHealth = this.maxHealth;
    }
    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public bool IsDeath()
    {
        return this.currentHealth <= 0;
    }

    public void TakeDamage(int damage)
    {
        this.currentHealth -= damage;
        this.droneCtrl.GraphicEffect.PlayHitEffect();
        if (this.currentHealth <= 0) 
        {
            this.currentHealth = 0;
            this.Die();
        }
    }

    private void Die()
    {
        StopCoroutine(this.droneCtrl.LifeTimeOfDrone());
        StartCoroutine(this.droneCtrl.ShutdownDrone());
        Debug.Log("DronHP = 0", gameObject);
    }

    public void TakeDamage(int damage, Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody)
    {
        throw new System.NotImplementedException();
    }
}
