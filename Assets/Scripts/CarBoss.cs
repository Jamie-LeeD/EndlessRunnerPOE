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
    // Update is called once per frame
    void Update()
    {
        CarMove();
    }

    public void CarMove()
    {
        Vector3 forwardMovement = Vector3.left * runSpeed * Time.fixedDeltaTime;

        rb.MovePosition(Vector3.Lerp(rb.position + forwardMovement, rb.position, runSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                if(PickUpManager.Instance.isGhost == false)
                {
                    collision.gameObject.GetComponent<PlayerController>().Dead();
                }   
            }

            if(collision.gameObject.GetComponent<Obstacle>() != null)
            {
                Destroy(collision.gameObject.GetComponent<Obstacle>());
            }
        } 
    }
}
