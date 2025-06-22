using Unity.Services.CloudSave;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Serialization;

public class CloudSaveManager : MonoBehaviour
{
    //public PlayerProfile tempLoad;

    //public async Task SavePlayerData()
    //{
    //    var data = new Dictionary<string, object>
    //    {
    //        {

    //        }
    //   };

    //    await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    //}

    //public async Task LoadPlayerData()
    //{
    //    string playerNameValue = "DefaultPlayer";
    //    Color colourTemp = Color.white;
    //    string colourString = "#ffffff";

    //    var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "playerName", "favouriteColor" });
    //    if (data.TryGetValue("playerName", out var playerName))
    //    {
    //        playerNameValue = playerName.Value.GetAs<string>();
    //        Debug.Log($"Player Name: {playerNameValue}");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Player name not found.");
    //    }


    //    if (data.TryGetValue("favouriteColor", out var favouriteColour))
    //    {
    //        colourString = favouriteColour.Value.GetAs<string>();
    //        Debug.Log($"Favourite Color (hex): {colourString}");


    //    }

    //    tempLoad = new PlayerProfile(playerNameValue, colourString);
    //}
}
