using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : SaiMonoBehaviour
{
    [SerializeField] private List<NPCBehaviour> listBehaviours = new List<NPCBehaviour>();
    [SerializeField] private Animator animator;
    [SerializeField] private float delay = 3f;

    private bool stopBehave;
    private float timer;
    private float random;
    private Quaternion originRot;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.animator == null)
            this.animator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.originRot = transform.localRotation;
    }

    private void FixedUpdate()
    {
        if (this.stopBehave || this.listBehaviours.Count == 0) return;

        this.timer += Time.fixedDeltaTime;
        if (this.timer > this.delay + this.random)
        {
            this.timer = 0;
            this.random = Random.Range(-this.delay / 5f, this.delay / 5f);
            this.animator.SetTrigger(this.listBehaviours[Random.Range(0, this.listBehaviours.Count)].ToString());
        }
    }

    public void SetAnimaton(string name, float time)
    {
        this.animator.SetTrigger(name);
        this.stopBehave = true;
        CancelInvoke("ContinueBehave");
        Invoke("ContinueBehave", time);
    }

    private void ContinueBehave()
    {
        this.stopBehave = false;
        transform.localRotation = this.originRot;
    }
}
