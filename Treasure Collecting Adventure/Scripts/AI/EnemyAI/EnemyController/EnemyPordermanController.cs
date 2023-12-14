using UnityEngine;

public class EnemyPordermanController : EnemyController
{
    public override void PlayDetectSound()
    {
        if (AudioManager.HasInstance)
        {
            int random = Random.Range(0, 4);
            if (random == 0)
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DETECT_JAYHI);     
            else if (random == 1)
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DETECT_JGAAAH);        
            else if (random == 2)
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DETECT_NAHHGG);       
            else
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DETECT_NIEAAGGG);
        }
    }

    public override void PlayDeathSound()
    {
        if (AudioManager.HasInstance)
        {
            int random = Random.Range(0, 3);
            if (random == 0)
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DIE_DIE01);
            else if (random == 1)
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DIE_DIE03);
            else
                AudioManager.Instance.PlaySE(AUDIO.SE_GOBLIN_DIE_DIE04);
        }
    }
}
