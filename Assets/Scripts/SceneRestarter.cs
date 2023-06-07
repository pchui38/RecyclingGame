using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneRestarter : MonoBehaviour
{
    public void RestartScene()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeSceneIndex);
    }
}
