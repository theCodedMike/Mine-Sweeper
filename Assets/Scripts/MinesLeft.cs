using UnityEngine;
using UnityEngine.UI;

public class MinesLeft : MonoBehaviour
{
    public Text mineLeft;

    private int _total;

    public void SetTotal(int total)
    {
        _total = total;
        Display();
    }

    private void Display()
    {
        mineLeft.text = _total.ToString("D2");
    }
    
    public void AddMine()
    {
        _total++;
        Display();
    }
    
    public void RemoveMine()
    {
        _total--;
        Display();
    }
}
