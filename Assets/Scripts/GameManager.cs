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
}
