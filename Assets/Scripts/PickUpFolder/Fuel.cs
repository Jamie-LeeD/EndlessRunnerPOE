using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private float effectTimer = 10f;

    private float timer = 0;
    private bool inAffect = false;

    [SerializeField]
    Animator animator;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                PlayerController.runSpeed = 15f;
                
                inAffect = true;
                timer = 0;

                animator = collision.gameObject.GetComponent<PlayerController>().GetAnimator();
                animator.SetBool("IsSprint", true);

                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (inAffect)
        {
            if (timer <= effectTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                inAffect = false;
                PlayerController.runSpeed = 10f;
                animator.SetBool("IsSprint", false);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        animator.SetBool("IsSprint", false);
    }
}
