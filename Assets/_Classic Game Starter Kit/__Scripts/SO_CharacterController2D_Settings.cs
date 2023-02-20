using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This separates the movement settings from the CharacterController2D script itself.
/// </summary>
[CreateAssetMenu( fileName = "CharacterController2D_Settings", menuName = "ScriptableObjects/CharacterController2D_Settings" )]
public class SO_CharacterController2D_Settings : ScriptableObject {
    
}
/*
/// <summary>
/// This separates the movement settings from the RockyCharacterController script itself.
/// </summary>
[CreateAssetMenu( fileName = "KinematicCharacterControllerSettings", menuName = "ScriptableObjects/KinematicCharacterControllerSettings" )]
public class SO_KinematicCharacterControllerSettings : ScriptableObject {

    [Header( "Stable Movement" )]
    public InspectorInfo infoStableMovement = new InspectorInfo( "Stable Movement",
        "The following values are used to calculate CrawlSpeed\n" +
        "  during OnValidate().", 
        true, false);
    public float MaxStableMoveSpeed = 10f;
    public float StableMovementSharpness = 15;
    public float OrientationSharpness = 10;
    public float CrawlMoveSpeedMultiplier = 0.5f;
    public float LongJumpMoveSpeedMultiplier = 1.5f;
    public float LongJumpCrouchTimeLimit = 0.25f;
    [KinematicCharacterController.ReadOnly]
    public float CrawlSpeed, LongJumpMoveSpeed;

    [Header( "Air Movement" )]
    public float MaxAirMoveSpeed = 10f;
    public float AirAccelerationSpeed = 5f;
    public float Drag = 0.1f;

    [Header( "Jumping" )]
    public bool AllowJumpingWhenSliding = false;
    public bool  AllowDoubleJump            = false;
    public bool  AllowWallJump              = false;
    public float WallJumpInflateValue       = 0.25f;
    public float JumpPreGroundingGraceTime  = 0f;
    public float JumpPostGroundingGraceTime = 0f;

    public InspectorInfo infoJump = new InspectorInfo( "Jump Velocity and Gravity Calculation",
        "The following values are used to calculate derived \n" +
        "  values like jump velocity & gravity in OnValidate().", 
        true, false);
    public float maxJumpHeight = 4;
    public float maxJumpDist   = 16;
    [Range( 0.5f, 1 )]
    public float jumpPeakPercent = 0.75f;
    [Tooltip("Multiplier for jumpVelocity to generate maxFallVelocity value.")]
    public float maxFallVelocityMultiple = 2;

    public float groundPoundGravMultiple = 2;
    public float groundPoundHoverTime = 0.2f;
    
    public float LongJumpHeightMultiplier = 0.5f;
    public float LongJumpDistMultiplier = 2f;

    [KinematicCharacterController.ReadOnly]
    public float jumpVelocity = 40, maxFallVelocity = 80;
    [KinematicCharacterController.ReadOnly]
    public float gravityUp = 40; // This is positive because it is multiplied by the Gravity.normalized direction
    [KinematicCharacterController.ReadOnly]
    public float gravityDown = 40; // This is positive because it is multiplied by the Gravity.normalized direction
    [KinematicCharacterController.ReadOnly]
    public float gravityGroundPound = 40; // This is positive because it is multiplied by the Gravity.normalized direction

    [KinematicCharacterController.ReadOnly]
    public float longJumpHeight, longJumpDist, longJumpVelocity, longJumpGravity;

    [Header( "Misc" )]
    public bool RotationObstruction;
    public Vector3 Gravity = new Vector3( 0, -30f, 0 );
    public bool OrientTowardsGravity = true;
    
    [Header("NoClip")]
    public float NoClipMoveSpeed = 10f;
    public float NoClipSharpness = 15;

    /// <summary>
    /// Calculates gravityUp and gravityDown
    /// </summary>
    void OnValidate() {
        CrawlSpeed = MaxStableMoveSpeed * CrawlMoveSpeedMultiplier;
        
        float distUp = maxJumpDist   * jumpPeakPercent;
        float distDown = maxJumpDist * ( 1 - jumpPeakPercent );
        jumpVelocity = 2 * maxJumpHeight * MaxStableMoveSpeed / distUp;
        maxFallVelocity = -jumpVelocity * maxFallVelocityMultiple;
        gravityUp = 2 * maxJumpHeight * MaxStableMoveSpeed * MaxStableMoveSpeed
                    / ( distUp * distUp );
        gravityDown = 2 * maxJumpHeight * MaxStableMoveSpeed * MaxStableMoveSpeed
                    / ( distDown * distDown );
        gravityGroundPound = groundPoundGravMultiple * gravityDown;

        LongJumpMoveSpeed = MaxStableMoveSpeed * LongJumpMoveSpeedMultiplier;
        longJumpHeight = maxJumpHeight * LongJumpHeightMultiplier;
        longJumpDist = maxJumpDist * LongJumpDistMultiplier;
        float longJumpDistHalf = longJumpDist * 0.5f;
        longJumpVelocity = 2 * longJumpHeight * LongJumpMoveSpeed / longJumpDistHalf;
        longJumpGravity = 2 * longJumpHeight * LongJumpMoveSpeed * LongJumpMoveSpeed
                      / ( longJumpDistHalf * longJumpDistHalf );
    }
}
*/
