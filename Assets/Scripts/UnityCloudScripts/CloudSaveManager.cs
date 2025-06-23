using Unity.Services.CloudSave;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Serialization;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class CloudSaveManager : MonoBehaviour
{
    public static CloudSaveManager Instance;

    public int tempScore;

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

    //private async void Start()
    //{
    //    await UnityServices.InitializeAsync();

    //    if (!AuthenticationService.Instance.IsSignedIn)
    //    {
    //        await AuthenticationService.Instance.SignInAnonymouslyAsync();
            
    //    }
    //}
    public async void SaveHighScore(int highScore)
    {

        var data = new Dictionary<string, object>
            { { "highScore", highScore } };

        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

    public async Task LoadHighScore()
    {

        int highScoreValue = 0;

        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "highScore" });
        if (data.TryGetValue("highScore", out var highScore))
        {
            highScoreValue = highScore.Value.GetAs<int>();
        }
        else
        {
            Debug.LogWarning("No score found");
        }



        tempScore = highScoreValue;
    }
}