using UnityEngine;

public class ManBoss : MonoBehaviour
{
    public Transform player;            // Reference to the player
    public float moveSpeed = 5f;        // How fast the boss moves forward
    public float followSpeed = 3f;      // How fast the boss tracks player's X position

    void Update()
    {
        if (player == null) return;

        // Current position
        Vector3 currentPosition = transform.position;

        // Create a target position that follows the player's X but keeps boss's own Y
        float targetX = player.position.x;
        float newX = Mathf.Lerp(currentPosition.x, targetX, followSpeed * Time.deltaTime);

        // Move forward constantly (e.g., Z axis)
        float newZ = currentPosition.z + moveSpeed * Time.deltaTime;

        // Apply the new position
        transform.position = new Vector3(newX, currentPosition.y, newZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Obstacle>() == null)
            {
                Destroy(gameObject);
            }
    }

    private void OnDestroy()
    {
        EventManager.Instance.Invoke(GameEvents.BOSS_DEFEATED, this);
        EventManager.Instance.Invoke(GameEvents.SCORE_CHANGED, this);
    }
}
