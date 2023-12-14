using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact(Transform interactorTransfrom);
    string GetInteractableText();
    Transform GetTransform();
    bool CanInteract();
}

public interface IInfoScanner
{
    FactionType GetFactionType();
    Transform GetTransform();
    Transform GetCenterPoint();
    string GetTargetName();
    int GetTargetLevel();
    IHealth GetHealth();
    bool CanScan();
}

public interface IHealth
{
    int GetMaxHealth();
    int GetCurrentHealth();
    bool IsDeath();
    void TakeDamage(int damage);
    void TakeDamage(int damage, Vector3 force, Vector3 hitPoint, Rigidbody hitRigidbody = null);
}

namespace LupinrangerPatranger
{
    public interface IIdentity
    {
        int ID { get; set; }
    }

    public interface INameable
    {
        string Name { get; set; }
    }

    public interface ISelectable
    {
        bool enabled { get; }
        Vector3 position { get; }
        void OnSelect();
        void OnDeselect();
    }

    public interface IJsonSerializable
    {
        void GetObjectData(Dictionary<string, object> data);
        void SetObjectData(Dictionary<string, object> data);
    }

    public interface ICondition : IAction
    {

    }
}
