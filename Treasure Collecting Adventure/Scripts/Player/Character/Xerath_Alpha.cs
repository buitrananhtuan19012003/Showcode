using UnityEngine;

public class Xerath_Alpha : SaiMonoBehaviour
{
    [SerializeField] private Character_Xerath character_Xerath;
    [SerializeField] private DealDamageBox fistL;
    [SerializeField] private DealDamageBox fistR;
    [SerializeField] private Animator animator;
    [SerializeField] private float coolDownTime = 0.1f;

    private int comboCounter;
    private int currentComboIndex = 0;
    private float lastClicked;
    private float lastComboEnd = 0;
    private string[] combos = new string[3];

    private float level0 = 1;
    private float level1 = 1.5f;
    private float level2 = 2f;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.character_Xerath == null)
            this.character_Xerath = transform.parent.GetComponent<Character_Xerath>();

        if (this.animator == null)
            this.animator = GetComponent<Animator>();

        if (this.fistL == null)
            this.fistL = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_L").GetComponentInChildren<DealDamageBox>();

        if (this.fistR == null)
            this.fistR = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R").GetComponentInChildren<DealDamageBox>();
    }

    private void Start()
    {
        this.combos[0] = "Unarmed01";
        this.combos[1] = "Unarmed02";
        this.combos[2] = "Unarmed03";

        this.fistL.Damage = this.character_Xerath.AlphaFistDamage;
        this.fistR.Damage = this.character_Xerath.AlphaFistDamage;
    }

    private void OnEnable()
    {
        this.currentComboIndex = 0;
    }
    private void Update()
    {
        this.ResetComboState();
    }

    public void Acttack()
    {
        if (this.combos == null || Time.time - this.lastComboEnd <= this.coolDownTime) return;

        if (this.comboCounter == this.combos.Length && Time.time - this.lastClicked > this.coolDownTime)
            this.comboCounter = 0;
        this.comboCounter++;
        this.comboCounter = Mathf.Clamp(this.comboCounter, 0, this.combos.Length);
        this.lastClicked = Time.time;
        string comboName = this.combos[this.currentComboIndex];
        for (int i = 0; i < this.comboCounter; i++)
        {
            if (i == 0)
            {
                if (this.comboCounter == 1 && this.animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
                {
                    this.animator.SetBool("Attack", true);
                    this.animator.SetBool(comboName + "_" + (i + 1), true);
                }
            }
            else
            {
                if (this.comboCounter >= (i + 1) && this.animator.GetCurrentAnimatorStateInfo(0).IsName(comboName + "_" + i))
                {
                    this.animator.SetBool(comboName + "_" + (i + 1), true);
                    this.animator.SetBool(comboName + "_" + i, true);
                }
            }
        }
    }

    private void ResetComboState()
    {
        AnimatorStateInfo state = this.animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag("Movement")) return;

        string comboName = this.combos[this.currentComboIndex];
        for (int i = 0; i < this.combos.Length; i++)
        {
            if (state.IsName(comboName + "_" + (i + 1)))
            {
                if (state.normalizedTime >= 0.9)
                {
                    Debug.Log("run");
                    this.comboCounter = 0;
                    this.lastComboEnd = Time.time;
                    this.animator.SetBool("Attack", false);
                    for (int j = i + 1; j > 0; j--)
                    {
                        this.animator.SetBool(comboName + "_" + j, false);
                        Debug.Log(j + "run");
                        Debug.Log("RunAttackFalse");
                    }
                }
            }
        }
    }

    public void OnAnimationEndCombo() // Call in the end frame of the last attack in the combo
    {
        this.comboCounter = 0; // Reset comboCounter after the combo is executed
        this.currentComboIndex++;
        if (this.currentComboIndex >= this.combos.Length)
            this.currentComboIndex = 0;
    }

    public void SetLevelDamage(int level)   //Call in animation
    {
        if (level == 0) 
        {
            this.fistL.Damage = (int) (this.character_Xerath.AlphaFistDamage * this.level0);
            this.fistR.Damage = (int) (this.character_Xerath.AlphaFistDamage * this.level0);
        }
        else if (level == 1)
        {
            this.fistL.Damage = (int)(this.character_Xerath.AlphaFistDamage * this.level1);
            this.fistR.Damage = (int)(this.character_Xerath.AlphaFistDamage * this.level1);
        }
        else
        {
            this.fistL.Damage = (int)(this.character_Xerath.AlphaFistDamage * this.level2);
            this.fistR.Damage = (int)(this.character_Xerath.AlphaFistDamage * this.level2);
        }
    }

    public void EnableDealDamageCollider_XerathAlpha(int index) //Call in animation
    {

        if (index == 0)
        {
            this.fistL.SetActiveDeal(false);
            this.fistR.SetActiveDeal(false);
        }
        else if (index == 1)
        {
            this.fistL.SetActiveDeal(true);
            this.fistR.SetActiveDeal(false);
        }   
        else if (index == 2)
        {
            this.fistL.SetActiveDeal(false);
            this.fistR.SetActiveDeal(true);
        }
        else
        {
            this.fistL.SetActiveDeal(true);
            this.fistR.SetActiveDeal(true);
        }
    }
}
