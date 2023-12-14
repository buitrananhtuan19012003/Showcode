using LupinrangerPatranger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupVictory : BasePopup
{
    private void OnEnable()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_WIN);
        }
    }

    public override void Show(object data)
    {
        base.Show(data);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickedGameButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.RestarGame();
        }
    }

    public void OnClickedExitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}