using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirebaseSaveManager : MonoBehaviour
{
    public static FirebaseSaveManager Instance;

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

    public static DatabaseReference DBreference;
    public StatsManager tempLoad;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                DBreference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + status);
            }
        });
    }

    public async Task SaveData(StatsManager stats)
    {
        if (DBreference == null)
        {
            Debug.LogError("Firebase not initialized.");
            return;
        }

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "HighScore", stats.highScore },
            { "Obsticles", stats.obsticleP },
            { "PickUps", stats.pickupP },
            { "BossDefeated", stats.bossDefeated }
        };

        string userId = SystemInfo.deviceUniqueIdentifier;

        await DBreference.Child("players").Child(userId).SetValueAsync(data);
        Debug.Log("Player data saved to Firebase.");
    }

    public async Task LoadData()
    {
        if (DBreference == null)
        {
            Debug.LogError("Firebase not initialized.");
            return;
        }

        string userId = SystemInfo.deviceUniqueIdentifier;

        DataSnapshot snapshot = await DBreference.Child("players").Child(userId).GetValueAsync();

        if (snapshot.Exists)
        {
            string sHS = snapshot.Child("HighScore")?.Value?.ToString() ?? "0";
            string sOP = snapshot.Child("Obsticles")?.Value?.ToString() ?? "0";
            string sPP = snapshot.Child("Pickups")?.Value?.ToString() ?? "0";
            string sBD = snapshot.Child("BossDefeated")?.Value?.ToString() ?? "0";

            int hs = int.Parse(sHS);
            int op = int.Parse(sOP);
            int pp = int.Parse(sPP);
            int bd = int.Parse(sBD);


            tempLoad = new StatsManager(hs, op, pp, bd);
            
        }
        else
        {
            Debug.LogWarning("No player data found.");
        }
    }
}
