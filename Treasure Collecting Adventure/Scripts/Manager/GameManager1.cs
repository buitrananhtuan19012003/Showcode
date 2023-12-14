using UnityEngine.SceneManagement;

public class GameManager1 : BaseManager<GameManager1>
{
    private void Start()
    {
        if (UIManager1.HasInstance)
        {
            UIManager1.Instance.ShowNotify<NotifyLoading>();
            NotifyLoading scr = UIManager1.Instance.GetExistNotify<NotifyLoading>();
            if (scr != null)
            {
                scr.AnimationLoaddingText();
                //scr.DoAnimationLoadingProgress(5, () =>
                //{
                //    UIManager.Instance.ShowScreen<ScreenHome>();
                //    scr.Hide();
                //});
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}