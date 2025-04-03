using System;
using UnityEngine;

public class Torch : MonoBehaviour
{
    
    [SerializeField] private float effectTimer = 10f;
    
    [SerializeField] private float fogDensity = 0.10f;
    [SerializeField] private float fogReduced = 0.05f;

    private float timer = 0;
    private bool inAffect = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.GetComponent<PlayerController>() != null)
            {
                RenderSettings.fogDensity = fogReduced;
                inAffect = true;
                timer = 0;

                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
        
    }

    private void Update()
    {
        if (inAffect)
        {
            if(timer <= effectTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                inAffect = false;
                RenderSettings.fogDensity = fogDensity;
                Destroy(gameObject);
            }
        }
        
    }
}
