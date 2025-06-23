using UnityEngine;

public class Boss2Manager : MonoBehaviour
{
    public static Boss2Manager Instance;

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
        //Vector3 vecPlayer = player.transform.position;
        //Vector3 targetPosition = vecPlayer + offset;

        //Instantiate(boss, targetPosition, Quaternion.identity, transform);
    }
}
