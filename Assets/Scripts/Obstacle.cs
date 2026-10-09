using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void Update()
    {
        if (PickUpManager.Instance == null)
            return;

        Collider obstacleCollider = GetComponent<Collider>();
        if (obstacleCollider == null)
            return;

        obstacleCollider.enabled = !PickUpManager.Instance.isGhost;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                collision.gameObject.GetComponent<PlayerController>().Dead();
            }
        }
    }
}
