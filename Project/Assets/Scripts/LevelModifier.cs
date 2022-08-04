using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelModifier : MonoBehaviour
{
    [SerializeField] TileBase[] numberTiles;

    [SerializeField] TileBase flaggedTile;
    [SerializeField] TileBase defaultTile;
    [SerializeField] TileBase mineTile;
    [SerializeField] LevelGenerator lg;
    [SerializeField] Tilemap tilemap;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cellPos = CurrentCellPosition();

            if (!WithinBounds(new Vector2(cellPos.x, cellPos.y))) { return; }

            TileBase newTile = lg.TileToChangeTo(cellPos);

            if (newTile == numberTiles[0])
            {
                FloodEmptyTiles(cellPos);
            }
            else if (newTile == mineTile)
            {
                lg.RevealAllMines();
            }
            else
            {
                tilemap.SetTile(cellPos, newTile);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            var cellPos = CurrentCellPosition();

            if (!WithinBounds(new Vector2(cellPos.x, cellPos.y))) { return; }

            if (tilemap.GetTile(cellPos) == flaggedTile)
            {
                tilemap.SetTile(cellPos, defaultTile);
                return;
            }
            else if (tilemap.GetTile(cellPos) != defaultTile)
            {
                return;
            }

            tilemap.SetTile(cellPos, flaggedTile);
        }
    }

    void FloodEmptyTiles(Vector3Int pos)
    {
        tilemap.SetTile(pos, lg.TileToChangeTo(pos));

        for (int x = pos.x - 1; x <= pos.x + 1; x++)
            for (int y = pos.y - 1; y <= pos.y + 1; y++)
            {
                if (!WithinBounds(new Vector2(x, y))) { continue; }

                if (tilemap.GetTile(new Vector3Int(x, y, 0)) == defaultTile && lg.TileToChangeTo(pos) == numberTiles[0])
                {
                    FloodEmptyTiles(new Vector3Int(x, y, 0));
                }
            }
    }

    Vector3Int CurrentCellPosition()
    {
        var p = cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3Int((int)p.x, (int)p.y, 0);
    }

    bool WithinBounds(Vector2 pos)
    {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < lg.levelSize.x && pos.y < lg.levelSize.y);
    }
}
