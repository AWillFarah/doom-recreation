using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    public int currentHealth;
   
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ChangeHealth(int amount)
    {
        Debug.Log("Dealt:" + amount);
        
        // If we want to heal the player just add a negative amount 
        currentHealth -= amount;
        
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
