using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int points = 0;
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
    }

    private void FixedUpdate()
    {
        if (points == 10)
        {
            BossManager.Instance.spawnBoss();

            points++;
        }
        if(points == 20) 
        {
            BossManager.Instance.spawnBoss();

            points++;
        }
        if (points == 30) 
        {
            SceneManager.LoadScene(2);
            points++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        DisplayScore();
        DisplayHighScore();
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

    public void DisplayScore()
    {
        score.text = "Score: " + points;
    }

    public void DisplayHighScore()
    {
        hightScore.text = "High Score: " + points; 
    }
}
