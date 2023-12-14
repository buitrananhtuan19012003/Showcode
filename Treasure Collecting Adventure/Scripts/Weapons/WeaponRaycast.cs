
using Unity.VisualScripting;
using UnityEngine;
using static WeaponRaycast;

public class WeaponRaycast : MonoBehaviour
{
    public float ForceRaycast;
    [HideInInspector] public Weapon Weapon;
    [HideInInspector] public float runtTimeFire;


    //RaycastWeapon Variable
    public ParticleSystem[] muzzleflash;
    public ParticleSystem hitEffect;
    public Transform raycastOrigin;
    protected Ray ray;
    public RaycastHit hitInfo;
    private float maxlifeTime = 3;
    public RecoilWeapon recoil;
    public GameObject magazine;

    public bool isDelay;
    private float bulletDrop = 0;
    public int currentAmmo;
    public int maxAmmo;
    public float[] Spreads = new float[4];

    private void Awake()
    {
        Weapon = GetComponent<Weapon>();
        runtTimeFire = 0;
        recoil = GetComponent<RecoilWeapon>();
        maxAmmo = Weapon.WeaponData.MagazineSize;
        currentAmmo = maxAmmo;
        if (hitEffect == null)
        {
            hitEffect = transform.Find("FX/HitEffect_Asaka").GetComponent<ParticleSystem>();
        }
    }
    private void Start()
    {
        Spreads[0] = Weapon.WeaponData.Spreads[0];
        Spreads[1] = Weapon.WeaponData.Spreads[1];
        Spreads[2] = Weapon.WeaponData.Spreads[2];
        Spreads[3] = Weapon.WeaponData.Spreads[3];
    }

    public virtual void DelayPerShot()
    {
        // for override
    }

    public virtual void FireBullet(Vector3 target)
    {
        if (currentAmmo <= 0)
        {
            return;
        }
        currentAmmo--;
        if (muzzleflash != null)
        {
            foreach (var particle in muzzleflash)
            {
                particle.Emit(1);
            }
        }

        for (int i = 0; i < Weapon.WeaponData.AmmoPerShot; i++)
        {
            float xspread = Random.Range(Spreads[0], Spreads[1]);
            float yspread = Random.Range(Spreads[2], Spreads[3]);
            Vector3 randomSpread = new Vector3(xspread, yspread, 0f);
            Vector3 raycastDirection = ((target - raycastOrigin.position).normalized + randomSpread) * Weapon.WeaponData.BulletSpeed;
            var bullet = ObjectPool.Instance.GetPooledObject();
            bullet.Active(raycastOrigin.position, raycastDirection);
            if (recoil)
                recoil.GenerateRecoil();
        }
    }

    public virtual void UpdateFiring(Vector3 target)
    {
        runtTimeFire += Time.deltaTime;
        if (Weapon.WeaponData.TimePerFireRate == 0)
        {
            Debug.LogWarning("Please set TimePerFireRate variable # 0");
            return;
        }
        float fireInterval = Weapon.WeaponData.TimePerFireRate / Weapon.WeaponData.FireRate; // 1/firerate ( 1 ở đây đại diện cho 1 giây , firerate là số đạn nhả ra trong 1 giây , nếu ta set firerate = 25 là 25 viên trong 1s
        while (runtTimeFire >= 0.0f)
        {
            FireBullet(target);
            runtTimeFire -= fireInterval;
        }
    }

    public virtual void UpdateBullets()
    {
        ObjectPool.Instance.pooledObjects.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += Time.deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
        DeactiveBullets();
    }

    public virtual Vector3 GetPosition(Bullet bullet)
    {
        // p = p0 + v*t +0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    public virtual void RaycastSegment(Vector3 Start, Vector3 End, Bullet bullet)
    {
        Vector3 direction = End - Start;
        float distance = direction.magnitude;
        ray.origin = Start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            if (hitEffect)
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
            }

            bullet.transform.position = hitInfo.point;
            bullet.time = maxlifeTime;
            End = hitInfo.point;
            var Enemy = hitInfo.collider.GetComponentInParent<HitBox>();
            if (Enemy && !Enemy.CompareTag("PlayerRagdoll") && !Enemy.CompareTag("PlayerCollider"))
            {
                Enemy.GetComponentInParent<HitBox>().OnHit(Weapon.WeaponData.RangedDamage);
            }
           // AddForceToHitInfo(ray, hitInfo);

        }
        bullet.transform.position = End;
    }

    public virtual void DeactiveBullets()
    {
        foreach (Bullet bullet in ObjectPool.Instance.pooledObjects)
        {
            if (bullet.time >= maxlifeTime)
            {
                bullet.Deactive();
            }
        }
    }



    public virtual void AddForceToHitInfo(Ray ray, RaycastHit hitInfo)
    {
        var hitpointObj = hitInfo.collider.GetComponent<Rigidbody>();
        if (hitpointObj != null)
        {
            hitpointObj.AddForceAtPosition(ray.direction * ForceRaycast, hitInfo.point, ForceMode.Impulse);
        }
    }

}
