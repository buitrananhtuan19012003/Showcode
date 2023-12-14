using UnityEngine;

public class PopoutContainer : BaseUIElement
{
    [Header("POPOUT CONTAINER")]
    [SerializeField] private CanvasGroup mm_PlayPopout;
    [SerializeField] private CanvasGroup mm_QuitPopout;
    [SerializeField] private CanvasGroup cs_StartPopout;
    [SerializeField] private CanvasGroup ig_MainMenuPopout;
    [SerializeField] private CanvasGroup ig_QuitPopout;
    [SerializeField] private CanvasGroup ig_TravelDungeonPopout;

    protected virtual void OnEnable()
    {
        this.Hide();
    }

    public override void Show(object data)
    {
        base.Show(data);
        //if (InputManager.HasInstance)
        //{
        //    if (InputManager.Instance.Input_ChrSelScene.enabled)
        //    {
        //        InputManager.Instance.Input_ChrSelScene.CanSelect = false;
        //    }
        //}

        if (PlayerCtrl.HasInstance)
        {
            PlayerCtrl.Instance.PlayerInput.enabled = false;
        }

    }

    public override void Hide()
    {
        base.Hide();
        this.HideAllPopout();
        //if (InputManager.HasInstance)
        //{
        //    if (InputManager.Instance.Input_ChrSelScene.enabled)
        //    {
        //        InputManager.Instance.Input_ChrSelScene.CanSelect = true;
        //    }
        //}

        if (PlayerCtrl.HasInstance)
        {
            PlayerCtrl.Instance.PlayerInput.enabled = true;
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }
    }

    private void HideAllPopout()
    {
        this.SetActiveCanvasGroup(this.mm_PlayPopout, false);
        this.SetActiveCanvasGroup(this.mm_QuitPopout, false);
        this.SetActiveCanvasGroup(this.cs_StartPopout, false);
        this.SetActiveCanvasGroup(this.ig_MainMenuPopout, false);
        this.SetActiveCanvasGroup(this.ig_QuitPopout, false);
        this.SetActiveCanvasGroup(this.ig_TravelDungeonPopout, false);
    }

    protected virtual void SetActiveCanvasGroup(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.blocksRaycasts = isActive;
    }

    public void ShowMM_StartPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.mm_PlayPopout, true);
    }
    public void ShowMM_QuitPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.mm_QuitPopout, true);
    }
    public void ShowCS_StartPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.cs_StartPopout, true);
    }
    public void ShowIG_MainMenuPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.ig_MainMenuPopout, true);
    }
    public void ShowIG_QuitPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.ig_QuitPopout, true);
    }
    public void ShowIG_TravelDungeonPopout()
    {
        this.Show(null);
        this.SetActiveCanvasGroup(this.ig_TravelDungeonPopout, true);
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public virtual void OnClickNoButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_CLICKS);
        }

        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.IsShowCursor(true);
        }
    }

    public virtual void OnClickPlay_YesButton() //MainMenu
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_CLICKS);
        }

        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.StartGame();
        }
    }

    public virtual void OnClickMainMenu_YesButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_CLICKS);
        }

        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.MainMenu();
        }
    }

    public virtual void OnClickStart_YesButton() //Character Selection
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_CLICKS);
        }

        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.TravelToVillage();
        }
        //if (InputManager.Instance.Input_ChrSelScene.enabled)
        //{
        //    InputManager.Instance.Input_ChrSelScene.CanSelect = false;
        //    InputManager.Instance.Disable_Input_All();
        //}
    }

    public virtual void OnClickTravelDungeon_YesButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySE(AUDIO.SE_BTN_CLICKS);
        }
        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.TravelToDungeon();
        }
    }

    public virtual void OnClickQuit_YesButton()
    {
        this.Hide();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.EndGame();
        }
    }
}
