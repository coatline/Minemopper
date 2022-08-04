using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] TileBase[] numberTiles;
    [SerializeField] TileBase defaultTile;
    [SerializeField] TileBase mineTile;

    [SerializeField] Tilemap tilemap;
    [SerializeField] int minePercent;
    List<Vector2Int> openSpots;
    public Vector2Int levelSize;
    public int[,] map;

    void Start()
    {
        if (LD.levelSize.x != 0 && LD.levelSize.y != 0 && LD.levelSize.x <= 300 && LD.levelSize.y <= 300)
        {
            levelSize = LD.levelSize;
            minePercent = LD.minePercentage;
        }

        Camera.main.transform.position = new Vector3(levelSize.x / 2, levelSize.y / 2, -10);
        Camera.main.orthographicSize = levelSize.x / 2;

        map = new int[levelSize.x, levelSize.y];
        openSpots = new List<Vector2Int>();

        Generate();
        PlaceMines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Menu");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            RevealAllMines();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            CoverBack();
        }
    }

    void Generate()
    {
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), defaultTile);
                openSpots.Add(new Vector2Int(x, y));
            }
        }
    }

    void PlaceMines()
    {
        for (int i = 0; i < ((levelSize.x * levelSize.y) * (minePercent / 100f)); i++)
        {
            if (openSpots.Count > 0)
            {
                var ind = Random.Range(0, openSpots.Count);
                var pos = openSpots[ind];
                openSpots.RemoveAt(ind);

                map[pos.x, pos.y] = 1;
            }
        }
    }

    public void RevealAllMines()
    {
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), mineTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), TileToChangeTo(new Vector3Int(x, y, 0)));
                }
            }
        }
    }

    public void CoverBack()
    {
        for (int x = 0; x < levelSize.x; x++)
        {
            for (int y = 0; y < levelSize.y; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), defaultTile);
            }
        }
    }

    public TileBase TileToChangeTo(Vector3Int pos)
    {
        if (map[pos.x, pos.y] == 1) { return mineTile; }

        int mines = 0;

        for (int x = pos.x - 1; x <= pos.x + 1; x++)
            for (int y = pos.y - 1; y <= pos.y + 1; y++)
            {
                if (x < 0 || x >= levelSize.x || y < 0 || y >= levelSize.y) { continue; }
                if (map[x, y] == 1) { mines++; }
            }

        return numberTiles[mines];
    }

    //void ChangeToEmptyTile(Vector3Int pos)
    //{
    //    for (int x = pos.x - 1; x <= pos.x + 1; x++)
    //        for (int y = pos.y - 1; y <= pos.y + 1; y++)
    //        {
    //            if()
    //        }
    //}
}
