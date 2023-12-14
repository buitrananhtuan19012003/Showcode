public enum ListenType
{
    ANY = 0,
    ON_PLAYER_DEATH,
    ON_ENEMY_DEATH,
    UPDATE_COUNT_TEXT,
    UPDATE_USER_INFO,
    UPDATE_AMMO
}
public enum UIType
{
    Unknow = 0,
    Screen = 1,
    Popup = 2,
    Notify = 3,
    Overlap = 4,
}

public enum AmmoType
{
    None,
    PistolRound,
    RifleRound,
    ShotgunCell,
    SniperRound,
}

public enum AiStateID
{
    ChasePlayer,
    Death,
    Idle,
    FindWeapon,
    Attack
}

public enum BodyType
{
    Normal,
    Short,
    BigSize
}

public enum CombatAction
{
    None = 0,
    RangedWeapon = 10,
    MeleeWeapon = 20,
    CharacterSpecific = 30
}

public enum DroneStateId
{
    Idle,
    Follow,
    Attack,
}

public enum DebuffsType
{
    None = 0,
    Low = 5,
    Stun = 10,
    Electrocuted = 15,
    Bleeding = 20,
    Blind = 25,
}

public enum WeaponSlot
{
    Primary = 0,
    Secondary = 1
}

public enum SocketID
{
    RightLeg,
    RightHand
}

public enum ItemType
{
    None = 0,
    Weapon = 1,
    Consumable = 2,
    Ammo = 3,
}

public enum EquipWeaponBy
{
    Player,
    AI
}

public enum EnemyType
{
    None,
    Porderman,
    Semi,
    Boss
}

public enum WeaponList
{
    EquippedWeapons,
    BackpackWeapons
}

public enum WeaponType
{
    None,
    Pistol,
    AssaultRifle,
    Shotgun,
    SniperRifle,
    Melee,
}

public enum FactionType
{
    Unknow = 0,
    Alliance = 5,
    Voidspawn = 10
}

public enum EnemyStateId
{
    Idle,
    Roam,
    Follow,
    Chase,
    Attack,
    Death,
    Debuff_Electrocuted,
}

public enum NPCBehaviour
{
    None = 0,

    Wave = 10,

    Talk1 = 20,
    Talk2 = 21,

    Question1 = 30,
    Questtion2 = 31,

    Sit = 40,

    SitWounded = 50,

    Hammer = 60,
}

public enum MeleeType
{
    None,
    LongMelee,
    Knife
}

public enum ShotGunType
{
    None,
    Venom,
    Slowhand
}

public enum SceneIndex
{
    MainMenu,
    CharacterSelection,
    Village,
    Dungeon
}

public enum Species
{
    Unknow,
    Human,
    Cyborg,
    AdvancedAIRobot,
    Dwarf,
    Siren,
    Titan,
    Morphosians,
}

namespace LupinrangerPatranger
{
    public enum VariableType
    {
        None,
        Bool,
        Color,
        Float,
        GameObject,
        Int,
        Object,
        String,
        Vector2,
        Vector3
    }

    public enum ActionStatus
    {
        Inactive,
        Failure,
        Success,
        Running
    }

    //public enum ConnectionStyle
    //{
    //    Angular,
    //    Curvy,
    //    Line
    //}

    public enum ConditionType
    {
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual,
    }
}