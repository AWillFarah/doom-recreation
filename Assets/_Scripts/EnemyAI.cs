using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent thisAgent;
    private Transform target;
    private bool hasTarget;
    void Start()
    {
        thisAgent = GetComponent<NavMeshAgent>();
    }

    public void PlayerDetected(Transform targetPlayer)
    {
        target = targetPlayer;
        hasTarget = true;
    }

    

    public void Update()
    {
        if (hasTarget)
        {
            thisAgent.destination = target.position;
            transform.LookAt(target);
            transform.rotation = new Quaternion(0, transform.rotation.eulerAngles.y, 0, 0);
        }
    }
}
