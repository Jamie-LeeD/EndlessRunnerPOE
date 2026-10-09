using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameListener
{
    public static GameManager Instance;

    public int points = 0;
    public int obsticlesPassed = 0;
    public int pickupsUsed = 0;
    public int bossDefeated = 0;

    private float timer = 0f;
    private bool bossSpawned = false;
    private static bool allowRandomSwitching = false;

    public GameObject gameOver;
    public GameObject pauseMenu;

    public TextMeshProUGUI score;
    public TextMeshProUGUI hightScore;

    private void Awake()
    {
        // The previous scene is still alive while this scene's Awake runs.
        if (Instance != null && Instance != this)
        {
            GameManager previous = Instance;
            Instance = this;
            if (previous.gameObject != gameObject)
                Destroy(previous.gameObject);
        }
        else
        {
            Instance = this;
        }

        Ground.spawn = true;
    }

    void Start()
    {
        Ground.spawn = true;
        if (gameOver != null)
            gameOver.SetActive(false);
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (EventManager.Instance == null)
        {
            Debug.LogError("EventManager is missing. Score and stats events will not be recorded.");
        }
        else
        {
            EventManager.Instance.AddListener(GameEvents.SCORE_CHANGED, this);
            EventManager.Instance.AddListener(GameEvents.PICK_UP_ADDED, this);
            EventManager.Instance.AddListener(GameEvents.OBSTICLE_PASSED, this);
            EventManager.Instance.AddListener(GameEvents.BOSS_SPAWN, this);
            EventManager.Instance.AddListener(GameEvents.BOSS_DEFEATED, this);
        }

        DisplayScore();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        if (EventManager.Instance == null)
            return;

        EventManager.Instance.RemoveListener(GameEvents.SCORE_CHANGED, this);
        EventManager.Instance.RemoveListener(GameEvents.PICK_UP_ADDED, this);
        EventManager.Instance.RemoveListener(GameEvents.OBSTICLE_PASSED, this);
        EventManager.Instance.RemoveListener(GameEvents.BOSS_SPAWN, this);
        EventManager.Instance.RemoveListener(GameEvents.BOSS_DEFEATED, this);
    }

    private void FixedUpdate()
    {
        if (!bossSpawned && timer >= 30f)
        {
            bossSpawned = true;
            if (BossManager.Instance != null)
                BossManager.Instance.spawnBoss();
            else
                Debug.LogError("BossManager is missing, so the boss was not spawned.");
        }

        if (timer >= 50f)
        {
            timer = 0f;
            LoadNextScene();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOver != null && gameOver.activeSelf)
                return;

            if (pauseMenu != null)
                pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        points = 0;
        allowRandomSwitching = false;
        SceneManager.LoadScene(1);
    }

    public void ContinueLevel()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HomeBtn()
    {
        Time.timeScale = 1f;
        points = 0;
        allowRandomSwitching = false;
        SceneManager.LoadScene(0);
    }

    public void DisplayScore()
    {
        if (score != null)
            score.text = "Score: " + points;
    }

    public async void DisplayHighScore()
    {
        if (FirebaseSaveManager.Instance == null)
        {
            Debug.LogError("FirebaseSaveManager is missing, so the high score was not saved.");
            if (hightScore != null)
                hightScore.text = "High Score: " + points;
            return;
        }

        try
        {
            bool loaded = await FirebaseSaveManager.Instance.LoadData();
            StatsManager statsManager = FirebaseSaveManager.Instance.GetBestStats();
            FirebaseSaveManager.Instance.tempLoad = statsManager;
            statsManager.compareScore(points, obsticlesPassed, pickupsUsed, bossDefeated);
            FirebaseSaveManager.Instance.SaveLocal(statsManager);

            if (loaded)
                await FirebaseSaveManager.Instance.SaveData(statsManager);
            else
                Debug.LogError("High score was not saved because the existing record could not be loaded.");

            if (hightScore != null)
                hightScore.text = "High Score: " + statsManager.highScore;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update the high score: " + ex.Message);
            if (hightScore != null)
                hightScore.text = "High Score: " + points;
        }
    }

    public void OnEvent(GameEvents eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case GameEvents.SCORE_CHANGED:
                Debug.Log("score changed");
                points++;
                DisplayScore();
                break;
            case GameEvents.PICK_UP_ADDED:
                Debug.Log("Pickup added");
                pickupsUsed++;
                break;
            case GameEvents.OBSTICLE_PASSED:
                Debug.Log("obi added");
                obsticlesPassed++;
                break;
            case GameEvents.BOSS_SPAWN:
                Debug.Log("Boss spawned");
                break;
            case GameEvents.BOSS_DEFEATED:
                Debug.Log("Boss defeated");
                bossDefeated++;
                break;
        }
    }

    public void LoadNextScene()
    {
        if (!allowRandomSwitching)
        {
            SceneManager.LoadScene(2);
            allowRandomSwitching = true;
        }
        else
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            int[] scenes = { 1, 2 };
            int randomScene = scenes[UnityEngine.Random.Range(0, scenes.Length)];
            if (randomScene != index)
                SceneManager.LoadScene(randomScene);
        }
    }
}
