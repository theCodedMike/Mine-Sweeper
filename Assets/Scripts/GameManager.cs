using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;
    [Header("地雷个数")]
    public int minesCount;
    [Header("计时(单位为秒)")]
    public int playTime;

    private const int TileSize = 32;
    public const int TilesRow = 10;
    public const int TilesCol = 10;
    public static bool gameOver;

    public static Tile[,] grid = new Tile[TilesRow, TilesCol];
    public static List<Tile> mines = new();


    private MinesLeft _minesLeft;
    private TimeLeft _timeLeft;

    private void Start()
    {
        _minesLeft = FindFirstObjectByType<MinesLeft>();
        _timeLeft = FindFirstObjectByType<TimeLeft>();
        
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
        
        InitData();
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
            mineTile.MarkAsMine();
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


    // 初始化部分数据
    private void InitData()
    {
        _minesLeft.SetTotal(minesCount);
        _timeLeft.SetTotal(playTime);
    }

    private void Update()
    {
        if(!gameOver)
            CheckGameOver();
    }

    private void CheckGameOver()
    {
        int revealedCount = 0;
        for (int j = 0; j < TilesRow; j++)
        {
            for (int i = 0; i < TilesCol; i++)
            {
                if (grid[j, i].state == State.Revealed)
                    revealedCount++;
            }
        }

        if (revealedCount == (TilesRow * TilesCol - minesCount))
        {
            gameOver = true;
            print("游戏结束...");
        }
    }
}
