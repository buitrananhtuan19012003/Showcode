using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RagdollController : SaiMonoBehaviour
{
    [SerializeField] private List<Rigidbody> listRagdollRigidbody = new List<Rigidbody>();
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    public Animator Animator { get => this.animator; set => this.animator = value; }
    public CharacterController CharacterController { get => this.characterController; set => this.characterController = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        //Rigidbody[] rigidbodies = transform.parent.Find("root hips 1").GetComponentsInChildren<Rigidbody>();
        //if (this.listRagdollRigidbody.Count != rigidbodies.Length)
        //{
        //    this.listRagdollRigidbody.Clear(); ;
        //    for (int i = 0; i < rigidbodies.Length; i++)
        //    {
        //        this.listRagdollRigidbody.Add(rigidbodies[i]);
        //        //if (rigidbodies[i].GetComponent<HitBox>() == null) 
        //        //{
        //        //    rigidbodies[i].AddComponent<HitBox>();
        //        //}
        //    }
        //}
    }

    public void DisableRagdoll()
    {
        this.animator.enabled = true;
        this.animator.Rebind();
        this.characterController.enabled = true;
        this.SetKinematic(true);
    }

    public void EnableRagdoll()
    {
        this.animator.enabled = false;
        this.characterController.enabled = false;
        this.SetKinematic(false);

        Invoke("SetKinematicFalse", 2f);
    }

    private void SetKinematic(bool isKinematic)
    {
        for (int i = 0; i < this.listRagdollRigidbody.Count; i++)
        {
            this.listRagdollRigidbody[i].isKinematic = isKinematic;
        }
    }

    private void SetKinematicFalse()
    {
        this.SetKinematic(false);
    }

    public Rigidbody ClosestRigidbody(Vector3 hitPoint)
    {
        Rigidbody hitRigidbody = this.listRagdollRigidbody.OrderBy(rb => Vector3.Distance(rb.position, hitPoint)).First();
        return hitRigidbody;
    }
}
