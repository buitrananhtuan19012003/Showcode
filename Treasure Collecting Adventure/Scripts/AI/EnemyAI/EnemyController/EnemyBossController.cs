public class EnemyBossController : EnemyController
{
    public override void PlayDetectSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_ORDE_DETECT_LETSGOWAR);
        }
    }

    public override void PlayDeathSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_ORDE_DIE_IMHURT001);
        }
    }
}