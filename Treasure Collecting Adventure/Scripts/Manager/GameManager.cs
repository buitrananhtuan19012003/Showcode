using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    [SerializeField] private PlayerCtrl playerCtrl;
    [SerializeField] private CharacterDataSO characterData;

    private bool isPlaying = false;

    public PlayerCtrl PlayerCtrl { get => this.playerCtrl; }
    public CharacterDataSO CharacterData { get => this.characterData; set => this.characterData = value; }
    public bool IsPlaying { get => this.isPlaying; }

    //protected override void LoadComponent()
    //{
    //    base.LoadComponent();
    //    if (this.playerCtrl == null)
    //        this.playerCtrl = GetComponentInChildren<PlayerCtrl>();
    //}

    public void StartGame()
    {
        this.isPlaying = true;
        this.characterData = null;
        StartCoroutine(this.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void PauseGame()
    {
        if (!this.isPlaying) return;

        this.isPlaying = false;
        Time.timeScale = 0.0f;

        this.IsShowCursor(true);
    }

    public void ResumeGame()
    {
        this.isPlaying = true;
        Time.timeScale = 1.0f;

        this.IsShowCursor(false);
    }

    public void IsShowCursor(bool showCursor)
    {
        if (showCursor)
        {
            //Unlock & show cursor (PauseMenuCanvas enable)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            //Lock & hide cursor (PauseMenuCanvas disable)
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void MainMenu()
    {
        this.characterData = null;
        this.playerCtrl.gameObject.SetActive(false);
        this.playerCtrl.ResetPlayer();

        this.ResumeGame();
        StartCoroutine(this.LoadScene((int)SceneIndex.MainMenu));
    }

    public void RestarGame()
    {
        ChangeScene("Menu");

        if (UIManager.HasInstance)
        {
            UIManager.Instance.ActiveVictoryPanel(false);
            UIManager.Instance.ActiveGamePanel(false);
            UIManager.Instance.ActiveLosePanel(false);
            UIManager.Instance.ActiveMenuPanel(true);
        }
    }

    public void EndGame()
    {
        this.playerCtrl.gameObject.SetActive(false);
        this.playerCtrl.ResetPlayer();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (GameManager.HasInstance)
            {
                GameManager.Instance.TravelToVillage();
            }
        }
    }
    public void TravelToVillage()
    {
        if (this.playerCtrl.Character != null)
            this.playerCtrl.PlayerWeapon.PlayerWeaponManager.SaveWeapon();

        StartCoroutine(this.LoadScene((int)SceneIndex.Village));
        this.ResumeGame();
    }

    public void TravelToDungeon()
    {
        this.playerCtrl.PlayerWeapon.PlayerWeaponManager.SaveWeapon();

        StartCoroutine(this.LoadScene((int)SceneIndex.Dungeon));
        this.ResumeGame();
        this.playerCtrl.PlayerInput.enabled = true;
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        if (UIManager.HasInstance)
        {
            //UIManager.Instance.AnimatorTransition.SetTrigger("End");
            yield return new WaitForSeconds(1.25f);

            //UIManager.Instance.Enable_UI_LoadingPanel();
            //StartCoroutine(UIManager.Instance.LoadingPanel.LoadScene(sceneIndex));
            yield return null;
        }
    }

    public void GenerateCharacter(Character character)
    {
        if (this.characterData == null) return;

        this.playerCtrl.gameObject.SetActive(false);
        this.playerCtrl.SetCharacter(character);
        character.CharacterRigAttach.SetRig();
        this.playerCtrl.gameObject.SetActive(true);

    }
}