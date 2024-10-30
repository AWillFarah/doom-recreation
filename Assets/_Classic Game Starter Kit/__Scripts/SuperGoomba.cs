using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperGoomba : MonoBehaviour {
    public enum eState { patrol, chase };
    
    [Header("Inscribed")]
    public float     speed = 10;
    public float     chaseSpeed     = 8;
    public float     chaseDistLimit = 10;
    public LayerMask raycastLayers; // Ground and Enemy
    [Range(0,1)]
    public float     raycastDistance = 0.4f;

    [Header( "Dynamic" )]
    public eState state = eState.patrol;
    public float currSpeed = 0;

    private Rigidbody2D r2d;
    private Collider2D  col2d;
    public  GameObject  sensedPlayer;
    
    void Start() {
        currSpeed = -speed;
        r2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D( Collider2D coll ) {
        CharacterMovement cm = coll.transform.root.GetComponent<CharacterMovement>();
        if ( cm != null ) {
            sensedPlayer = cm.gameObject;
            state = eState.chase;
        }
    }

    void FixedUpdate()
    {
        switch ( state ) {
        case eState.patrol:
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
            break;
        
        case eState.chase:
            if ( sensedPlayer == null ) {
                state = eState.patrol;
                break;
            }
            // Find where the player is and move towards them
            // if ( sensedPlayer.transform.position.x < transform.position.x ) {
            //     currSpeed = -chaseSpeed;
            // } else {
            //     currSpeed = chaseSpeed;
            // }
            currSpeed = (sensedPlayer.transform.position.x < transform.position.x) ? -chaseSpeed : chaseSpeed;
            if ( (sensedPlayer.transform.position - transform.position).magnitude > chaseDistLimit ) {
                sensedPlayer = null;
                state = eState.patrol;
            }
            break;
        }
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
