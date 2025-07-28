using UnityEngine;

public enum State
{
    Unrevealed,
    Revealed,
    Flag
}
public class Tile : MonoBehaviour
{
    private State _state = State.Unrevealed;
    private SpriteRenderer _renderPhoto;

    public bool isMine;
    public bool mouseOver;
    public int x, y;

    private void Start()
    {
        _renderPhoto = GetComponent<SpriteRenderer>();
        _renderPhoto.sprite = Photo.instance.unrevealed;
    }

    // 每个方块都需要初始化
    public void Reset()
    {
        _state = State.Unrevealed;
        isMine = false;
        mouseOver = false;

        if (_renderPhoto != null)
            _renderPhoto.sprite = Photo.instance.unrevealed;
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
        print($"Mine: {x}, {y}");
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
        _renderPhoto.sprite = isMine ? Photo.instance.mine : Photo.instance.revealed;
    }
}
