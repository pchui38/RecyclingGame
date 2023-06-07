using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Player : MonoBehaviour
{
    public GameObject GameManager;
    public GameObject ZigZagRoad;
    public GameObject ScreenLoader;
    private GameObject currentBins;

    // Points & Final score 
    public int points = 0;
    public int finalScore = 0;

    // Contamination
    public int contaminationMultiplier = 100;
    public int currentContamination = 0;

    // Garbage truck
    public float TruckSpeed_Fast = 0.3f;
    public float TruckSpeed_Medium = 0.2f;
    public float TruckSpeed_Slow = 0.1f;
    public float speedFactor = 1f;
    Vector3 nextRoadTile;
    Transform startPoint;
    Transform endPoint;
    float t;
    public int binsCount = 0;

    // Camera
    public Camera Cam;
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        ZigZagRoad = GameObject.Find("ZigZagRoad");
        ScreenLoader = GameObject.Find("ScreenLoader");

        ZigZagRoad.GetComponent<ZigZagRoad>().InstantiateRoadTile();

        // Set Garbage Truck position at mid of Tiles
        this.transform.position = ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles[(ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles.Count - 1) / 2].transform.position;

        // Initialise Start point & End point
        startPoint = ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles[(ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles.Count - 1) / 2].transform;
        endPoint = ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles[(ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles.Count - 1) / 2 + 1].transform;

        RotateTruck();

        contaminationMultiplier = 100;
        currentContamination = 0;
    }

    private void Update()
    {  
        if (GameManager.GetComponent<GameManager>().currentMode == GameMode.GamePause)
        {

        }
        else if (GameManager.GetComponent<GameManager>().currentMode == GameMode.GameRun)
        {
            ZigZagRoad.GetComponent<ZigZagRoad>().UpdateRoadTile(this.transform.position);

            // When the player has passed 30 houses
            if (binsCount <= 30)
            {
                t += TruckSpeed_Medium * Time.deltaTime * speedFactor;
                t = Mathf.Clamp01(t);
                this.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

                if (t >= 1f)
                {
                    t = 0f;
                    // Initialise Start point & End point

                    startPoint = ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles[(ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles.Count - 1) / 2 - 1].transform;
                    endPoint = ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles[(ZigZagRoad.GetComponent<ZigZagRoad>().RoadTiles.Count - 1) / 2].transform;

                    RotateTruck();
                }
            }
            else
            {
                GameManager.GetComponent<GameManager>().currentMode = GameMode.GameFinish;
                // Display Game Finish nickname result
                ScreenLoader.GetComponent<GameScreen>().NicknameResult.text = "Well Done " + GameManager.GetComponent<GameManager>().nickname;
            }
        }
        else if (GameManager.GetComponent<GameManager>().currentMode == GameMode.GameOver)
        {
            // Calculate & Show Game Result on Game Screen UI
            ScreenLoader.GetComponent<GameScreen>().NicknameResult.text = "Game Over " + GameManager.GetComponent<GameManager>().nickname;
            ScreenLoader.GetComponent<GameScreen>().PointsResult.text = "Points: " + points;
            if (contaminationMultiplier < 0)
                contaminationMultiplier = 0;
            int revertedContamination = 100 - contaminationMultiplier;
            ScreenLoader.GetComponent<GameScreen>().ContaminationResult.text = "Contamination: " + revertedContamination + "%";
            ScreenLoader.GetComponent<GameScreen>().FinalScoreResult.text = " Your final score: " + points * revertedContamination;
        }

    }

    private void LateUpdate()
    {
        // Update the camera's position to match the car's position
        Cam.transform.position = this.transform.position + offset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bins"))
        {
            // Reduce Truck speed to half
            speedFactor *= 0.5f;

            // Increment Bins count
            binsCount += 1;

            // Reset Current Bins contamination to 0
            currentContamination = 0;


            // Enable UI Bin items text & bkg
            other.gameObject.GetComponent<BinsManager>().BinItemsName.gameObject.SetActive(true);
            currentBins = other.gameObject;
            other.gameObject.GetComponent<BinsManager>().BinItemsBkg.gameObject.SetActive(true);


            //Enable Collect & Reject Button
            ScreenLoader.GetComponent<GameScreen>().EnableButtons();

            // Enable swipe up or down
            ScreenLoader.GetComponent<GameScreen>().isSwipeEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Set Truck to original speed
        speedFactor *= 2f;

        // Enable UI Bin items text & bkg
        other.gameObject.GetComponent<BinsManager>().BinItemsName.gameObject.SetActive(false);
        other.gameObject.GetComponent<BinsManager>().RejectX.gameObject.SetActive(false);
        other.gameObject.GetComponent<BinsManager>().Accept1.gameObject.SetActive(false);
        other.gameObject.GetComponent<BinsManager>().Accept2.gameObject.SetActive(false);
        other.gameObject.GetComponent<BinsManager>().Accept3.gameObject.SetActive(false);
        other.gameObject.GetComponent<BinsManager>().BinItemsBkg.gameObject.SetActive(false);

        // Disable Button
        ScreenLoader.GetComponent<GameScreen>().DisableButtons();

        // D 10 points for correct swipe or button press
        if (ScreenLoader.GetComponent<GameScreen>().hasUserSwipeOrPressed)
        {
            //points += 10;
            // Display points on Popup message
            //StartCoroutine(DisplayPopupText(1f, "+10", Color.green));
        }
        else
        {
            points -= 20;
            // Display points on Popup message
            StartCoroutine(ScreenLoader.GetComponent<GameScreen>().DisplayPopupText(1f, "-20", Color.red));
            if (points < 0)
                points = 0;
        }

        // Display points on UI Score & Popup message
        ScreenLoader.GetComponent<GameScreen>().ScoreText.text = points.ToString() + " / 300";

        // Disable swipe up or down
        ScreenLoader.GetComponent<GameScreen>().isSwipeEnabled = false;
        // Reset bool for hasUserSwipeOrPressed
        ScreenLoader.GetComponent<GameScreen>().hasUserSwipeOrPressed = false;
    }


    public void AcceptBins()
    {
        // Add 10 points for correctly swipe / press up or down on a bin
        int newPointsToBeAdded = 10;

        // Accept current Bins by play animation
        currentBins.GetComponent<BinsManager>().Accept1.gameObject.SetActive(true);
        currentBins.GetComponent<BinsManager>().Accept2.gameObject.SetActive(true);
        currentBins.GetComponent<BinsManager>().Accept3.gameObject.SetActive(true);

        if (currentBins.GetComponent<BinsManager>().selectedCharacters[0] == 'A' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[0] == 'B' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[0] == 'C')
        {
            currentBins.GetComponent<BinsManager>().Accept1.gameObject.GetComponent<Text>().color = Color.green;
        }
        else
        {
            currentBins.GetComponent<BinsManager>().Accept1.gameObject.GetComponent<Text>().color = Color.red;
            // Deduct 5 points for incorrect bin item
            newPointsToBeAdded -= 5;
            // Add contamination 10%
            currentContamination += 10;
        }

        if (currentBins.GetComponent<BinsManager>().selectedCharacters[1] == 'A' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[1] == 'B' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[1] == 'C')
        {
            currentBins.GetComponent<BinsManager>().Accept2.gameObject.GetComponent<Text>().color = Color.green;
        }
        else
        {
            currentBins.GetComponent<BinsManager>().Accept2.gameObject.GetComponent<Text>().color = Color.red;
            // Deduct 5 points for incorrect bin item
            newPointsToBeAdded -= 5;
            // Add contamination 10%
            currentContamination += 10;
        }

        if (currentBins.GetComponent<BinsManager>().selectedCharacters[2] == 'A' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[2] == 'B' ||
            currentBins.GetComponent<BinsManager>().selectedCharacters[2] == 'C')
        {
            currentBins.GetComponent<BinsManager>().Accept3.gameObject.GetComponent<Text>().color = Color.green;
        }
        else
        {
            currentBins.GetComponent<BinsManager>().Accept3.gameObject.GetComponent<Text>().color = Color.red;
            // Deduct 5 points for incorrect bin item
            newPointsToBeAdded -= 5;
            // Add contamination 10%
            currentContamination += 10;
        }

        // Play animation for Contamination
        ScreenLoader.GetComponent<GameScreen>().contaminationBar.SetAnimatedContamination(contaminationMultiplier, contaminationMultiplier - currentContamination);
        // Display new Contamination amount on UI
        if (currentContamination > 0)
            StartCoroutine(DisplayNewContaminationAmtMessage(1f));
        // Add current Bins contamination to the Total contamination
        contaminationMultiplier -= currentContamination;

        if (contaminationMultiplier <= 0)
        {
            // Change to Game Over mode
            if (GameManager != null)
                GameManager.GetComponent<GameManager>().currentMode = GameMode.GameOver;
        }
 
        // Display new points added or deducted on Popup text
        if (newPointsToBeAdded < 0)
            StartCoroutine(ScreenLoader.GetComponent<GameScreen>().DisplayPopupText(1f, newPointsToBeAdded.ToString(), Color.red));
        else if (newPointsToBeAdded > 0)
            StartCoroutine(ScreenLoader.GetComponent<GameScreen>().DisplayPopupText(1f, "+" + newPointsToBeAdded, Color.green));

        // Display points on UI Score & Popup message
        points += newPointsToBeAdded;
        if (points <= 0)
        {
            points = 0;
        }
        else
        {
            StartCoroutine(UpdateScoreText());
        }
    }

    private IEnumerator UpdateScoreText()
    {
        ScreenLoader.GetComponent<GameScreen>().ScoreText.text = points.ToString() + " / 300";
        
        yield return new WaitForSeconds(0.1f);
        ScreenLoader.GetComponent<ColorAndFontSizeAnimation>().StartAnimation();
    }

    private IEnumerator DisplayNewContaminationAmtMessage(float seconds)
    {
        ScreenLoader.GetComponent<GameScreen>().ContaminationNewAmt.text = "-" + currentContamination + "%";
        yield return new WaitForSeconds(seconds);
        ScreenLoader.GetComponent<GameScreen>().ContaminationNewAmt.text = "";
    }


    public void RejectBins()
    {
        // Add 10 points for correctly swipe / press up or down on a bin
        points += 10;

        StartCoroutine(UpdateScoreText());

        // Reject current Bins
        currentBins.GetComponent<BinsManager>().RejectX.gameObject.SetActive(true);
    }

    private void RotateTruck()
    {
        // Rotate the truck to follow direction of road
        Vector3 direction = endPoint.position - startPoint.position;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        this.transform.rotation = targetRotation;
    }
}
