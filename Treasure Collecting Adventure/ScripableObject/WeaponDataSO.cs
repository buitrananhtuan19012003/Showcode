#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "SO/WeaponData")]
public class WeaponDataSO : ItemDataSO
{
    [Header("OFFSET SPECIES")]
    public BodyType BodyType;

    [Header("OFFSET HOLSTER")]
    public Vector3 BigsizePosHolster;
    public Vector3 BigsizeRosHolster;

    [Header("OFFSET HOLSTER")]
    public Vector3 ShortPosHolster;
    public Vector3 ShortRosHolster;

    [Header("OFFSET HOLSTER")]
    public Vector3 NormalPosHolster;
    public Vector3 NormalRosHolster;

    [Header("OFFSET EQUIP")]
    public Vector3 BigsizePosEquip;
    public Vector3 BigsizeRosEquip;

    [Header("OFFSET EQUIP")]
    public Vector3 ShortPosEquip;
    public Vector3 ShortRosEquip;

    [Header("OFFSET EQUIP")]
    public Vector3 NormalPosEquip;
    public Vector3 NormalRosEquip;


    [Header("WEAPON")]

    public WeaponType WeaponType;
    public ShotGunType ShotGunType;
    public MeleeType MeleeType;

    [Header("RANGED WEAPON")]
    [SerializeField] private int rangedDamage;
    [SerializeField] private float accuracy;
    [SerializeField] private float timePerFireRate;//
    [SerializeField] private float fireRate; //
    [SerializeField] private float bulletSpeed;//
    [SerializeField] private float timeDelayPerShot;

    [Space(10)]
    [SerializeField] private AmmoType ammoType;
    [SerializeField] private int ammoPerShot;//
    [SerializeField] private int ammo;//
    [SerializeField] private int magazineSize;//
    [SerializeField] private float range;
    [Space(10)]
    [SerializeField] private float spreadAiming;//
    [SerializeField] private float spreadNotAim;//
    [SerializeField] private float[] spreads;


    [Header("MELEE WEAPON")]
    [SerializeField] private float meleeDamage;
    [SerializeField] private float swingSpeed;


    #region Getter
    public int RangedDamage { get => this.rangedDamage; private set => this.rangedDamage = value; }
    public float Accuracy { get => this.accuracy; private set => this.accuracy = value; }
    public float TimePerFireRate { get => this.timePerFireRate; private set => this.timePerFireRate = value; }
    public float FireRate { get => this.fireRate; private set => this.fireRate = value; }

    public float BulletSpeed { get => this.bulletSpeed; private set => this.bulletSpeed = value; }
    public float TimeDelayPerShot { get => this.timeDelayPerShot; private set => this.timeDelayPerShot = value; }
    public AmmoType AmmoType { get => this.AmmoType; private set => this.AmmoType = value; }
    public int AmmoPerShot { get => this.ammoPerShot; private set => this.ammoPerShot = value; }
    public int Ammo { get => this.ammo; set => this.ammo = value; }
    public int MagazineSize { get => this.magazineSize; private set => this.magazineSize = value; }
    public float Range { get => this.range; private set => this.range = value; }
    public float SpreadAiming { get => this.spreadAiming; private set => this.spreadAiming = value; }
    public float SpreadNotAim { get => this.spreadNotAim; private set => this.spreadNotAim = value; }
    public float[] Spreads { get => this.spreads; private set => this.spreads = value; }


    public float MeleeDamage { get => this.meleeDamage; private set => this.meleeDamage = value; }
    public float SwingSpeed { get => this.swingSpeed; private set => this.swingSpeed = value; }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(WeaponDataSO))]
public class WeaponDataEditor : Editor
{
    SerializedProperty itemName;
    SerializedProperty itemValue;
    SerializedProperty itemType;

    SerializedProperty icon;
    SerializedProperty model;

    SerializedProperty bodyType;

    SerializedProperty BigsizePosHolster;
    SerializedProperty BigsizeRosHolster;

    SerializedProperty ShortPosHolster;
    SerializedProperty ShortRosHolster;

    SerializedProperty NormalPosHolster;
    SerializedProperty NormalRosHolster;

    SerializedProperty BigsizePosEquip;
    SerializedProperty BigsizeRosEquip;

    SerializedProperty ShortPosEquip;
    SerializedProperty ShortRosEquip;

    SerializedProperty NormalPosEquip;
    SerializedProperty NormalRosEquip;

    SerializedProperty weaponType;
    SerializedProperty meleeType;
    SerializedProperty shotgunType;

    SerializedProperty rangedDamage;
    SerializedProperty accuracy;
    SerializedProperty timePerFireRate;
    SerializedProperty fireRate;
    SerializedProperty bulletSpeed;
    SerializedProperty timeDelayPerShot;
    SerializedProperty ammoType;
    SerializedProperty ammoPerShot;
    SerializedProperty ammo;
    SerializedProperty magazineSize;
    SerializedProperty range;
    SerializedProperty spreadAiming;
    SerializedProperty spreadNotAim;
    SerializedProperty spreads;

    SerializedProperty meleeDamage;
    SerializedProperty swingSpeed;

    private void OnEnable()
    {
        this.itemName = serializedObject.FindProperty("ItemName");
        this.itemValue = serializedObject.FindProperty("ItemValue");
        this.itemType = serializedObject.FindProperty("ItemType");
        this.icon = serializedObject.FindProperty("Icon");
        this.model = serializedObject.FindProperty("Model");

        /*        this.PosHolster = serializedObject.FindProperty("PosHolster");
                this.RosHolster = serializedObject.FindProperty("RosHolster");
                this.PosEquip = serializedObject.FindProperty("PosEquip");
                this.RosEquip = serializedObject.FindProperty("RosEquip");*/

        this.bodyType = serializedObject.FindProperty("BodyType");

        this.BigsizePosHolster = serializedObject.FindProperty("BigsizePosHolster");
        this.BigsizeRosHolster = serializedObject.FindProperty("BigsizeRosHolster");

        this.ShortPosHolster = serializedObject.FindProperty("ShortPosHolster");
        this.ShortRosHolster = serializedObject.FindProperty("ShortRosHolster");

        this.NormalPosHolster = serializedObject.FindProperty("NormalPosHolster");
        this.NormalRosHolster = serializedObject.FindProperty("NormalRosHolster");

        this.BigsizePosEquip = serializedObject.FindProperty("BigsizePosEquip");
        this.BigsizeRosEquip = serializedObject.FindProperty("BigsizeRosEquip");

        this.ShortPosEquip = serializedObject.FindProperty("ShortPosEquip");
        this.ShortRosEquip = serializedObject.FindProperty("ShortRosEquip");

        this.NormalPosEquip = serializedObject.FindProperty("NormalPosEquip");
        this.NormalRosEquip = serializedObject.FindProperty("NormalRosEquip");

        this.weaponType = serializedObject.FindProperty("WeaponType");
        this.shotgunType = serializedObject.FindProperty("ShotGunType");
        this.meleeType = serializedObject.FindProperty("MeleeType");

        this.rangedDamage = serializedObject.FindProperty("rangedDamage");
        this.accuracy = serializedObject.FindProperty("accuracy");
        this.timePerFireRate = serializedObject.FindProperty("timePerFireRate");
        this.fireRate = serializedObject.FindProperty("fireRate");
        this.bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        this.timeDelayPerShot = serializedObject.FindProperty("timeDelayPerShot");
        this.ammoType = serializedObject.FindProperty("ammoType");
        this.ammoPerShot = serializedObject.FindProperty("ammoPerShot");
        this.ammo = serializedObject.FindProperty("ammo");
        this.magazineSize = serializedObject.FindProperty("magazineSize");
        this.range = serializedObject.FindProperty("range");
        this.spreadAiming = serializedObject.FindProperty("spreadAiming");
        this.spreadNotAim = serializedObject.FindProperty("spreadNotAim");
        this.spreads = serializedObject.FindProperty("spreads");

        this.meleeDamage = serializedObject.FindProperty("meleeDamage");
        this.swingSpeed = serializedObject.FindProperty("swingSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        EditorGUILayout.PropertyField(this.itemName);
        EditorGUILayout.PropertyField(this.itemValue);
        EditorGUILayout.PropertyField(this.itemType);
        EditorGUILayout.PropertyField(this.icon);
        EditorGUILayout.PropertyField(this.model);

        EditorGUILayout.PropertyField(this.bodyType);
        BodyType selectedSpecies = (BodyType)bodyType.enumValueIndex;

        if (selectedSpecies == BodyType.BigSize)
        {
            EditorGUILayout.PropertyField(this.BigsizePosHolster);
            EditorGUILayout.PropertyField(this.BigsizeRosHolster);

            EditorGUILayout.PropertyField(this.BigsizePosEquip);
            EditorGUILayout.PropertyField(this.BigsizeRosEquip);
        }

        else if (selectedSpecies == BodyType.Short)
        {
            EditorGUILayout.PropertyField(this.ShortPosHolster);
            EditorGUILayout.PropertyField(this.ShortRosHolster);

            EditorGUILayout.PropertyField(this.ShortPosEquip);
            EditorGUILayout.PropertyField(this.ShortRosEquip);
        }

        if (selectedSpecies == BodyType.Normal)
        {
            EditorGUILayout.PropertyField(this.NormalPosHolster);
            EditorGUILayout.PropertyField(this.NormalRosHolster);

            EditorGUILayout.PropertyField(this.NormalPosEquip);
            EditorGUILayout.PropertyField(this.NormalRosEquip);
        }
        else
        {
            //nothing
        }

        EditorGUILayout.PropertyField(this.weaponType);

        WeaponType selectedWeaponType = (WeaponType)weaponType.enumValueIndex;
        EditorGUILayout.PropertyField(this.rangedDamage);
        EditorGUILayout.PropertyField(this.accuracy);


        if (selectedWeaponType == WeaponType.AssaultRifle)
        {
            EditorGUILayout.PropertyField(this.timePerFireRate);
            EditorGUILayout.PropertyField(this.fireRate);
            EditorGUILayout.PropertyField(this.bulletSpeed);
            EditorGUILayout.PropertyField(this.ammoType);
            EditorGUILayout.PropertyField(this.ammoPerShot);
            EditorGUILayout.PropertyField(this.ammo);
            EditorGUILayout.PropertyField(this.magazineSize);
            EditorGUILayout.PropertyField(this.range);
            EditorGUILayout.PropertyField(this.spreadAiming);
            EditorGUILayout.PropertyField(this.spreadNotAim);
            EditorGUILayout.PropertyField(this.spreads);
        }
        else if (selectedWeaponType == WeaponType.Shotgun || selectedWeaponType == WeaponType.Pistol)
        {
            if (selectedWeaponType == WeaponType.Shotgun)
            {
                EditorGUILayout.PropertyField(this.shotgunType);
            }
            EditorGUILayout.PropertyField(this.bulletSpeed);
            EditorGUILayout.PropertyField(this.timeDelayPerShot);
            EditorGUILayout.PropertyField(this.ammoType);
            EditorGUILayout.PropertyField(this.ammoPerShot);
            EditorGUILayout.PropertyField(this.ammo);
            EditorGUILayout.PropertyField(this.magazineSize);
            EditorGUILayout.PropertyField(this.range);
            EditorGUILayout.PropertyField(this.spreadAiming);
            EditorGUILayout.PropertyField(this.spreadNotAim);
            EditorGUILayout.PropertyField(this.spreads);
        }

        else if (selectedWeaponType == WeaponType.SniperRifle)
        {
            EditorGUILayout.PropertyField(this.bulletSpeed);
            EditorGUILayout.PropertyField(this.timeDelayPerShot);
            EditorGUILayout.PropertyField(this.ammoType);
            EditorGUILayout.PropertyField(this.ammoPerShot);
            EditorGUILayout.PropertyField(this.ammo);
            EditorGUILayout.PropertyField(this.magazineSize);
            EditorGUILayout.PropertyField(this.range);
            EditorGUILayout.PropertyField(this.spreadAiming);
            EditorGUILayout.PropertyField(this.spreadNotAim);
            EditorGUILayout.PropertyField(this.spreads);
        }
        else
        {
            EditorGUILayout.PropertyField(meleeType);
            EditorGUILayout.PropertyField(meleeDamage);
            EditorGUILayout.PropertyField(swingSpeed);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif