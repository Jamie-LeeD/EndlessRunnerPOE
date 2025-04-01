using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOver;
    public GameObject pauseMenu;
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
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueLevel()
    {
        pauseMenu.SetActive(false);
    }
}
