using UnityEngine;

public class Health : MonoBehaviour
{
    protected float maxHealth;
    protected float currentHealth;

    void Start()
    {
        SetUp();
        OnStart();
    }

    public void TakeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealth(amount);
    }

    public void TakeDamage(float amount, Vector3 direction, Rigidbody rigidbody)
    {
        currentHealth -= amount;

        OnDamage(direction, rigidbody);

        if (currentHealth <= 0f)
        {
            Die(direction, rigidbody);
        }
    }

    private void SetUp()
    {
        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidbody in rigidBodies)
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            //hitBox.health = this;
            //hitBox.rb = rigidbody;
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("Hitbox");
            }
        }
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    private void Die(Vector3 direction, Rigidbody rigidbody)
    {
        OnDeath(direction, rigidbody);
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath(Vector3 direction, Rigidbody rigidbody)
    {

    }

    protected virtual void OnDamage(Vector3 direction, Rigidbody rigidbody)
    {

    }

    protected virtual void OnHealth(float amount)
    {

    }
}