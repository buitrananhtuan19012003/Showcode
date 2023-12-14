using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "SO/CharacterData")]
public class CharacterDataSO : ScriptableObject
{
    [Header("OVERVIEW")]
    public string CharacterName;
    public string CharacterClass;
    public Species Species;
    public BodyType BodyType;
    public Sprite CharacterIcon;
    public GameObject CharacterModel;
    [TextAreaAttribute(5, 10)] public string CharacterDescription;

    [Header("STATS")]
    [Range(50, 100)] public int Health = 50;
    [Range(0, 10)] public int Power = 5;
    [Range(0, 10)] public int Defence = 5;
    [Range(0,10)] public int Agility = 5;
    [Range(0, 10)] public int HitPoint = 5;

    [Header("SKILL")]
    public float ExecutionSkillTime;
    public float CooldownSkillTime;
    public string SpecialSkillName;
    public Sprite SpecialSkillIcon;
    [TextArea(3, 8)] public string SpecialSkillDescription;

    [Header("SKIN")]
    /// <summary>
    ///(string) The first skin is set as default skin
    /// </summary>
    public List<GameObject> ListSkin;
}
