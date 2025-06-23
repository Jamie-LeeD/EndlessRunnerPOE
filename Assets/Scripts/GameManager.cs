using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IGameListener
{
    public static GameManager Instance;

    public static int points = 0;
    public int obsticlesPassed = 0;
    public int pickupsUsed = 0; 
    public int bossDefeated = 0;    

    

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
        //if (points == 10)
        //{
        //    BossManager.Instance.spawnBoss();

        //    points++;
        //}
        //if(points == 20) 
        //{
        //    BossManager.Instance.spawnBoss();

        //    points++;
        //}
        //if (points == 30) 
        //{
        //    SceneManager.LoadScene(2);
        //    points++;
        //}
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) 
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
        statsManager.compareScore(points);
        statsManager.highScore = points;
        await FirebaseSaveManager.Instance.SaveData(statsManager);
        hightScore.text = "High Score: " + points; 
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
}
