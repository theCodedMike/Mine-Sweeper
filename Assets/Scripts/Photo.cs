using UnityEngine;

public class Photo : MonoBehaviour
{
    [Header("未打开")]
    public Sprite unrevealed;
    [Header("已打开")]
    public Sprite revealed;
    [Header("标记")]
    public Sprite flag;

    [Header("地雷")]
    public Sprite mine;
    public Sprite ooh;

    [Header("笑脸")]
    public Sprite faceOk;
    public Sprite faceOoh;
    public Sprite faceDead;
    public Sprite faceWow;

    public static Photo instance;
    private void Awake()
    {
        instance = this;
    }
}
