using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    private int damage;
    private Health health;
    bool isTouched = false;
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Health>() != null)
        {
            health = other.GetComponent<Health>();
            damage = (int)Math.Round(health.currentHealth * 0.05f);
            if (damage < 1)
            {
                damage = 1;
            }
            
            isTouched = true;
        }
        
        
        
    }

    void OnTriggerExit(Collider other)
    {
        isTouched = false;
    }

    void FixedUpdate()
    {
        if(isTouched) health.ChangeHealth(damage);
        
    }
    
    void Refresh()
    {
      isTouched = false;   
    }
}
