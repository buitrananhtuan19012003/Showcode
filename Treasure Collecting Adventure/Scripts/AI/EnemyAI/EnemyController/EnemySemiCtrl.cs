public class EnemySemiCtrl : EnemyController
{
    public override void PlayDetectSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_ORDE_DETECT_ANGRY001);
        }
    }

    public override void PlayDeathSound()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_ORDE_DIE_HIT007);
        }
    }
}
