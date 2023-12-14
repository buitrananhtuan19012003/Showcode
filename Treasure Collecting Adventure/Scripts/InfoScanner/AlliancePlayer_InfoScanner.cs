using UnityEngine;

public class AlliancePlayer_InfoScanner : SaiMonoBehaviour, IInfoScanner
{
    [SerializeField] private Transform centerPoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.centerPoint == null)
            this.centerPoint = GetComponent<Character>().CenterPoint;
    }

    public FactionType GetFactionType()
    {
        return FactionType.Alliance;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Transform GetCenterPoint()
    {
        return this.centerPoint;
    }

    public string GetTargetName()
    {
        return gameObject.name;
    }

    public int GetTargetLevel()
    {
        return 1;
    }

    public IHealth GetHealth()
    {
        if (PlayerCtrl.HasInstance)
        {
            return PlayerCtrl.Instance.PlayerHealth;
        }
        return null;
    }

    public bool CanScan()
    {
        if (PlayerCtrl.HasInstance)
        {
            return !PlayerCtrl.Instance.PlayerHealth.IsDeath();
        }
        return false;
    }
}
