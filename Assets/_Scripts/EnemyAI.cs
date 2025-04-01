using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [Header("Inscribed")] 
    public bool hasMeleeAttack;
    public eWeaponType meleeWeapon = eWeaponType.none;
    public eWeaponType rangedWeapon = eWeaponType.pistol;

    public float attackTime;
    
    [Header("Dynamic")]
    private NavMeshAgent thisAgent;
    private Transform target;
    private Weapon weapon;
    
    private bool hasTarget;
    private bool isAttacking;
    
    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();
        weapon = GetComponentInChildren<Weapon>();
    }

    public void TargetDetected(Transform targetPlayer)
    {
        
        target = targetPlayer;
        hasTarget = true;
    }

    

    public void Update()
    {
        // Actually chasing the player
        if (target == null) return;
        thisAgent.destination = target.position;
        
        // If we are already attacking ignore
        if(isAttacking) return;
        float distance = Vector3.Distance (thisAgent.transform.position, target.position);
        
        if(weapon.enabled == false) return;
        if (distance <= (thisAgent.stoppingDistance + 1) && hasMeleeAttack)
        {
            weapon.ChangeWeapon(meleeWeapon);
            weapon.FireShot();
            Invoke("AttackRefresh", attackTime);
            print("melee");
            isAttacking = true;
        }
        else
        {
            weapon.ChangeWeapon(rangedWeapon);
            weapon.transform.LookAt(target);
            weapon.FireShot();
            Invoke("AttackRefresh", attackTime);
            print("firing");
            isAttacking = true;
            
        }
    }

    public void AttackRefresh()
    {
        isAttacking = false;
        print("refresh");
    }
    
    
}
