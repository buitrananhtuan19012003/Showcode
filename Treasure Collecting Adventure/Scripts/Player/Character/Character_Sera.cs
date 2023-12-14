using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Sera : Character
{
    [Header("SERA")]
    [SerializeField] private int skillDamage = 2;
    [SerializeField] private float dealySkillDealDamage = 1;
    [SerializeField] private int maxScanTimes = 3;
    [SerializeField] private float timer1 = 0.1f;
    [SerializeField] private float scanRange = 5f;
    [SerializeField] private PoolingObject poolingObject;
    [SerializeField] private ParticleSystem lightningStrikeFx;
    [SerializeField] private ScannerEnemy scannerEnemy;
    [SerializeField] private ElectricLine electricLine;

    private Transform enemyHit;
    private bool isDealDamageEnemies;
    private float timerSkillDealDamage;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.poolingObject == null)
            this.poolingObject = GetComponent<PoolingObject>();

        if (this.lightningStrikeFx == null)
            this.lightningStrikeFx = transform.Find("FX_LightningStrike").GetComponent<ParticleSystem>();

        if (this.scannerEnemy == null)
            this.scannerEnemy = GetComponentInChildren<ScannerEnemy>();

        if (this.electricLine == null)
            this.electricLine = GetComponentInChildren<ElectricLine>();
    }

    protected override void Update()
    {
        base.Update();

        this.DealDamageEnemies();
    }

    private void DealDamageEnemies()
    {
        if (!this.isDealDamageEnemies || this.scannerEnemy.Enemies.Count == 0) return;

        this.timerSkillDealDamage += Time.deltaTime;
        if (this.timerSkillDealDamage > this.dealySkillDealDamage)
        {
            this.timerSkillDealDamage = 0;
            foreach (EnemyController e in this.scannerEnemy.Enemies)
            {
                e.EnemyHealth.TakeDamage(this.skillDamage);
            }
        }
    }

    public override void SpecialSkill()
    {
        base.SpecialSkill();
        if (this.isReadySpecialSkill)
        {
            if (this.IsHitEnemy())
            {
                this.isMiss = false;
            }
            else
            {
                this.isMiss = true;
            }

            this.isReadySpecialSkill = false;
            this.isCoolingDownSpecicalSkill = true;
            this.isSpecialSkill = true;
            this.animator.SetTrigger("SpecialSkill");
            this.Invoke("EndSpecialSkill", 0.5f);
        }
    }

    public void EndSpecialSkill() // only Sera
    {
        this.isSpecialSkill = false;
    }
    public IEnumerator CastSpell()
    {
        if (this.enemyHit == null)
            yield break;

        this.PlayLightningStrikeFx();
        yield return new WaitForSeconds(this.timer1);

        this.scannerEnemy.Scan(this.enemyHit.GetComponent<EnemyController>(), this.maxScanTimes, this.scanRange);
        List<EnemyController> listEnemy = this.scannerEnemy.Enemies;
        List<ParticleSystem> listFx = new List<ParticleSystem>();

        this.isDealDamageEnemies = true; //Deal damage enemies
        this.timerSkillDealDamage = this.dealySkillDealDamage;

        if (listEnemy.Count > 1)
        {
            this.electricLine.SetListPoint(listEnemy);
            this.electricLine.gameObject.SetActive(true);
            StartCoroutine(this.electricLine.AnimateElectricBeam());
        }

        //TODO: Set state Electrocuted/ Dizzy in EnemyCtrl
        for (int i = 0; i < listEnemy.Count; i++)
        {
            ParticleSystem fx = this.poolingObject.GetObject(listEnemy[i].CenterPoint.position, listEnemy[i].CenterPoint.rotation).GetComponent<ParticleSystem>();
            float timeMinus = (float) i / (listEnemy.Count);
            listEnemy[i].EnemyDebuffs.Electrocuted(this.characterData.ExecutionSkillTime - this.timer1 - timeMinus);
            fx.Play();
            listFx.Add(fx);
        }
        yield return new WaitForSeconds(this.characterData.ExecutionSkillTime - this.timer1);

        this.isDealDamageEnemies = false;

        if (listEnemy.Count > 1)
        {
            this.electricLine.gameObject.SetActive(false);
        }
        for (int i = 0; i < listEnemy.Count; i++)
        {
            //listEnemy[i].Animator.Rebind();
            listFx[i].gameObject.SetActive(false);
        }
    }

    private void PlayLightningStrikeFx()
    {
        if (!this.enemyHit) return;

        this.lightningStrikeFx.transform.position = this.enemyHit.position;
        this.lightningStrikeFx.Play();
    }

    private bool IsHitEnemy()
    {
        if (PlayerCtrl.HasInstance)
        {
            Voidspawn_InfoScanner enemyHit = PlayerCtrl.Instance.PlayerInfoScanner.GetInfoScannerObjectByRaycast() as Voidspawn_InfoScanner;
            if (enemyHit != null)
            {
                this.enemyHit = enemyHit.transform;
                return true;
            }

            this.enemyHit = null;
            return false;

        }

        this.enemyHit = null;
        return false;
    }
}
