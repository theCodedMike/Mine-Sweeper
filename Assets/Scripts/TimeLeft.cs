using UnityEngine;
using UnityEngine.UI;

public class TimeLeft : MonoBehaviour
{
    public Text timeLeft;

    private float _totalTime;
    private bool _pause;

    public void SetTotal(int total)
    {
        _totalTime = total;
    }

    // 暂停
    public void Pause() => _pause = true;
    // 继续
    public void Resume() => _pause = false;
    
    private void Update()
    {
        if (!_pause)
        {
            _totalTime -= Time.deltaTime;
            int leftTime = (int)_totalTime;
            timeLeft.text = leftTime.ToString("D3");
        }

        if (_totalTime <= 0 && !GameManager.gameOver)
        {
            _totalTime = 0;
            _pause = true;
            GameManager.gameOver = true;
            print("游戏结束...");
        }
    }
}
