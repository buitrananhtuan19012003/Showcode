using UnityEngine;

public class EnemyHealth : EnemyAbstract, IHealth
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int currentHealth;

    private bool isDeath;

    public void SetHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.currentHealth = this.maxHealth;
    }

    public void ResetHealth()
    {
        this.currentHealth = this.maxHealth;
    }

    public int GetMaxHealth()
    {
        return this.maxHealth;
    }

    public int GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public bool IsDeath()
    {
        return this.isDeath;
    }

    public void TakeDamage(int damage)
    {
        if (this.isDeath) return;

        this.currentHealth -= damage;
        this.enemyCtrl.GraphicEffect.PlayHitEffect();

        if (this.enemyCtrl.EnemyDebuffs.CurDebuff != DebuffsType.Electrocuted)
            this.enemyCtrl.Animator.SetTrigger(Random.Range(0, 2) == 0 ? "TakeDamage1" : "TakeDamage2");

        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            this.Die();
        }
    }

    public void TakeDamage(int damage, Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody)
    {
        this.currentHealth -= damage;
        this.enemyCtrl.GraphicEffect.PlayHitEffect();
        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            this.Die(force, hitPoint, hitRigidbody);
        }
    }

    public void Die()
    {
        this.isDeath = true;
        this.enemyCtrl.SetDeath();
    }

    public void Die(Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody = null)
    {
        this.Die();
        if (hitRigidbody == null)
            hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
        else
            this.enemyCtrl.RagdollCtrl.ClosestRigidbody(hitPoint).AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }
}
