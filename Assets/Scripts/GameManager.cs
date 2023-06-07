using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameMode
    {
        GameLogo,
        GameUserSelect,
        GamePause,
        GameRun,
        GameFinish,
        GameOver
    }
    public GameMode currentMode;
    public static GameManager Instance;

    public string nickname = "";
    public string ageGroup = "";

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Set the initial game mode to GameStart
        currentMode = GameMode.GameLogo;
    }

    private void Update()
    {
        
    }
}
