using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

//This script handles moving the character on the Y axis, for jumping and gravity
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterGround))]
public class CharacterJump : MonoBehaviour
{
    // [Header("Components")]
    [InfoBox("If you want to see a prediction of the jump when this GameObject is selected, check showJumpLine in the Character_Settings_SO that you're using.", EInfoBoxType.Normal)]
    [XnTools.Hidden] public Rigidbody2D rigid;
    [XnTools.Hidden] public Vector2 velocity;

    private CharacterMovement movement;
    private CharacterGround   ground;
    [Header( "Platformer Character Settings" )]
    private Character_Settings_SO characterSettingsSO = null;


//    [Header("Jumping Stats")]
//    [SerializeField, Range(2f, 5.5f)][Tooltip("Maximum jump height")] public float jumpHeight = 7.3f;


////If you're using your stats from Platformer Toolkit with this CharacterJump, please note that the number on the Jump Duration handle does not match this stat
////It is re-scaled, from 0.2f - 1.25f, to 1 - 10.
////You can transform the number on screen to the stat here, using the function at the bottom of this script
//    [SerializeField, Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] public float timeToJumpApex;
//    [SerializeField, Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] public float upwardMovementMultiplier = 1f;
//    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float downwardMovementMultiplier = 6.17f;
//    [SerializeField, Range(0, 1)][Tooltip("How many times can you jump in the air?")] public int maxAirJumps = 0;

//    [Header("Options")]
//    [Tooltip("Should the character drop when you let go of jump?")] public bool variableJumpHeight;
//    [SerializeField, Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] public float jumpCutOff;
//    [SerializeField][Tooltip("The fastest speed the character can fall")] public float speedLimit;
//    [SerializeField, Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float coyoteTime = 0.15f;
//    [SerializeField, Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] public float jumpBuffer = 0.15f;

    [Header("Calculations")]
    public float jumpSpeed;
    private float defaultGravityScale;
    public float gravMultiplier;

    [Header("Current State")]
    public bool canJumpAgain = false;
    private bool desiredJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    private bool pressingJump;
    public bool onGround;
    private bool currentlyJumping;

    void Awake()
    {
        //Find the character's Rigidbody and ground detection and juice scripts
        movement = GetComponent<CharacterMovement>();
        rigid = GetComponent<Rigidbody2D>();
        ground = GetComponent<CharacterGround>();
        defaultGravityScale = 1f;
        characterSettingsSO = movement.characterSettingsSO;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //This function is called when one of the jump buttons (like space or the A button) is pressed.
        //When we press the jump button, tell the script that we desire a jump.
        //Also, use the started and canceled contexts to know if we're currently holding the button
        if (context.started)
        {
            desiredJump = true;
            pressingJump = true;
        }

        if (context.canceled)
        {
            pressingJump = false;
        }
    }

    private void SetPhysics() {
        //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
        Vector2 newGravity = new Vector2( 0, ( -2 * characterSettingsSO.jumpHeight ) / ( characterSettingsSO.jumpDuration.up * characterSettingsSO.jumpDuration.up ) );
        rigid.gravityScale = ( newGravity.y / Physics2D.gravity.y ) * gravMultiplier;
    }

    void Update()
    {


        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            coyoteTimeCounter = 0;
        }
    }



    private void FixedUpdate()
    {

        //Check if we're on ground, using Kit's Ground script
        onGround = ground.GetOnGround();

        SetPhysics();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if ( characterSettingsSO.jumpBuffer > 0 ) {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if ( desiredJump ) {
                jumpBufferCounter += Time.deltaTime;

                if ( jumpBufferCounter > characterSettingsSO.jumpBuffer ) {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }



        //Get velocity from Kit's Rigidbody 
        velocity = rigid.velocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump)
        {
            DoAJump();

            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            rigid.velocity = velocity; // Assign velocity to rigid because return will be called next line.
            return;
        }

        CalculateGravity();
        rigid.velocity = velocity;
    }

    private void CalculateGravity()
    {
        // I removed all references to the Rigidbody in this script. - JGB 2022-10-30

        //We change the character's gravity based on her Y direction

        //If Kit is going up...
        if ( velocity.y > 0.01f ) //if (rigid.velocity.y > 0.01f)
        {
            if (onGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if ( characterSettingsSO.jumpSettingsVariableHeight.useVariableJumpHeight )
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (pressingJump && currentlyJumping) {
                        gravMultiplier = characterSettingsSO.jumpGrav.up;// characterSettingsSO.upwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else {
                        gravMultiplier = characterSettingsSO.jumpGrav.up * characterSettingsSO.jumpSettingsVariableHeight.gravUpMultiplierOnRelease;// characterSettingsSO.jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = characterSettingsSO.jumpGrav.up;// characterSettingsSO.upwardMovementMultiplier;
                }
            }
        }

        //Else if going down...
        else if ( velocity.y < -0.01f ) //else if (rigid.velocity.y < -0.01f)
        {

            if (onGround)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                gravMultiplier = characterSettingsSO.jumpGrav.down; //characterSettingsSO.downwardMovementMultiplier;
            }

        }
        //Else not moving vertically at all
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        velocity.y = Mathf.Clamp( velocity.y, -characterSettingsSO.speedLimit, 100 );
        //rigid.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < characterSettingsSO.coyoteTime ) || canJumpAgain) {
            StairMaster.ON_STAIRS = false;
            
            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // Fix the gravity bug that led to double-height jumps when the timing was perfect
            gravMultiplier = defaultGravityScale;
            SetPhysics();

            //If we have double jump on, allow us to jump again (but only once)
            // canJumpAgain = ( characterSettingsSO.maxAirJumps == 1 && canJumpAgain == false);
            canJumpAgain = ( characterSettingsSO.jumpsBetweenGrounding > 1 && canJumpAgain == false);
            
            //Determine the power of the jump, based on our gravity and stats
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigid.gravityScale * characterSettingsSO.jumpHeight );
            if (jumpSpeed > 100) {
                 Debug.LogError( "Break" );
            }

            //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rigid.velocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }

        if ( characterSettingsSO.jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            desiredJump = false;
        }
    }

    public void BounceUp(float bounceAmount)
    {
        //Used by the springy pad
        rigid.AddForce(Vector2.up * bounceAmount, ForceMode2D.Impulse);
    }

/*

timeToApexStat = scale(1, 10, 0.2f, 2.5f, numberFromPlatformerToolkit)


  public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

*/
#if UNITY_EDITOR
    
    [CustomEditor( typeof(CharacterJump) )]
    public class CharacterJump_Editor : Editor {
        private const float dashSize = 4;
        
        private CharacterMovement cMove;
        private CharacterJump     cJump;
    
        private void OnEnable() {
            cJump = (CharacterJump) target;
            cMove = cJump.GetComponent<CharacterMovement>();
        }
    
    
        private void OnSceneGUI() {
            if ( cMove                     == null ) return;
            if ( cMove.characterSettingsSO == null ) return;
            Character_Settings_SO csso = cMove.characterSettingsSO;
            if ( !csso.showJumpLine ) return;
            if ( csso.jumpSettingsType == Character_Settings_SO.eJumpSettingsType.GMTK_GameMakersToolKit ) return;
            if (csso.jumpLinePoints == null) cMove.characterSettingsSO.CalculateJumpLine();

            GUIStyle labelStyle = new GUIStyle( EditorStyles.foldoutHeader );
            labelStyle.imagePosition = ImagePosition.TextOnly; // NOTE: This didn't seem to do anything.
            labelStyle.richText = true;
            
            Handles.matrix = Matrix4x4.Translate(cJump.transform.position);
            Handles.color = Color.green;
            Handles.DrawAAPolyLine(4, csso.jumpLinePoints);
            Vector3 tVec;
            Vector3[] jSME = csso.jumpStartMidEndPoints;
            if ( jSME != null && jSME.Length == 3 ) {
                Vector3 offset = Vector3.up * 0.2f;
                Handles.DrawDottedLine( jSME[0] + offset, jSME[2] + offset, dashSize );
                tVec = ( jSME[0] + jSME[2] ) / 2f + offset * 4 + Vector3.left * 0.4f;
                Handles.Label( tVec, $"<b>Dist: {csso.maxJumpDistHeight.x:0.##}</b>", labelStyle );
                tVec = jSME[1];
                tVec.y = 0;
                Handles.DrawDottedLine( tVec, jSME[1], dashSize );
                tVec = ( tVec + jSME[1] ) / 2f + Vector3.left * 0.4f;
                Handles.Label( tVec, $"<b>Height: {csso.maxJumpDistHeight.y:0.##}</b>", labelStyle );
            }

            if ( csso.jumpSettingsVariableHeight.useVariableJumpHeight ) {
                Handles.color = Color.magenta;
                Handles.DrawAAPolyLine( 8, csso.minJumpLinePoints.ToArray() );
                if ( csso.minJumpStartMidEndPoints        != null &&
                     csso.minJumpStartMidEndPoints.Length == 3 ) {
                    tVec = csso.minJumpStartMidEndPoints[0] + Vector3.down * 0.25f;
                    Handles.Label( tVec, $"<b>Min: Ht: {csso.minJumpDistHeight.y:0.##}   Dst: {csso.minJumpDistHeight.x:0.##}" +
                                         $"   tApex: {csso.minTimeApexFull.x:0.##}   tFull: {csso.minTimeApexFull.y:0.##}</b>", labelStyle );
                }
            }

        }
    }    
    
#endif



}