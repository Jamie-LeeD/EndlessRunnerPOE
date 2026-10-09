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

    public static DatabaseReference DBreference;
    public StatsManager tempLoad;

    const string DefaultDatabaseUrl = "https://noexit-6dc8d-default-rtdb.firebaseio.com/";

    private TaskCompletionSource<bool> ready;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        tempLoad = ReadLocal();

        ready = new TaskCompletionSource<bool>();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Could not resolve Firebase dependencies: " + task.Exception);
                ready.TrySetResult(false);
                return;
            }

            DependencyStatus status = task.Result;
            if (status == DependencyStatus.Available)
            {
                string databaseUrl = FirebaseApp.DefaultInstance.Options.DatabaseUrl != null
                    ? FirebaseApp.DefaultInstance.Options.DatabaseUrl.AbsoluteUri
                    : DefaultDatabaseUrl;
                DBreference = FirebaseDatabase.GetInstance(databaseUrl).RootReference;
                Debug.Log("Firebase Initialized at " + databaseUrl);
                ready.TrySetResult(true);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + status);
                ready.TrySetResult(false);
            }
        });
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public async Task SaveData(StatsManager stats)
    {
        if (stats == null)
            return;

        bool firebaseReady = ready != null && await ready.Task;
        if (!firebaseReady || DBreference == null)
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

        SaveLocal(stats);
        string userId = SystemInfo.deviceUniqueIdentifier;
        await DBreference.Child("players").Child(userId).SetValueAsync(data);
        Debug.Log("Player data saved to Firebase.");
    }

    public void SaveLocal(StatsManager stats)
    {
        if (stats == null)
            return;

        PlayerPrefs.SetInt("HighScore", stats.highScore);
        PlayerPrefs.SetInt("Obsticles", stats.obsticleP);
        PlayerPrefs.SetInt("PickUps", stats.pickupP);
        PlayerPrefs.SetInt("BossDefeated", stats.bossDefeated);
        PlayerPrefs.Save();
    }

    public StatsManager GetBestStats()
    {
        StatsManager local = ReadLocal();
        if (tempLoad == null || local.highScore > tempLoad.highScore)
            return local;

        return tempLoad;
    }

    private static StatsManager ReadLocal()
    {
        return new StatsManager(
            PlayerPrefs.GetInt("HighScore", 0),
            PlayerPrefs.GetInt("Obsticles", 0),
            PlayerPrefs.GetInt("PickUps", 0),
            PlayerPrefs.GetInt("BossDefeated", 0));
    }

    public async Task<bool> LoadData()
    {
        if (tempLoad == null)
            tempLoad = ReadLocal();

        bool firebaseReady = ready != null && await ready.Task;
        if (!firebaseReady || DBreference == null)
        {
            Debug.LogError("Firebase not initialized.");
            return false;
        }

        try
        {
            string userId = SystemInfo.deviceUniqueIdentifier;
            DataSnapshot snapshot = await DBreference.Child("players").Child(userId).GetValueAsync();

            if (snapshot.Exists)
            {
                int hs = ReadStat(snapshot, "HighScore");
                int op = ReadStat(snapshot, "Obsticles");
                int pp = ReadStat(snapshot, "PickUps", "Pickups");
                int bd = ReadStat(snapshot, "BossDefeated");
                tempLoad = new StatsManager(hs, op, pp, bd);
            }
            else
            {
                Debug.LogWarning("No player data found.");
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to load player data: " + ex.Message);
            return false;
        }
    }

    private static int ReadStat(DataSnapshot snapshot, string key, string alternateKey = null)
    {
        object value = snapshot.Child(key).Value;
        if (value == null && !string.IsNullOrEmpty(alternateKey))
            value = snapshot.Child(alternateKey).Value;

        if (value == null)
            return 0;
        if (value is int intValue)
            return intValue;
        if (value is long longValue)
            return (int)longValue;
        if (value is double doubleValue)
            return (int)doubleValue;

        return int.TryParse(value.ToString(), out int parsed) ? parsed : 0;
    }
}
