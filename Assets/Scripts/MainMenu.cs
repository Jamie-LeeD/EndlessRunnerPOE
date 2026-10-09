using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI txtStats;

    private async void Start()
    {
        await ShowSavedStats();
    }

    public async void OpenStats()
    {
        if (txtStats != null)
        {
            txtStats.transform.parent.gameObject.SetActive(true);
            StatsManager known = FirebaseSaveManager.Instance != null
                ? FirebaseSaveManager.Instance.GetBestStats()
                : new StatsManager();
            txtStats.text = FormatStats(known);
        }

        gameObject.SetActive(false);
        await ShowSavedStats();
    }

    private async System.Threading.Tasks.Task ShowSavedStats()
    {
        if (txtStats == null)
            return;

        StatsManager stats = new StatsManager();
        if (FirebaseSaveManager.Instance != null)
        {
            await FirebaseSaveManager.Instance.LoadData();
            stats = FirebaseSaveManager.Instance.GetBestStats();
        }
        else
        {
            Debug.LogError("FirebaseSaveManager is missing from the main menu.");
        }

        txtStats.text = FormatStats(stats);
    }

    private static string FormatStats(StatsManager stats)
    {
        return "High Score: " + stats.highScore
            + "\nObsticles Passed: " + stats.obsticleP
            + "\nPickUps Used: " + stats.pickupP
            + "\nBosses Defeated: " + stats.bossDefeated;
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
