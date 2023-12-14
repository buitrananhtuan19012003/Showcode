using System.Collections;
using UnityEngine;

public class Character_Ironstone : Character
{
    [Header("IRONSTONE")]

    [SerializeField] private ParticleSystem HammerImpactEffect;
    [SerializeField] private ParticleSystem TransformEffect;
    private AnimatorStateInfo state;
    public GameObject Hammer;
    private float timeDelta;
    [SerializeField] private float delayAttack = 2f;
    private bool canAttack;
    public float damageRange = 3.5f;
    [SerializeField] private int powerSkill = 35;

    protected override void Start()
    {
        base.Start();
        if (this.HammerImpactEffect == null)
        {
            this.HammerImpactEffect = this.transform.Find("HammerImpactEFFECT").gameObject.GetComponent<ParticleSystem>();
        }
        if (this.TransformEffect == null)
        {
            this.TransformEffect = this.transform.Find("TransformEffect").gameObject.GetComponent<ParticleSystem>();
        }

    }
    protected override void Update()
    {
        base.Update();
        if (this.isReadySpecialSkill) return;
        this.HandleStateAnima();
        this.DelayAttack();
    }

    private void HandleStateAnima()
    {
        if (PlayerCtrl.Instance.PlayerInput.MovementInput != Vector2.zero)
        {
            Animator.SetLayerWeight(2, 0);
            Animator.SetLayerWeight(3, 1);
            state = Animator.GetCurrentAnimatorStateInfo(3);
        }
        else
        {
            Animator.SetLayerWeight(2, 1);
            Animator.SetLayerWeight(3, 0);
            state = Animator.GetCurrentAnimatorStateInfo(2);
            //state = Animator.GetCurrentAnimatorStateInfo(3);

        }

        if (state.IsName("HammerForm"))
        {
            if (state.normalizedTime > 0.5)
            {
                Hammer.gameObject.SetActive(true);
            }
        }

        if (state.IsName("Attack"))
        {
            PlayerCtrl.Instance.PlayerLocomotion.velocity.y -= 25 * Time.deltaTime;
            if (state.normalizedTime > 0.5)
            {
                if (HammerImpactEffect.isPlaying)
                {
                    return;
                }
                HammerImpactEffect.Play();
            }
        }
        else
        {
            HammerImpactEffect.Stop();
        }

    }

    public override void ActionMouseL()
    {
        base.ActionMouseL();
        if (this.canAttack)
        {
            this.Attack();
        }
    }


    public void DelayAttack()
    {
        timeDelta += Time.deltaTime;
        if (timeDelta < delayAttack) return;
        timeDelta = 0;
        canAttack = true;
    }
    public void Attack()
    {
        Animator.SetTrigger("AttackHammer");
        canAttack = false;
        //NovaImpactDamage();
    }

    public void NovaImpactDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRange);
        foreach (Collider collider in colliders)
        {
            var enemy = collider.GetComponent<HitBox>();
            if (enemy && enemy.CompareTag("EnemyCollider"))
            {
                enemy.OnHit(powerSkill);
            }
        }
    }
    public override void SpecialSkill()
    {
        base.SpecialSkill();
        if (this.isReadySpecialSkill)
        {
            StartCoroutine(TransformationCoroutine());
        }
    }

    private IEnumerator TransformationCoroutine()
    {
        this.Transform();
        yield return new WaitForSeconds(CharacterData.ExecutionSkillTime);
        this.RevertoForm();
        yield return new WaitForSeconds(1f);
        this.Animator.SetBool("RevertoForm", false);
    }

    public void Transform()
    {
        Invoke("PlayTransformEffect", 0.5f);
        this.isSpecialSkill = true;
        this.isReadySpecialSkill = false;
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.CharacterSpecific);
        this.Animator.SetTrigger("HammerTransfer");
    }
    public void RevertoForm()
    {
        this.isSpecialSkill = false;
        this.isCoolingDownSpecicalSkill = true;
        this.Animator.SetBool("RevertoForm", true);;
        PlayerCtrl.Instance.PlayerCombatAction.SetActionMouseLeft(CombatAction.None);
        Hammer.gameObject.SetActive(false);
    }

    public void PlayTransformEffect()
    {
        TransformEffect.Play();
    }
}
