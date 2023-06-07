using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinsManager : MonoBehaviour
{
    public List<char> selectedCharacters = new List<char>();
    public Text BinItemsName;
    public Text Accept1;
    public Text Accept2;
    public Text Accept3;
    public Text RejectX;
    public Image BinItemsBkg;
    private GameObject BinsItemManager;

    public Vector3 uiItemsOffset = new Vector3(0, 0, 0);
    public Vector3 uiBkgOffset = new Vector3(0, 0, 0);

    private RectTransform uiItemsRectTransform;
    private RectTransform uiBkgRectTransform;
    private RectTransform uiRejectXRectTransform;
    private RectTransform uiAccept1RectTransform;
    private RectTransform uiAccept2RectTransform;
    private RectTransform uiAccept3RectTransform;

    // Start is called before the first frame update
    void Start()
    {
        BinsItemManager = GameObject.Find("BinsItemGenerator");
        DisplayRandomBinItems();

        uiItemsRectTransform = BinItemsName.GetComponent<RectTransform>();
        uiBkgRectTransform = BinItemsBkg.GetComponent<RectTransform>();
        uiRejectXRectTransform = RejectX.GetComponent<RectTransform>();
        uiAccept1RectTransform = Accept1.GetComponent<RectTransform>();
        uiAccept2RectTransform = Accept2.GetComponent<RectTransform>();
        uiAccept3RectTransform = Accept3.GetComponent<RectTransform>();

        // Hide Bin items name & its background
        BinItemsName.gameObject.SetActive(false);
        RejectX.gameObject.SetActive(false);
        BinItemsBkg.gameObject.SetActive(false);
        Accept1.gameObject.SetActive(false);
        Accept2.gameObject.SetActive(false);
        Accept3.gameObject.SetActive(false);
    }

    private void DisplayRandomBinItems()
    {
        // Take out 3 random characters from the List of SelectedCharacters
        //List<char> selectedCharacters = new List<char>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, BinsItemManager.GetComponent<BinsItemGenerator>().charactersList.Count);
            char selectedCharacter = BinsItemManager.GetComponent<BinsItemGenerator>().charactersList[randomIndex];
            selectedCharacters.Add(selectedCharacter);
            BinsItemManager.GetComponent<BinsItemGenerator>().charactersList.RemoveAt(randomIndex);
        }

        // Display each character accordingly
        foreach (char character in selectedCharacters)
        {
            BinItemsName.text += character.ToString() + "\n";

            /*
            if (character == 'A' || character == 'B' || character == 'C')
            {

                Debug.Log("Correct character: " + character);
                // Display it in the desired way for correct characters
            }
            else if (character == 'D' || character == 'E' || character == 'F')
            {
                Debug.Log("Incorrect character: " + character);
                // Display it in the desired way for incorrect characters
            }
            */
        }
    }


    void LateUpdate()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        uiItemsRectTransform.position = screenPosition + uiItemsOffset;
        uiRejectXRectTransform.position = screenPosition + uiItemsOffset;
        uiAccept1RectTransform.position = screenPosition + uiItemsOffset;
        uiAccept2RectTransform.position = uiAccept1RectTransform.position;
        uiAccept3RectTransform.position = uiAccept1RectTransform.position;
        uiBkgRectTransform.position = screenPosition + uiBkgOffset;
    }
}
