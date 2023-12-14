using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float time;
    public Vector3 initialPosition;
    public Vector3 initialVelocity;
    public TrailRenderer tracer;
    protected bool isActive = false;
    public bool IsActive => isActive;

    public  void Deactive()
    {
        this.gameObject.SetActive(false);
        isActive = false;
        initialPosition = Vector3.zero;
        initialVelocity = Vector3.zero;
/*      tracer.emitting = false;
        tracer.Clear();*/
    }

    public virtual void Active(Vector3 position, Vector3 velocity)
    {
        this.gameObject.SetActive(true);
        isActive = true;
        time = 0f;
        initialPosition = position;
        initialVelocity = velocity;
/*        WeaponRaycast weapon = PlayerCtrl.Instance.PlayerWeapon.PlayerWeaponManager.GetActiveRaycastWeapon();
        if (weapon && weapon.Weapon.WeaponData.WeaponType != WeaponType.Shotgun) return;
        tracer.AddPosition(position);
        tracer.emitting = true;*/
    }
}