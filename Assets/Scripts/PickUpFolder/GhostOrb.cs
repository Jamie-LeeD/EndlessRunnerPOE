using UnityEngine;

public class GhostOrb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {

                PickUpManager.Instance.AddEffect(PickUpEffects.GHOST, 5f);
                Destroy(gameObject);
            }
        }
    }
}
