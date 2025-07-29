using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [Header("地雷个数")]
    public int minesCount;

    private const int TileSize = 32;
    private const int TilesRow = 10;
    private const int TilesCol = 10;

    public Tile[,] grid = new Tile[TilesRow, TilesCol];
    public List<Tile> mines = new();

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        
        for (int j = 0; j < TilesRow; j++)
        {
            for (int i = 0; i < TilesCol; i++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(i * TileSize, j * TileSize, 0), Quaternion.identity);
                tileObj.transform.SetParent(transform);
                grid[j, i] = tileObj.GetComponent<Tile>();
                grid[j, i].SetPosition(i, j);
            }
        }
        
        Reset();
        
        CountNearbyMines();
    }

    // 随机放置地雷
    private void Reset()
    {
        for (int j = 0; j < TilesRow; j++)
        {
            for (int i = 0; i < TilesCol; i++)
            {
                grid[j, i].Reset();
            }
        }

        for (int i = 0; i < minesCount; i++)
        {
            Tile mineTile;
            do
            {
                mineTile = grid[Random.Range(0, TilesRow), Random.Range(0, TilesCol)];
            } while (mineTile.isMine);
            // 添加地雷
            mineTile.SetMine();
            // 用于游戏失败时地雷的全部显示
            mines.Add(mineTile);
        }
    }

    // 统计周围的地雷数
    private void CountNearbyMines()
    {
        for (int j = 0; j < TilesRow; j++)
        {
            for (int i = 0; i < TilesCol; i++)
            {
                Tile tile = grid[j, i];
                if(tile.isMine)
                    continue;
                tile.nearbyMines = CountOneTile(tile);
            }
        }
    }

    // 统计一个格子周围的地雷数
    private int CountOneTile(Tile tile)
    {
        int count = 0;
        
        for (int j = -1; j <= 1; j++)
        {
            for (int i = -1; i <= 1; i++)
            {
                if (j == 0 && i == 0)
                    continue;
                
                (int newJ, int newI) = (tile.y + j, tile.x + i);
                if (newJ < 0 || newI < 0 || newJ >= TilesRow || newI >= TilesCol)
                    continue;
                
                if (grid[newJ, newI].isMine)
                    count++;
            }
        }
        
        return count;
    }
}
