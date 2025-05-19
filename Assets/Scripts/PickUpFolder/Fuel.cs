using UnityEngine;

public class Fuel : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                PickUpManager.Instance.AddEffect(PickUpEffects.FUEL, 10f);
                Destroy(gameObject);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
