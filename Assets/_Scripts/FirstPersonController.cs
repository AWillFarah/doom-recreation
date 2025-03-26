#define Use_Xnput

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using NaughtyAttributes;


[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour {
    [Header( "Inscribed" )]
    public float speed = 10;
    public float acceleration = 13;
    public float deceleration = 13;
  
    
    [OnValueChanged("SetJumpVars")]
    public float jumpHeight = 5;
    [OnValueChanged("SetJumpVars")]
    public float jumpDist   = 10;
    [Range(0.1f,0.9f)]
    [OnValueChanged("SetJumpVars")]
    public float jumpApex = 0.5f;
    [Tooltip("Note that variable jump height only works if jumpApex > 0.5." +
             "    Otherwise, there is no difference between rising and falling gravity")]
    public bool useVariableHeightJump = true;
    
    public Transform camTrans;
    public float     yawMult     = 30;
    public float     pitchMult   = 20;
    public bool      invertPitch = true;
    public Vector2   pitchLimits = new Vector2( -60, 60 );
    
    [SerializeField] Transform rayOrigin;
    [SerializeField] private float rayDistance = 0.1f;
    
    [Header("Dynamic")]
    public float jumpVel;
    public float jumpGrav;
    public float jumpGravDown;
    public bool  jumpRising = false;
    
    private Rigidbody rigid;
    
    private Vector3 moveDirection;
    private Vector3 velocity;

    void Start() {
        rigid = GetComponent<Rigidbody>();
        
        // Hide and Lock the Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        // We're not using Unity gravity because we're modifying it ourselves - JGB 2025-03-09
        // rigid.useGravity = false;
    }

    // Update is called once per frame
    void Update() {
        // NOTE: This is only really necessary when you're tuning the jump values.
        //   You can remove this call once you're happy with the jump values. - JGB 2025-03-09
#if UNITY_EDITOR // Only run this in the Editor, in case you don't remove it.
        SetJumpVars();
#endif
        
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
#if Use_Xnput
        float h = Xnput.GetAxis( Xnput.eAxis.horizontal );
        float v = Xnput.GetAxis( Xnput.eAxis.vertical );
        float mX = Xnput.GetAxisRaw( Xnput.eAxis.rightStickH );
        float mY = Xnput.GetAxisRaw( Xnput.eAxis.rightStickV );
        bool jumpNow = Xnput.GetButtonDown( Xnput.eButton.a );
        bool jumpHeld = Xnput.GetButton( Xnput.eButton.a );
#else
        float h = Input.GetAxis( "Horizontal" );
        float v = Input.GetAxis( "Vertical" );
        float mX = Input.GetAxisRaw( "Mouse X" );
        float mY = Input.GetAxisRaw( "Mouse Y" );
        bool jumpNow = Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyDown( KeyCode.X );
        bool jumpHeld = Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.X );
#endif
        // Player rotation (Yaw)
        Vector3 rot = transform.eulerAngles;
        rot.y += mX * yawMult * Time.deltaTime;
        transform.eulerAngles = rot;
        
        
        Vector3 rotCam = camTrans.eulerAngles;
        float rotX = rotCam.x + ( mY * pitchMult * Time.deltaTime * (invertPitch ? -1 : 1) );
        if ( rotX > 180 ) rotX = rotX - 360;
        rotX = Mathf.Clamp( rotX, pitchLimits.x, pitchLimits.y );
        rotCam = new Vector3( rotX, 0, 0 );
        camTrans.localEulerAngles = rotCam; 
        
    }

    void FixedUpdate()
    { 
        // NOTE: This is only really necessary when you're tuning the jump values.
        //   You can remove this call once you're happy with the jump values. - JGB 2025-03-09
#if UNITY_EDITOR // Only run this in the Editor, in case you don't remove it.
        SetJumpVars();
#endif
        
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
#if Use_Xnput
        float h = Xnput.GetAxis( Xnput.eAxis.horizontal );
        float v = Xnput.GetAxis( Xnput.eAxis.vertical );
        float mX = Xnput.GetAxisRaw( Xnput.eAxis.rightStickH );
        float mY = Xnput.GetAxisRaw( Xnput.eAxis.rightStickV );
        bool jumpNow = Xnput.GetButtonDown( Xnput.eButton.a );
        bool jumpHeld = Xnput.GetButton( Xnput.eButton.a );
#else
        float h = Input.GetAxis( "Horizontal" );
        float v = Input.GetAxis( "Vertical" );
        float mX = Input.GetAxisRaw( "Mouse X" );
        float mY = Input.GetAxisRaw( "Mouse Y" );
        bool jumpNow = Input.GetKeyDown( KeyCode.Space ) || Input.GetKeyDown( KeyCode.X );
        bool jumpHeld = Input.GetKey( KeyCode.Space ) || Input.GetKey( KeyCode.X );
#endif

        // XY movement
        //Vector3 vel = (transform.forward * v + transform.right * h);
        moveDirection = (transform.forward * v + transform.right * h).normalized;
        // Acceleration
        if (moveDirection.magnitude > 0)
        {
            velocity = Vector3.Lerp(velocity, moveDirection * speed, Time.deltaTime * acceleration);
        }
        // Deceleration
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime * deceleration);
        }
        
        velocity.y = rigid.velocity.y;
        
        if(Physics.Raycast(rayOrigin.position, Vector3.down, out RaycastHit hit, rayDistance))
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y + hit.distance + .1f, transform.position.z);
        } 
        
        // Assign back to Rigidbody
        rigid.velocity = velocity;

      
        
        Vector3 down = Vector3.down * rayDistance;
        Debug.DrawRay(rayOrigin.position, down, Color.green);
       
    }
    
    

    void SetJumpVars() {
        
        float jumpDistHalf = jumpDist * jumpApex;
        jumpVel = 2 * jumpHeight * speed / jumpDistHalf;
        jumpGrav = -2 * jumpHeight * (speed * speed) / (jumpDistHalf * jumpDistHalf);

        float fallingDistHalf = jumpDist - jumpDistHalf;
        jumpGravDown = -2 * jumpHeight * (speed * speed) / (fallingDistHalf * fallingDistHalf);
        
    }
    
    
    // Door interaction
    void OnTriggerStay(Collider other)
    {
        
        if (other.tag == "Door" && Input.GetKey( KeyCode.Space ))
        {
            Door door = other.gameObject.GetComponent<Door>();
            if(door.isOpened) return;
            door.OpenDoor();
        }
    }
}