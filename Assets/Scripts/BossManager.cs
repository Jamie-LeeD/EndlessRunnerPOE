using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            BossManager previous = Instance;
            Instance = this;
            if (previous.gameObject != gameObject)
                Destroy(previous.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject boss;

    Vector3 offset = new Vector3(7, 0, 8);

    public void spawnBoss()
    {
        if (EventManager.Instance != null)
            EventManager.Instance.Invoke(GameEvents.BOSS_SPAWN, this);

        if (player == null || boss == null)
        {
            Debug.LogError("BossManager is missing a player or boss reference.");
            return;
        }

        int index = SceneManager.GetActiveScene().buildIndex;
        if (index == 1)
        {
            Vector3 vecPlayer = player.transform.position;
            Vector3 targetPosition = vecPlayer + offset;
            Instantiate(boss, targetPosition, Quaternion.identity, transform);
        }
        else
        {
            offset = new Vector3(0, 0, -2);
            Vector3 vecPlayer = player.transform.position;
            Vector3 targetPosition = vecPlayer + offset;
            boss.transform.position = targetPosition;
            boss.SetActive(true);
        }
    }
}
