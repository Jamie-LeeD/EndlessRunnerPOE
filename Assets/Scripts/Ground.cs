using UnityEngine;

public class Ground : MonoBehaviour
{
    private GroundSpawner groundspawner;


    public GameObject[] pickups;
    public GameObject[] obstacals;
    public Transform[] lanes;

    private int occupiedLane;

    [SerializeField]
    public static bool spawn;

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
        groundspawner.spawnGround();
        GameManager.points++;
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
        int selectLane = Random.Range(0, lanes.Length);
        int selectPickup = Random.Range(0, pickups.Length);
        if(occupiedLane != selectLane) 
        {
            Instantiate(pickups[selectPickup], lanes[selectLane].transform.position, Quaternion.identity, transform);
        }
        
    }
}
