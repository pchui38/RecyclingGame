using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagRoad : MonoBehaviour
{
    // Road
    public List<GameObject> RoadTiles;

    [SerializeField]
    private GameObject RoadTilePrefab;

    [SerializeField]
    private GameObject LeftBinsPrefab;

    [SerializeField]
    private GameObject RightBinsPrefab;
    private Vector3 lastDirection = Vector3.right;
    private Vector3 nextDirection = Vector3.right;

    public void InstantiateRoadTile()
    {
        RoadTiles = new List<GameObject>() { Instantiate(RoadTilePrefab, new Vector3(3, 0, 3), Quaternion.identity) };

        for (int i = 0; i < 29; i++)
        {
            // Add Road Tile
            AddNewRoadTile(RoadTilePrefab);
        }
    }

    public void UpdateRoadTile(Vector3 position)
    {
        float d = Vector3.Distance(position, RoadTiles[(RoadTiles.Count - 1) / 2].transform.position);

        if (d < 2)
        {
            AddNewRoadTile(RoadTilePrefab);
        }
    }

    private void AddNewRoadTile(GameObject RoadORBinPrefab)
    {
        if (RoadTiles.Count % 3 == 0)
        {
            lastDirection = nextDirection;
            nextDirection = (UnityEngine.Random.Range(0, 2) == 1 ? Vector3.right : Vector3.forward);
        }

        if (RoadTiles.Count % 3 != 0)
        {
            RoadTiles.Add(Instantiate(RoadORBinPrefab, RoadTiles[RoadTiles.Count - 1].transform.position + nextDirection * 3, Quaternion.identity));
        }
        else if (nextDirection == Vector3.forward)
        {
            RoadTiles.Add(Instantiate(RightBinsPrefab, RoadTiles[RoadTiles.Count - 1].transform.position + nextDirection * 3, Quaternion.identity));
        }
        else
        {
            RoadTiles.Add(Instantiate(LeftBinsPrefab, RoadTiles[RoadTiles.Count - 1].transform.position + nextDirection * 3, Quaternion.identity));
        }

        if (RoadTiles.Count >= 300)
        {
            Destroy(RoadTiles[0]);
            RoadTiles.RemoveAt(0);            
        }
    }    
}
