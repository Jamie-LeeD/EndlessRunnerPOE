using UnityEngine;

public class Ground : MonoBehaviour
{
    private GroundSpawner groundspawner;


    public GameObject[] pickups;
    public GameObject[] obstacals;
    public Transform[] lanes;

    private int occupiedLane;

    public static bool spawn = true;

    private bool scored;

    private void Awake()
    {
        groundspawner = GameObject.FindFirstObjectByType<GroundSpawner>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(spawn)
        {
            SpawnObstacal();
            SpawnPickUp();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (scored || other.GetComponentInParent<PlayerController>() == null)
            return;

        scored = true;
        if (groundspawner != null)
            groundspawner.spawnGround();
        else
            Debug.LogError("GroundSpawner is missing, so the next road tile was not spawned.");

        if (EventManager.Instance != null)
            EventManager.Instance.Invoke(GameEvents.SCORE_CHANGED, this);

        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObstacal()
    {
        int selectLane = Random.Range(0, lanes.Length);
        int selectObstacle = Random.Range(0, obstacals.Length);
        occupiedLane = selectLane;
        Instantiate(obstacals[selectObstacle], lanes[selectLane].transform.position, Quaternion.identity, transform);
    } 

    public void SpawnPickUp()
    {
        int shouldPickup = Random.Range(0, 3);
        if(shouldPickup == 1) 
        {
            int selectLane = Random.Range(0, lanes.Length);
            int selectPickup = Random.Range(0, pickups.Length);
            if (occupiedLane != selectLane)
            {
                Instantiate(pickups[selectPickup], lanes[selectLane].transform.position, Quaternion.identity, transform);
            }
        }
        
        
    }
}
