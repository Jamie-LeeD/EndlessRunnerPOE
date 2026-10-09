using UnityEngine;

public class ManBoss : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 5f;
    public float followSpeed = 3f;

    private Rigidbody rb;
    private bool defeated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        Vector3 currentPosition = rb != null ? rb.position : transform.position;
        float targetX = player.position.x;
        float newX = Mathf.Lerp(currentPosition.x, targetX, followSpeed * Time.fixedDeltaTime);
        float newZ = currentPosition.z + moveSpeed * Time.fixedDeltaTime;
        Vector3 nextPosition = new Vector3(newX, currentPosition.y, newZ);

        if (rb != null)
            rb.MovePosition(nextPosition);
        else
            transform.position = nextPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController playerHit = collision.gameObject.GetComponent<PlayerController>();
        if (playerHit == null)
            return;

        if (PickUpManager.Instance != null && PickUpManager.Instance.isGhost)
            return;

        defeated = true;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!defeated || EventManager.Instance == null)
            return;

        EventManager.Instance.Invoke(GameEvents.BOSS_DEFEATED, this);
        EventManager.Instance.Invoke(GameEvents.SCORE_CHANGED, this);
    }
}
