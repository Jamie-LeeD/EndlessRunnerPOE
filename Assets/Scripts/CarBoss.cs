using TMPro;
using UnityEngine;

public class CarBoss : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField]
    int runSpeed = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        CarMove();
    }

    public void CarMove()
    {
        if (rb == null)
            return;

        Vector3 forwardMovement = Vector3.left * runSpeed * Time.fixedDeltaTime;
        float blend = Mathf.Clamp01(runSpeed * Time.fixedDeltaTime);
        rb.MovePosition(Vector3.Lerp(rb.position + forwardMovement, rb.position, blend));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null)
            return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null && (PickUpManager.Instance == null || !PickUpManager.Instance.isGhost))
            player.Dead();

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
            Destroy(obstacle.gameObject);
    }
}
