using TMPro;
using UnityEditor.SearchService;
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
    private static bool allowRandomSwitching = false;

    public GameObject gameOver;
    public GameObject pauseMenu;

    public TextMeshProUGUI score;
    public TextMeshProUGUI hightScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Ground.spawn = true;
        gameOver.SetActive(false);
        pauseMenu.SetActive(false);

        EventManager.Instance.AddListener(GameEvents.SCORE_CHANGED, this);
        EventManager.Instance.AddListener(GameEvents.PICK_UP_ADDED, this);
        EventManager.Instance.AddListener(GameEvents.OBSTICLE_PASSED, this);
        EventManager.Instance.AddListener(GameEvents.BOSS_SPAWN, this);
        EventManager.Instance.AddListener(GameEvents.BOSS_DEFEATED, this);

    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvents.SCORE_CHANGED, this);
        EventManager.Instance.RemoveListener(GameEvents.PICK_UP_ADDED, this);
        EventManager.Instance.RemoveListener(GameEvents.OBSTICLE_PASSED, this);
        EventManager.Instance.RemoveListener(GameEvents.BOSS_SPAWN, this);
        EventManager.Instance.RemoveListener(GameEvents.BOSS_DEFEATED, this);
    }
    private void FixedUpdate()
    {
        if(timer >= 30f)
        {
            BossManager.Instance.spawnBoss();
        }
        if(timer >= 50f)
        {
            LoadNextScene();
            timer = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResetLevel()
    {
        Time.timeScale = 1f;
        points = 0;
        SceneManager.LoadScene(1);
    }

    public void ContinueLevel()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HomeBtn()
    {
        Time.timeScale = 1f;
        points = 0;
        SceneManager.LoadScene(0);
    }

    public void DisplayScore()
    {
        score.text = "Score: " + points;
    }

    public async void DisplayHighScore()
    {
        StatsManager statsManager = FirebaseSaveManager.Instance.tempLoad;
        statsManager.compareScore(points, obsticlesPassed, pickupsUsed, bossDefeated);
        await FirebaseSaveManager.Instance.SaveData(statsManager);
        hightScore.text = "High Score: " + statsManager.highScore; 
    }

    public void OnEvent(GameEvents eventType, Component sender, object param = null)
    {
        //throw new System.NotImplementedException();
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
            // First time: go from Scene1 to Scene2
            SceneManager.LoadScene(2);
            allowRandomSwitching = true;
        }
        else
        {
            int index = SceneManager.GetActiveScene().buildIndex;
            // From now on: randomly switch between Scene1 and Scene2
            int[] scenes = { 1, 2 };
            int randomScene = scenes[Random.Range(0, scenes.Length)];
            if (randomScene != index)
            {
                SceneManager.LoadScene(randomScene);
            }

        }
    }
}
