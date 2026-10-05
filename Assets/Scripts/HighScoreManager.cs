using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

    private const string HighScoreKey = "HighScore";

    public int CurrentHighScore { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CurrentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public bool SetHighScore(int score)
    {
        if (score > CurrentHighScore)
        {
            CurrentHighScore = score;

            PlayerPrefs.SetInt(HighScoreKey, CurrentHighScore);
            PlayerPrefs.Save();

            // Update UI
            if (HighScore.Instance != null)
            {
                HighScore.Instance.UpdateText();
            }

            return true;
        }

        return false;
    }

    public int GetHighScore()
    {
        return CurrentHighScore;
    }

    public void ResetHighScore()
    {
        CurrentHighScore = 0;

        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();

        // Update UI
        if (HighScore.Instance != null)
        {
            HighScore.Instance.UpdateText();
        }
    }
}