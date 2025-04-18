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
        gameOver.SetActive(false);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
        DisplayHighScore();
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            pauseMenu.SetActive(true);
            PlayerController.isPaused = true;
        }
    }

    public void ResetLevel()
    {
        points = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueLevel()
    {
        pauseMenu.SetActive(false);
        PlayerController.isPaused = false;
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
