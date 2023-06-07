using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class LogoScreen : MonoBehaviour
{
    public float pauseTime = 0.1f;
    private GameObject GameManager;
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        GameManager.GetComponent<GameManager>().currentMode = GameMode.GameLogo;
    }

    public void OnClickPlayGame()
    {
        StartCoroutine(LoadNextScreen());
    }

    IEnumerator LoadNextScreen()
    {
        yield return new WaitForSeconds(pauseTime);

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "P1_LogoScreen")
            SceneManager.LoadScene("P2_UserSelectScreen");
    }
}
