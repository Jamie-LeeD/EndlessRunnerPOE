using System;
using UnityEngine;

public class Torch : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {

        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                PickUpManager.Instance.AddEffect(PickUpEffects.TORCH, 10f);
                Destroy(gameObject);
            }
        }
        
    }

    private void Update()
    {
        
    }
}
