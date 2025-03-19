using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private EnemyAI parent;
    void Start()
    {
        parent = GetComponentInParent<EnemyAI>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // We found the player!
            print("player found");
            parent.PlayerDetected(other.transform);
        }
    }
}
