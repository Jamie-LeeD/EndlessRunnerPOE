using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

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

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject boss;

    Vector3 offset = new Vector3 (7, 0, 8);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnBoss()
    {
        EventManager.Instance.Invoke(GameEvents.BOSS_SPAWN, this);
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
