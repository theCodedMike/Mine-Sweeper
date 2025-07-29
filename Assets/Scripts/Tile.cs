using UnityEngine;
using UnityEngine.UI;

public enum State
{
    Unrevealed,
    Revealed,
    Flag
}
public class Tile : MonoBehaviour
{
    [Header("字体")]
    public Transform fontPrefab;
    private State _state = State.Unrevealed;
    private SpriteRenderer _renderPhoto;

    public bool isMine;
    public bool mouseOver;
    public int x, y;
    private Text _mineCount;
    public int nearbyMines; //周围的地雷数

    private void Start()
    {
        _renderPhoto = GetComponent<SpriteRenderer>();
        _renderPhoto.sprite = Photo.instance.unrevealed;
        // 将字体显示在方块的正中间，且需在上方
        Transform textObj = Instantiate(fontPrefab, new Vector3(transform.position.x + 16, transform.position.y + 16, -3), Quaternion.identity);
        textObj.SetParent(transform);
        _mineCount = textObj.GetComponent<Text>();
    }

    // 每个方块都需要初始化
    public void Reset()
    {
        _state = State.Unrevealed;
        isMine = false;
        mouseOver = false;

        if (_renderPhoto != null)
            _renderPhoto.sprite = Photo.instance.unrevealed;
        if(_mineCount != null)
            _mineCount.text = string.Empty;
    }

    // 获取当前方块在二维数组中的位置
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // 设置炸弹
    public void SetMine()
    {
        isMine = true;
        
    }

    private void OnMouseEnter()
    {
        mouseOver = true;
    }

    private void OnMouseExit()
    {
        mouseOver = false;
    }

    private void OnMouseDown()
    {
        if (isMine)
            _renderPhoto.sprite = Photo.instance.mine;
        else
        {
            // 非雷瓦片显示周围地雷个数
            _mineCount.text = nearbyMines > 0 ? $"{nearbyMines}" : string.Empty;
            _renderPhoto.sprite = Photo.instance.revealed;
        }

        _state = State.Revealed;
    }
}
