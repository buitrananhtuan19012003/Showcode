using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LupinrangerPatranger
{
    public class Finish : MonoBehaviour
    {
        private bool levelComplete = false;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Player") && !levelComplete)
            {
                if (AudioManager.HasInstance)
                {
                    AudioManager.Instance.PlaySE(AUDIO.SE_FINISH);
                }
                levelComplete = true;
                Invoke("CompleteLevel", 1f);
            }
        }

        private void CompleteLevel()
        {
            if (SceneManager.GetActiveScene().name.Equals("Level2"))
            {
                if (UIManager.HasInstance)
                {
                    Time.timeScale = 0f;
                    //UIManager.Instance.ActiveVictoryPanel(true);
                    return;
                }
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            if (UIManager.HasInstance && AudioManager.HasInstance)
            {
                //UIManager.Instance.GamePanel.SetTimeRemain(240);
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BMG_4);
            }
        }
    }
}