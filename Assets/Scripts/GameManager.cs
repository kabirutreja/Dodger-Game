using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    public Platformspawner platformSpawner;
    
    public static GameManager Instance { get; private set; }

    
    public float startingSpeed = 0.5f;

    
    public float speedIncrement = 0.05f;

        public float speedLimit = 2.3f;

   
    public float gravity = 25f;

    
    public float jumpStrength = 14f;

       public float lavaHeight = 0f;

    [Tooltip("Seconds to wait after death before the scene reloads.")]
    public float resetTime = 3f;

    
    public float deathFadeDuration = 0.5f;

    public ParticleSystem flamesEffect;
    public  ParticleSystem deatheffect;

        public ScoreDisplay scoreDisplay;
        public AudioSource deathSound;

   
    public float AscentSpeed { get; private set; }

   
    public int Score { get; private set; }

       public bool IsPlayerDead { get; private set; }
       public TMPro.TextMeshProUGUI highScoreText;

       private readonly List<Platform> activePlatforms = new List<Platform>();

    void Awake()
    {
       // Time.timeScale = 0f;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
         Debug.Log("GameManager Instance set on " + gameObject.name, gameObject);

        SetupGame();
    }

    
    public void SetupGame()
    {
        AscentSpeed = startingSpeed;
        Score = 0;
        IsPlayerDead = false;
        activePlatforms.Clear();

        if (scoreDisplay != null)
            scoreDisplay.SetVisible(false);   // original: score starts at opacity 0
    }

    
    public void OnPlatformSpawned()
    {
        Score++;
        AscentSpeed = Mathf.Min(AscentSpeed + speedIncrement, speedLimit);

        if (scoreDisplay != null)
        {
            scoreDisplay.SetScore(Score);
            scoreDisplay.SetVisible(true);   
        }
    }

   
    public void RegisterPlatform(Platform p)   => activePlatforms.Add(p);
    public void UnregisterPlatform(Platform p) => activePlatforms.Remove(p);

   
    public void KillPlayer(Vector3 deathPosition)
    {
        if (IsPlayerDead) return;  
         
        IsPlayerDead = true;
        deathSound.Play();
        HighScoreManager.Instance.SetHighScore(Score);
        highScoreText.text = "High Score: " + HighScoreManager.Instance.GetHighScore().ToString();
        platformSpawner.isworking = false;
        

       
        if (flamesEffect != null)
        {
            flamesEffect.transform.position = deathPosition;
            flamesEffect.Play();
            deatheffect.gameObject.SetActive(true); 
            deatheffect.transform.position = deathPosition;
            deatheffect.Play();
        }

        
        foreach (Platform p in activePlatforms)
            p.FadeOut(deathFadeDuration);

        StartCoroutine(RestartAfterDelay());
         StartCoroutine(DeathSequence());
    }
    private IEnumerator DeathSequence()
{
    
    Time.timeScale = 0f;

   
    yield return new WaitForSecondsRealtime(0.5f);

    
    Time.timeScale = 0.5f;

    
    StartCoroutine(RestartAfterDelay());
}

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(resetTime);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}