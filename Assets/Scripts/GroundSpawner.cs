using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject ground;

    Vector3 nextSpawn;

    public void spawnGround()
    {
        GameObject tempGround = Instantiate(ground, nextSpawn, Quaternion.identity);
        nextSpawn = tempGround.transform.GetChild(1).transform.position;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            spawnGround();
        }
    }
}
