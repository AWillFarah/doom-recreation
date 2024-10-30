using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour {
    
    [Header("Inscribed")]
    public float     speed = 10;
    public LayerMask raycastLayers; // Ground and Enemy
    [Range(0,1)]
    public float     raycastDistance = 0.4f;

    [Header( "Dynamic" )]
    public float currSpeed = 0;

    private Rigidbody2D r2d;
    private Collider2D  col2d;
    
    void Start() {
        currSpeed = -speed;
        r2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        // Check to see if our raycasts hit any raycastLayers
        RaycastHit2D leftHit, rightHit;
        Vector2 pos2D = transform.position;
        col2d.enabled = false;
        rightHit = Physics2D.Raycast( pos2D, Vector2.right, raycastDistance, raycastLayers );
        // If so, turn around
        if ( rightHit.collider != null ) { // we hit something!
            currSpeed = -speed;
        }

        leftHit = Physics2D.Raycast( pos2D, Vector2.left, raycastDistance, raycastLayers );
        // If so, turn around
        if ( leftHit.collider != null ) { // we hit something!
            currSpeed = speed;
        }
        col2d.enabled = true;
            
        Vector2 vel = r2d.velocity;
        vel.x = currSpeed;
        r2d.velocity = vel;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * raycastDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * raycastDistance);
    }
}
