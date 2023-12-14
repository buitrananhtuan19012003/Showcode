using UnityEngine;

public class AllianceCompanion_InfoScanner : SaiMonoBehaviour, IInfoScanner
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private bool isNPC;

    private IHealth health;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.centerPoint == null)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.gameObject.name == "CenterPoint")
                {
                    this.centerPoint = t;
                    break;
                }
            }
        }

        if (this.health == null)
            this.health = GetComponentInChildren<IHealth>();
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
        return this.health;
    }

    public bool CanScan()
    {
        if (this.isNPC)
        {
            Debug.Log("NPC");
            return true;
        }
        return !this.health.IsDeath();
    }
}
