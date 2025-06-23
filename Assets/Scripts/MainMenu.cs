using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI txtStats;

    private async void Start()
    {
        await FirebaseSaveManager.Instance.LoadData();
        int hs = FirebaseSaveManager.Instance.tempLoad.highScore;
        int op = FirebaseSaveManager.Instance.tempLoad.obsticleP;
        int pp = FirebaseSaveManager.Instance.tempLoad.pickupP;
        int bd = FirebaseSaveManager.Instance.tempLoad.bossDefeated;
        string temp = "";

        temp = temp + "High Score: ";
        temp = temp + hs;
        temp = temp + "\nObsticles Passed: ";
        temp = temp + op;
        temp = temp + "\nPickUps Used: ";
        temp = temp + pp;
        temp = temp + "\nBosses Defeated: ";
        temp = temp + bd;
        txtStats.text = temp;
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
