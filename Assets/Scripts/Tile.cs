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
    public State state = State.Unrevealed;
    private SpriteRenderer _renderPhoto;

    public bool isMine;
    public bool mouseOver;
    public int x, y;
    private Text _mineCount;
    public int nearbyMines; //周围的地雷数
    private MinesLeft _minesLeft;
    private TimeLeft _timeLeft;

    private void Start()
    {
        _renderPhoto = GetComponent<SpriteRenderer>();
        _renderPhoto.sprite = Photo.instance.unrevealed;
        // 将字体显示在方块的正中间，且需在上方
        Transform textObj = Instantiate(fontPrefab,
            new Vector3(transform.position.x + 16, transform.position.y + 16, -3), Quaternion.identity);
        textObj.SetParent(transform);
        _mineCount = textObj.GetComponent<Text>();
        _minesLeft = FindFirstObjectByType<MinesLeft>();
        _timeLeft = FindFirstObjectByType<TimeLeft>();
    }

    // 每个方块都需要初始化
    public void Reset()
    {
        state = State.Unrevealed;
        isMine = false;
        mouseOver = false;
        nearbyMines = 0;

        if (_renderPhoto != null)
            _renderPhoto.sprite = Photo.instance.unrevealed;
        if (_mineCount != null)
            _mineCount.text = string.Empty;
    }

    // 获取当前方块在二维数组中的位置
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // 设置炸弹
    public void MarkAsMine()
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

    // 鼠标左键单击
    private void OnMouseDown()
    {
        if (GameManager.gameOver)
            return;
        
        Reveal();
    }

    // 揭示瓦片
    public void Reveal()
    {
        if (state == State.Revealed || state == State.Flag)
            return;
        state = State.Revealed;

        if (isMine)
        {
            GameOver();
            return;
        }

        _renderPhoto.sprite = Photo.instance.revealed;

        if (nearbyMines > 0)
            _mineCount.text = $"{nearbyMines}";
        else if (nearbyMines == 0)
        {
            if(y + 1 < GameManager.TilesRow)
                GameManager.grid[y + 1, x].Reveal();
            if(y - 1 >= 0)
                GameManager.grid[y - 1, x].Reveal();
            if(x + 1 < GameManager.TilesCol)
                GameManager.grid[y, x + 1].Reveal();
            if(x - 1 >= 0)
                GameManager.grid[y, x - 1].Reveal();
        }
    }
    
    // 游戏失败
    private void GameOver()
    {
        GameManager.gameOver = true;
        // 暂停计时
        _timeLeft.Pause();
        // 把所有未揭示的炸弹全部揭示
        foreach (Tile mine in GameManager.mines)
        {
            mine.state = State.Revealed;
            mine._renderPhoto.sprite = Photo.instance.mine;
        }
    }

    private void Update()
    {
        if (GameManager.gameOver)
            return;
        
        if (mouseOver && Input.GetMouseButtonDown(1))
            MakeFlag();
    }


    // 鼠标右键单击标记小旗子
    private void MakeFlag()
    {
        if (state == State.Unrevealed)
        {
            state = State.Flag;
            _renderPhoto.sprite = Photo.instance.flag;
            _minesLeft.RemoveMine();
        } else if (state == State.Flag)
        {
            state = State.Unrevealed;
            _renderPhoto.sprite = Photo.instance.unrevealed;
            _minesLeft.AddMine();
        }
    }
}