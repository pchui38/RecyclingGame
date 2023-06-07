using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinsItemGenerator : MonoBehaviour
{
    public List<char> charactersList = new List<char>() { };

    // Start is called before the first frame update
    void Start()
    {
        GenerateCharacterList();
    }
    void GenerateCharacterList()
    {
        charactersList = new List<char>();

        // Add correct characters (A, B, C)
        for (int i = 0; i < 50; i++)
        {
            charactersList.Add('A');
            charactersList.Add('B');
            charactersList.Add('C');
        }

        // Add incorrect characters (D, E, F)
        for (int i = 0; i < 50; i++)
        {
            charactersList.Add('D');
            charactersList.Add('E');
            charactersList.Add('F');
        }

        // Shuffle the list
        int n = charactersList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            char value = charactersList[k];
            charactersList[k] = charactersList[n];
            charactersList[n] = value;
        }
    }
}
