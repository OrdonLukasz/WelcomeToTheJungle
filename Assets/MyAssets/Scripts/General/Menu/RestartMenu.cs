using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMenu : MonoBehaviour
{
     public GameObject RestartMenuUi;
     
    public void ActivationRestartMenu()
    {
        Time.timeScale = 0f;
        RestartMenuUi.SetActive(true);
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        RestartMenuUi.SetActive(false);
        SceneManager.LoadScene("Game");
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
