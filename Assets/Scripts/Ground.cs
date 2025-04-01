using UnityEngine;

public class Ground : MonoBehaviour
{
    private GroundSpawner groundspawner;

    public GameObject[] obsticals;
    public Transform[] lanes;

    private void Awake()
    {
        groundspawner = GameObject.FindFirstObjectByType<GroundSpawner>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnObstical();
    }

    private void OnTriggerExit(Collider other)
    {
        groundspawner.spawnGround();
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnObstical()
    {
        int selectLane = Random.Range(0, lanes.Length);
        int selectObsticle = Random.Range(0, obsticals.Length);

        Instantiate(obsticals[selectObsticle], lanes[selectLane].transform.position, Quaternion.identity, transform);
    } 
}
