using System.Collections;
using UnityEngine;

public class Character_Darlene : Character
{
    [Header("DARLENE")]
    [SerializeField] private PoolingObject poolingObject;
    [SerializeField] private Transform droneFollowPoint;
    [SerializeField] private ParticleSystem summonFX;

    [Space(10)]
    [SerializeField] private float droneLifeTime = 5f;

    private Vector3 summonPoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.droneFollowPoint == null)
            this.droneFollowPoint = transform.Find("DroneFollowPoint");
    }

    public override void SpecialSkill()
    {
        base.SpecialSkill();
        if (this.isReadySpecialSkill)
        {
            StartCoroutine(this.SummonDrone());
        }
    }

    private IEnumerator SummonDrone()
    {
        this.isSpecialSkill = true;
        this.isReadySpecialSkill = false;
        this.isCoolingDownSpecicalSkill = true;
        this.animator.SetTrigger("SpecialSkill");
        yield return new WaitForSeconds(0.8f);
        this.isSpecialSkill = false;

        GameObject droneObj = this.poolingObject.GetObject(this.summonPoint, Quaternion.identity);
        droneObj.GetComponent<DroneCtrl>().SetupDrone(this.droneFollowPoint, this.droneLifeTime);
    }

    public void PlaySummonFX() //Call in animation
    {
        float offsetY = 1.5f;
        this.summonPoint = this.droneFollowPoint.transform.position;
        this.summonFX.transform.position = new Vector3(this.summonPoint.x, offsetY + transform.position.y, this.summonPoint.z);
        this.summonFX.Play();
    }

    public void SetDroneLifeTime(float droneLifeTime)
    {
        this.droneLifeTime = droneLifeTime;
    }
}
