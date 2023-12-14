using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "SO/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public string EnemyName;
    public EnemyType EnemyType;
    public int Health;
    public int Damage;
    public float Speed;
    public float AttackRange;
    public float DetectionRange = 10;
}
