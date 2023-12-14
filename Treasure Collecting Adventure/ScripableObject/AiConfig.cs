using UnityEngine;

[CreateAssetMenu(fileName ="AiConfig", menuName = "AI/AiConfig")]
public class AiConfig : ScriptableObject
{
    [Header("CHASE STATE")]
    public float MaxTime = 0.5f;
    public float MaxDistance = 1f;
}
