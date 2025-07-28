using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject tilePrefab;

    private const int TileSize = 32;
    private const int TilesRow = 10;
    private const int TilesCol = 10;

    private void Start()
    {
        for (int j = 0; j < TilesRow; j++)
        {
            for (int i = 0; i < TilesCol; i++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(i * TileSize, j * TileSize, 0), Quaternion.identity);
                tileObj.transform.SetParent(transform);
            }
        }
    }
}
