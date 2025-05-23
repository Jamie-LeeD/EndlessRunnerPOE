using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private void Update()
    {
        if(PickUpManager.Instance.isGhost)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
        else 
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        
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
