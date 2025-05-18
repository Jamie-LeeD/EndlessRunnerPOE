using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int points = 0;
    public GameObject gameOver;
    public GameObject pauseMenu;

    public TextMeshProUGUI score;
    public TextMeshProUGUI hightScore;
    private void Awake()
    {
        instance = this;
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
            Ground.spawn = false;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
