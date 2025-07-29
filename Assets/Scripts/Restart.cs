using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private void OnMouseDown()
    {
        GameManager.gameOver = false;
        SceneManager.LoadScene("Game");
    }
}
