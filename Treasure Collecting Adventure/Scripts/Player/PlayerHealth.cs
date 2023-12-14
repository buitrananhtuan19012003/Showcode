using Cinemachine;
using UnityEngine;

public class PlayerHealth : PlayerAbstract, IHealth
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private int defence;
    private int agility;
    private bool isDeath;
    [SerializeField] private int x = 5;

    public delegate void TakeDamageEvent();
    public event TakeDamageEvent OnTakeDamage;

    private void Start()
    {
        if (this.playerCtrl.Character)
        {
            this.maxHealth = this.playerCtrl.Character.CharacterData.Health + this.playerCtrl.Character.CharacterData.HitPoint * x;
            this.currentHealth = this.maxHealth;
            this.defence = playerCtrl.Character.CharacterData.Defence;
            this.agility = playerCtrl.Character.CharacterData.Agility;
        }
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

        float damageTaken = (float)damage * (10f / (10f + this.defence + this.agility / 2f));
        this.currentHealth -= Mathf.RoundToInt(damageTaken);

        if (OnTakeDamage != null)
        {
            OnTakeDamage();
        }

        if (this.currentHealth <= 0)
        {
            this.currentHealth = 0;
            this.Die();
        }
    }

    public void Die()
    {
        this.isDeath = true;
        this.playerCtrl.Character.RagdollCtrl.EnableRagdoll();
        this.playerCtrl.PlayerInput.enabled = false;
        //this.playerCtrl.PlayerCamera.FPSCamera.Follow =this.playerCtrl.Character.CenterPoint;
        //this.playerCtrl.PlayerCamera.FPSCamera.LookAt = null;
        //this.playerCtrl.PlayerCamera.FPSCamera.GetComponent<CinemachineInputProvider>().enabled = false;
        this.playerCtrl.PlayerCamera.IsTPSCamera = true;
        this.playerCtrl.PlayerCamera.TPSCamera.Follow = this.playerCtrl.Character.CenterPoint;
        this.playerCtrl.PlayerCamera.TPSCamera.LookAt = null;
        this.playerCtrl.PlayerCamera.TPSCamera.GetComponent<CinemachineInputProvider>().enabled = false;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_LOSE_AMAZEING_HAMSTER_PIANO_VERSION, 1.5f);
        }

        Invoke("ShowLosePanel", 2f);
    }

    private void ShowLosePanel()
    {
        if (UIManager.HasInstance)
        {
            //UIManager.Instance.InGamePanel.ShowOther(null);
            //UIManager.Instance.InGamePanel.Other.ShowLosePanel();
        }
    }

    public void TakeDamage(int damage, Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody)
    {
        throw new System.NotImplementedException();
    }
}
