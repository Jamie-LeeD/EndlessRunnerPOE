using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] public float runSpeed
    {  get; set; }
    [SerializeField] private float JumpForce = 350;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float fallMultiplier = 2.5f; // Multiplier to make falling faster
    [SerializeField] private float lowJumpMultiplier = 2f; // Multiplier for short jumps

    private int currentPosIndex = 1;
    private float[] positions = { -3f, 0f, 3f };

    private bool jumpActivated = false;

    [SerializeField]
     Animator animator;
    [SerializeField]
    AudioManager audioManager;
    private void Start()
    {
        runSpeed = 10f;
        audioManager.PlaySteptsSFX();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Movement();

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            animator.SetBool("IsJump", false);
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            animator.SetBool("IsJump", false);
        }
    }

    // Update is called once per frame
    void Update()
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
            animator.SetBool("IsJump", true);
            audioManager.StartJump();
            Jump();
        }

        if(jumpActivated == true)
        {
            if(isground == true) 
            {
                //Debug.Log("JumpACt happens");
                audioManager.EndJump();
                jumpActivated = false;
            }
        }
        
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // Reset vertical velocity before jumping
        rb.AddForce(Vector3.up * JumpForce);
        jumpActivated = true;
    }

    public void Dead()
    {
        Time.timeScale = 0f;
        GameManager.Instance.gameOver.SetActive(true);
    }

    public void Movement()
    {
        Vector3 forwardMovement = Vector3.forward * runSpeed * Time.fixedDeltaTime;
        Vector3 targetPosition = new Vector3(positions[currentPosIndex], rb.position.y, rb.position.z);

        rb.MovePosition(Vector3.Lerp(rb.position + forwardMovement, targetPosition + forwardMovement, runSpeed * Time.fixedDeltaTime));
    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
