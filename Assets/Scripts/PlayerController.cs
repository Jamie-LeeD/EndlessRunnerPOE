using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private Rigidbody rb;
    public static bool isPaused = false;

    public static float runSpeed = 10f;
    [SerializeField] private float JumpForce = 350;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float fallMultiplier = 2.5f; // Multiplier to make falling faster
    [SerializeField] private float lowJumpMultiplier = 2f; // Multiplier for short jumps

    private int currentPosIndex = 1;
    private float[] positions = { -3f, 0f, 3f };

    private void Start()
    {
        isPaused = false;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isAlive && !isPaused)
        {
            Movement();

            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive && !isPaused) 
        {
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentPosIndex > 0)
            {
                currentPosIndex--; // Move left
            }
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && currentPosIndex < positions.Length - 1)
            {
                currentPosIndex++; // Move right
            }

            float playerHeight = GetComponent<Collider>().bounds.size.y;
            bool isground = Physics.Raycast(transform.position, Vector3.down, (playerHeight / 2) + 0.1f, GroundMask);

            if (Input.GetKeyDown(KeyCode.Space) && isground == true)
            {
                Jump();
            }
        }
        
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity before jumping
        rb.AddForce(Vector3.up * JumpForce);
    }

    public void Dead()
    {
        isAlive = false;
        GameManager.instance.gameOver.SetActive(true);
    }

    public void Movement()
    {
        Vector3 forwardMovement = Vector3.forward * runSpeed * Time.fixedDeltaTime;
        Vector3 targetPosition = new Vector3(positions[currentPosIndex], rb.position.y, rb.position.z);

        rb.MovePosition(Vector3.Lerp(rb.position + forwardMovement, targetPosition + forwardMovement, runSpeed * Time.fixedDeltaTime));
    }
}
