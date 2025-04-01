using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Inscribed")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int maxArmor = 0;
    public float enemyRefreshTime = 0.5f;
    [SerializeField] private bool isPlayer;
    
    [Header("Dynamic")]
    public int currentHealth;
    public int currentArmor;
    
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void ChangeHealth(int amount, bool addHealth = false, bool addArmor = false, bool bonus = false)
    {
        Debug.Log("Dealt:" + amount);
        if (addHealth)
        {
            
            if (!bonus && currentHealth < maxHealth)
            {
                // If our maxHealth is above 0, we dont want to change anything
                if(currentHealth >= maxHealth) return;
                
                if ((currentHealth + amount) <= maxHealth)
                {
                    currentHealth += amount;  
                }
                
                else if ((currentHealth + amount) > maxHealth)
                {
                    currentHealth = maxHealth;
                }
            }

            else if(bonus)
            {
                if (currentHealth + amount >= (maxHealth * 2))
                {
                    currentHealth = (maxHealth * 2);
                }
                else 
                {
                    currentHealth += amount;
                }
            }

        }
        if (addArmor)
        {
            
            if (!bonus && currentArmor < maxArmor)
            {
                // If our maxHealth is above 0, we dont want to change anything
                if(currentArmor >= maxArmor) return;
                
                if ((currentArmor + amount) <= maxArmor)
                {
                    currentArmor += amount;  
                }
                
                else if ((currentHealth + amount) > maxHealth)
                {
                    currentArmor = maxArmor;
                }
            }

            else if(bonus)
            {
                if (currentArmor + amount >= (maxArmor * 2))
                {
                    currentArmor = (maxArmor * 2);
                }
                else 
                {
                    currentArmor += amount;
                }
            }

        }
        else if(!addHealth && !addArmor)
        {
            if (currentArmor > 0)
            {
                currentArmor -= (int)(amount * .33f);
                currentHealth -= (int)(amount * .66f);
            }
            else
            {
                currentHealth -= amount;
                // Checking if we are an enemy
                if (GetComponent<EnemyAI>() != null)
                {
                    NavMeshAgent n = GetComponent<NavMeshAgent>();
                    Weapon w = GetComponentInChildren<Weapon>();
                    w.enabled = false;
                    n.speed = 0;
                    
                    Material mat = GetComponentInChildren<MeshRenderer>().material;
                    mat.SetColor("_Color", Color.red);
                    Invoke("EnemyRefresh", enemyRefreshTime);
                }
            }
            
        }
        
        if (currentHealth <= 0)
        {
            if(isPlayer) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else Destroy(gameObject);
        }

        if (currentArmor < 0)
        {
            currentArmor = 0;
        }
    }

    public void EnemyRefresh()
    {
        Weapon w = GetComponentInChildren<Weapon>();
        w.enabled = false;
        NavMeshAgent n = GetComponent<NavMeshAgent>();
        n.speed = 3.5f;
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        mat.SetColor("_Color", Color.white);
          
    }
}
