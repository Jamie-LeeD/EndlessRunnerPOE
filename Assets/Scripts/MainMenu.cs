using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI hightScore;

    private async void Start()
    {
        await CloudSaveManager.Instance.LoadHighScore();
        string temp = "High Score: ";
        temp = temp + CloudSaveManager.Instance.tempScore;
        hightScore.text = temp;
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
