using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;
    private float timeRemaining;
    private bool timerIsRunning = false;

    private void Awake()
    {
        //SetTimeRemain(120);
    }

    private void Start()
    {
        Invoke("DelayLoad", 0.3f);
    }

    private void OnEnable()
    {
        //SetTimeRemain(120);
        //timerIsRunning = true;
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                if (UIManager.HasInstance && GameManager.HasInstance && AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_LOSE);
                    GameManager.Instance.PauseGame();
                    UIManager.Instance.ActiveLosePanel(true);
                }
            }
        }
    }

    private void OnPlayerCollect(int value)
    {
        
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetTimeRemain(float v)
    {
        timeRemaining = v;
    }

    private void DelayLoad()
    {
        
    }
}