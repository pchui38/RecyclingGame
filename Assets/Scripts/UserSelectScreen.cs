using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using static GameManager;

public class UserSelectScreen : MonoBehaviour
{
    public float pauseTime = 0.3f;
    public TMP_InputField NameInput;
    public TMP_Dropdown AgeDropdown;
    public GameObject missingNameMsg;
    public GameObject missingAgeMsg;
    public bool loadNextPageError = false;
    private string ageDropDownSelectedValue = "";
    private GameObject GameManager;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        GameManager.GetComponent<GameManager>().currentMode = GameMode.GameUserSelect;

        if (missingNameMsg != null && missingAgeMsg != null)
        {
            missingNameMsg.SetActive(false);
            missingAgeMsg.SetActive(false);
        }

        if (AgeDropdown != null)
            AgeDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int value)
    {
        ageDropDownSelectedValue = AgeDropdown.options[value].text;
    }
    public void OnClickLetsGo()
    {
        // Check for Missing nickname & age selection
        if (NameInput != null && string.IsNullOrEmpty(NameInput.text))
        {
            missingNameMsg.SetActive(true);
            loadNextPageError = true;
            StartCoroutine(ResetAndHideErrorMessage(3f));
        }

        if (string.IsNullOrEmpty(ageDropDownSelectedValue))
        {
            missingAgeMsg.SetActive(true);
            loadNextPageError = true;
            StartCoroutine(ResetAndHideErrorMessage(3f));
        }

        if (!loadNextPageError)
        {
            // Save Nickname & Age in persistent memory
            GameManager.GetComponent<GameManager>().nickname = NameInput.text;
            GameManager.GetComponent<GameManager>().ageGroup = ageDropDownSelectedValue;
            StartCoroutine(LoadNextScreen());
        }

    }

    IEnumerator LoadNextScreen()
    {
        yield return new WaitForSeconds(pauseTime);

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "P2_UserSelectScreen" && !loadNextPageError)
            SceneManager.LoadScene("P3_GameScreen");
    }

    IEnumerator ResetAndHideErrorMessage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        missingNameMsg.SetActive(false);
        missingAgeMsg.SetActive(false);
        loadNextPageError = false;
    }
}
