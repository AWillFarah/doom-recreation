using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;

    public Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.transform == origin ) return;
        if (other.GetComponent<Health>() != null)
        {
            Health health = other.GetComponent<Health>();
            health.ChangeHealth(damage);
        }
        Destroy(gameObject);  
        // This ensures the object doesnt immediately disapear on creation
        
        
    }
}
