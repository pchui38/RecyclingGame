using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameManager;

public class GameScreen : MonoBehaviour
{
    // Garbage Truck
    public GameObject Player;

    // ReadyGo button & Pause bkg
    public Button ReadyGoButton;
    public Image PauseBkg;

    // UI Button Press
    public Button AcceptButton;
    public Button RejectButton;
    public Button RestartGameButton;

    // UI Points, Contamination
    public TMP_Text ScoreText;

    public ContaminationBar contaminationBar;
    public Image PopupMessageBox;
    public TMP_Text PopupText;
    public TMP_Text ContaminationNewAmt;
    public Image GameOverResultPanel;

    // Game Result UI
    public TMP_Text NicknameResult;
    public TMP_Text PointsResult;
    public TMP_Text ContaminationResult;
    public TMP_Text FinalScoreResult;

    // Swipe Up or Down
    public bool isSwipeEnabled = false;
    private Vector2 swipeStartPosition;
    private bool isSwipeStarted;
    public float swipeThreshold = 100f;

    // Has User swiped or pressed up/down
    public bool hasUserSwipeOrPressed = false;

    private GameObject GameManager;

    public float countdownDuration = 3f; // Duration of the countdown in seconds
    public TMP_Text countdownText; // Reference to the UI text object

    private float currentTime;


    void Start()
    {
        // Set Game Mode to GameStart
        GameManager = GameObject.Find("GameManager");
        if (GameManager != null)
            GameManager.GetComponent<GameManager>().currentMode = GameMode.GamePause;

        // Enable ReadyGo Button & Pause bkg
        if (ReadyGoButton != null)
        {
            ReadyGoButton.gameObject.SetActive(true);
            ReadyGoButton.interactable = true;
        }
        if (PauseBkg != null)
        {
            PauseBkg.gameObject.SetActive(true);
        }

        // Disable all buttons
        DisableButtons();

        contaminationBar.SetContamination(100);

        // Make Popup Message box transparent
        ChangeAlpha(PopupMessageBox.gameObject.GetComponent<Image>(), 0f);

        ContaminationNewAmt.text = "";

        // Hide GameOverResultPanel
        GameOverResultPanel.gameObject.SetActive(false);

        // Add RestartScene listener
        RestartGameButton.onClick.AddListener(RestartGame);
    }
    private void RestartGame()
    {
        SceneRestarter sceneRestarter = FindObjectOfType<SceneRestarter>();
        sceneRestarter.RestartScene();
    }

    public void OnClickReadyGo()
    {
        // Disable ReadyGo Button & Pause bkg
        ReadyGoButton.gameObject.SetActive(false);
        PauseBkg.gameObject.SetActive(false);

        // Count down 3,2,1,Go then start game
        StartTimer();
    }

    private void StartTimer()
    {
        currentTime = countdownDuration;
        InvokeRepeating("UpdateTimer", 0f, 1f);
    }

    private void UpdateTimer()
    {
        if (currentTime > 0)
        {
            countdownText.text = currentTime.ToString();
            currentTime--;
        }
        else if (currentTime == 0)
        {
            countdownText.text = "Go!";
            currentTime--;
        }
        else
        {
            CancelInvoke("UpdateTimer");
            countdownText.gameObject.SetActive(false);
            // Start the game or perform any other actions here
            if (GameManager != null)
                GameManager.GetComponent<GameManager>().currentMode = GameMode.GameRun;
        }
    }


    void Update()
    {
        Debug.Log("points: " + Player.GetComponent<Player>().points);

        if (GameManager.GetComponent<GameManager>().currentMode == GameMode.GameRun)
        {
            if (isSwipeEnabled)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    swipeStartPosition = Input.mousePosition;
                    isSwipeStarted = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    // Calculate the swipe direction and distance
                    Vector2 swipeEndPosition = Input.mousePosition;
                    Vector2 swipeDirection = swipeEndPosition - swipeStartPosition;
                    float swipeMagnitude = swipeDirection.magnitude;

                    if (isSwipeStarted && swipeMagnitude >= swipeThreshold)
                    {
                        // Swipe was long enough, determine the swipe direction
                        if (swipeDirection.y > 0)
                        {
                            // Swipe up
                            Debug.Log("Swipe Up detected!");
                            hasUserSwipeOrPressed = true;
                            Player.GetComponent<Player>().AcceptBins();
                        }
                        else
                        {
                            // Swipe down
                            Debug.Log("Swipe Down detected!");
                            hasUserSwipeOrPressed = true;
                            Player.GetComponent<Player>().RejectBins();
                        }
                    }
                    isSwipeStarted = false;
                }
            }
        }
        else if (GameManager.GetComponent<GameManager>().currentMode == GameMode.GameOver ||
            GameManager.GetComponent<GameManager>().currentMode == GameMode.GameFinish)
        {
            // Show Game Over result UI
            GameOverResultPanel.gameObject.SetActive(true);
        }
    }

    public IEnumerator DisplayPopupText(float displayDuration, string displayText, Color color)
    {
        // Show Popup Message box
        ChangeAlpha(PopupMessageBox.gameObject.GetComponent<Image>(), 1f);
        PopupText.text = displayText;
        PopupText.color = color;

        yield return new WaitForSeconds(displayDuration);

        // Make Popup Message box transparent
        ChangeAlpha(PopupMessageBox.gameObject.GetComponent<Image>(), 0f);
        PopupText.text = "";
    }
    public void ChangeAlpha(Image image, float alpha)
    {
        Color currentColor = image.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        image.color = newColor;
    }

    public void EnableButtons()
    {
        // Enable all buttons
        if (AcceptButton != null)
        {
            AcceptButton.interactable = true;
        }

        if (RejectButton != null)
        {
            RejectButton.interactable = true;
        }
    }
    public void DisableButtons()
    {
        // Disable all buttons
        if (AcceptButton != null)
        {
            AcceptButton.interactable = false;
        }

        if (RejectButton != null)
        {
            RejectButton.interactable = false;
        }
    }

    public void OnClickAccept()
    {
        hasUserSwipeOrPressed = true;

        Player.GetComponent<Player>().AcceptBins();
    }
    
    public void OnClickReject()
    {
        hasUserSwipeOrPressed = true;
        Player.GetComponent<Player>().RejectBins();
    }

    //public void OnClickRestartGame()
    //{
    //    StartCoroutine(LoadNextScreen());
    //}

    //IEnumerator LoadNextScreen()
    //{
    //    yield return new WaitForSeconds(pauseTime);

    //    Scene scene = SceneManager.GetActiveScene();
    //    if (scene.name == "P2_UserSelectScreen" && !loadNextPageError)
    //        SceneManager.LoadScene("P3_GameScreen");
    //}
}
