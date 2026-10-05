using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public static HighScore Instance { get; private set; }

    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (highScoreText == null)
            return;

        if (HighScoreManager.Instance == null)
            return;

        int score = HighScoreManager.Instance.GetHighScore();

        highScoreText.text = "High Score: " + score.ToString();
    }
}