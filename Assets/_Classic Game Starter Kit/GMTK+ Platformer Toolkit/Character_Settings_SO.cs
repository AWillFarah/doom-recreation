using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Created by Jeremy Bond for MI 231 at Michigan State University
/// Built to work with a modified version of the GMTK Platformer Toolkit
/// </summary>
[CreateAssetMenu( fileName = "GMTK_Settings_[GameName]", menuName = "ScriptableObjects/GMTK_Settings", order = 1 )]
public class Character_Settings_SO : ScriptableObject {

    [Header( "Movement Stats" )]
    [SerializeField, Range( 0f, 20f )] [Tooltip( "Maximum movement speed" )] public float maxSpeed = 10f;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to reach max speed" )] public float maxAcceleration = 52f;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to stop after letting go" )] public float maxDeceleration = 52f;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to stop when changing direction" )] public float maxTurnSpeed = 80f;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to reach max speed when in mid-air" )] public float maxAirAcceleration = 0;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to stop in mid-air when no direction is used" )] public float maxAirDeceleration = 0;
    [SerializeField, Range( 0f, 100f )] [Tooltip( "How fast to stop when changing direction when in mid-air" )] public float maxAirTurnSpeed = 80f;
    [SerializeField] [Tooltip( "Friction to apply against movement on stick" )] public float friction = 0;

    [Header( "Movement Options" )]
    [Tooltip( "When false, the charcter will skip acceleration and deceleration and instantly move and stop" )] public bool useAcceleration = true;



    // NOTE: CGSK jummp math comes from Math for Game Programmers: Building a Better Jump
    // https://www.youtube.com/watch?v=hG9SzQxaCm8&t=9m35s & https://www.youtube.com/watch?v=hG9SzQxaCm8&t=784s
    // Th = Xh/Vx     V0 = 2H / Th     G = -2H / (Th * Th)     V0 = 2HVx / Xh     G = -2H(Vx*Vx) / (Xh*Xh) 
    
    [Header( "Jump Settings" )]
    public eJumpSettingsType jumpSettingsType = eJumpSettingsType.CGSK_Time;
    public enum eJumpSettingsType { CGSK_Distance, CGSK_Time, GMTK_GameMakersToolKit };

    public bool showJumpLine = true;
    
    [Tooltip( "Maximum jump height" )]
    [Range( 1f, 10f )] public float jumpHeight = 4f;

    [ShowIf("jumpSettingsType", eJumpSettingsType.CGSK_Time)]
    public CGSK_JumpSettings_Time jumpSettingsTime;
    
    [ShowIf("jumpSettingsType", eJumpSettingsType.CGSK_Distance)]
    public CGSK_JumpSettings_Distance jumpSettingsDistance;

    // [HideIf( "jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit )]
    // [XnTools.ReadOnly][BoxGroup("CGSK Derived Jump Properties")]
    // public float jumpDistUp, jumpDurationUp, jumpVelUp, jumpGravUp, jumpDistDown, jumpDurationDown, jumpVelDown, jumpGravDown;


    [HideIf( "jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit )]
    [BoxGroup( "CGSK Derived Jump Properties" )]
    public CSSO_FloatUpDown jumpDist, jumpDuration, jumpVel, jumpGrav;


    
    
    
    [Header("Jump Settings - GameMakers ToolKit")]
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [SerializeField, Range( 1f, 10f )] [Tooltip( "How long it takes to reach that height before coming back down" )]
    public float jumpDurationFromVideo = 5;
    //If you're using your stats from Platformer Toolkit with this character controller,
    // please note that the number on the Jump Duration handle does not match this stat
    // It is re-scaled, from 0.2f - 1.25f, to 1 - 10.
    [XnTools.ReadOnly]
    [SerializeField, Range( 0.1f, 1.25f )][ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [Tooltip( "For some reason, the GMTK version of the value in the video is different from the one here, so I calculate this from the other value." )]
    public float timeToJumpApex;
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [SerializeField, Range( 0f, 5f )] [Tooltip( "Gravity multiplier to apply when going up" )] public float upwardMovementMultiplier = 1f;
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [SerializeField, Range( 1f, 10f )] [Tooltip( "Gravity multiplier to apply when coming down" )] public float downwardMovementMultiplier = 6.17f;
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [SerializeField, Range( 0, 1 )] [Tooltip( "How many times can you jump in the air?" )] public int maxAirJumps = 0;
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [Tooltip( "Should the character drop when you let go of jump?" )] public bool variablejumpHeight;
    [ShowIf("jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit)]
    [SerializeField, Range( 1f, 10f )] [Tooltip( "Gravity multiplier when you let go of jump" )] public float jumpCutOff;
    
    [Header( "Jump Options" )]
    [SerializeField] [Tooltip( "The fastest speed the character can fall" )] public float speedLimit = 26.45f;
    [SerializeField, Range( 0f, 0.3f )] [Tooltip( "How long should coyote time last?" )] public float coyoteTime = 0.15f;
    [SerializeField, Range( 0f, 0.3f )] [Tooltip( "How far from ground should we cache your jump?" )] public float jumpBuffer = 0.15f;





    [Header( "Juice Settings - Squash and Stretch" )]
    [SerializeField] public bool squashAndStretch;
    [SerializeField, Tooltip( "Width Squeeze, Height Squeeze, Duration" )] public Vector3 jumpSquashSettings;
    [SerializeField, Tooltip( "Width Squeeze, Height Squeeze, Duration" )] public Vector3 landSquashSettings;
    [SerializeField, Tooltip( "How powerful should the effect be?" )] public float landSqueezeMultiplier;
    [SerializeField, Tooltip( "How powerful should the effect be?" )] public float jumpSqueezeMultiplier;
    [SerializeField] public float landDrop = 1;

    [Header( "Juice Settings - Tilting" )]

    [SerializeField] public bool leanForward;
    [SerializeField, Tooltip( "How far should the character tilt?" )] public float maxTilt;
    [SerializeField, Tooltip( "How fast should the character tilt?" )] public float tiltSpeed;





    private void OnValidate() {
        switch ( jumpSettingsType ) {
        case eJumpSettingsType.CGSK_Time:
            CalculateDerivedCGSKJumpValues_Time();
            break;
        
        case eJumpSettingsType.CGSK_Distance:
            CalculateDerivedCGSKJumpValues_Distance();
            break;
        
        case eJumpSettingsType.GMTK_GameMakersToolKit:
            timeToJumpApex = scale( 1, 10, 0.2f, 1.25f, jumpDurationFromVideo );
            break;
        }
        
        CalculateJumpLine();
    }

    
    static private int       jumpLineResolution = 64; // NOTE: This must be a positive even number
    internal       Vector3[] jumpLinePoints;
    [HideIf( "jumpSettingsType", eJumpSettingsType.GMTK_GameMakersToolKit )]
    [BoxGroup( "CGSK Derived Jump Properties" )][SerializeField][XnTools.ReadOnly]
    internal       Vector2   maxJumpDistHeight;
    internal       Vector3[] jumpStartMidEndPoints;
    internal void CalculateJumpLine() {
        if ( jumpSettingsType == eJumpSettingsType.GMTK_GameMakersToolKit ) {
            jumpLinePoints = null;
            return;
        }
        maxJumpDistHeight = Vector2.zero;
        Vector3 acc = new Vector3( 0, jumpGrav.up, 0 );
        Vector3 p = Vector3.zero;
        jumpLinePoints = new Vector3[jumpLineResolution];
        jumpStartMidEndPoints = new Vector3[3];
        jumpLinePoints[0] = p;
        jumpStartMidEndPoints[0] = p;
        Vector3 v = new Vector3( maxSpeed, jumpVel.up, 0 );
        int numSteps = jumpLineResolution / 2;
        // Jumping Up
        float timeStep = jumpDuration.up  / (float) numSteps;
        int i = 1;
        for ( ; i <= jumpLineResolution / 2; i++ ) {
            SimplifiedVelocityVerletIntegration(ref p, ref v, acc, timeStep);
            // p.x += v.x         * timeStep;
            // v.y += jumpGrav.up * timeStep;
            // p.y += v.y         * timeStep;
            jumpLinePoints[i] = p;
        }
        jumpStartMidEndPoints[1] = p;
        maxJumpDistHeight.y = p.y;
        // Jumping Down
        acc.y = jumpGrav.down;
        timeStep = jumpDuration.down / (float) (numSteps-1);
        for ( ; i < jumpLineResolution; i++ ) {
            SimplifiedVelocityVerletIntegration(ref p, ref v, acc, timeStep);
            // p.x += v.x           * timeStep;
            // v.y += jumpGrav.down * timeStep;
            // p.y += v.y           * timeStep;
            jumpLinePoints[i] = p;
        }
        jumpStartMidEndPoints[2] = p;
        maxJumpDistHeight.x = p.x;
    }
    
    // NOTE: Simplified Velocity Verlet Integration from Math for Game Programmers: Building a Better Jump
    // https://www.youtube.com/watch?v=hG9SzQxaCm8&t=23m2s
    void SimplifiedVelocityVerletIntegration( ref Vector3 pos, ref Vector3 vel, Vector3 acc, float deltaTime ) {
        pos += vel * deltaTime + acc * ( 0.5f * deltaTime * deltaTime ); // pos += vel*dT + 1/2*acc*dT*dT
        vel += acc * deltaTime;
    }

    public float scale( float OldMin, float OldMax, float NewMin, float NewMax, float OldValue ) {
        float OldRange = ( OldMax - OldMin );
        float NewRange = ( NewMax - NewMin );
        float NewValue = ( ( ( OldValue - OldMin ) * NewRange ) / OldRange ) + NewMin;

        return ( NewValue );
    }


    [System.Serializable]
    public class CGSK_JumpSettings_Time {
        [Header("Classic Game Starter Kit - Time Jump Settings")]
        [Tooltip( "The full duration of the shortest jump possible (by tapping the button)" )]
        public float fullJumpDurationMin = 0.5f;
        [Tooltip( "The full duration of the longest jump possible (by holding the button)" )]
        public float fullJumpDurationMax = 1f;
        [Tooltip("The fraction of the jump that is going up")]
        [Range( 0.05f, 0.95f )] public float jumpApexFraction = 0.6f;
    }
    
    private void CalculateDerivedCGSKJumpValues_Time() {
        jumpDuration.up = jumpSettingsTime.fullJumpDurationMax * jumpSettingsTime.jumpApexFraction;
        jumpDuration.down = jumpSettingsTime.fullJumpDurationMax - jumpDuration.up;
        jumpVel.up = jumpHeight   * 2 / jumpDuration.up;
        jumpVel.down = jumpHeight * 2 / jumpDuration.down; // This is the velocity when the character lands. - GB 2023-03-10
        jumpGrav.up = -2 * jumpHeight / ( jumpDuration.up * jumpDuration.up );
        jumpGrav.down = -2 * jumpHeight / ( jumpDuration.down * jumpDuration.down );
        jumpDist.up = jumpDuration.up * maxSpeed;
        jumpDist.down = jumpDuration.down * maxSpeed;
    }
    
    [System.Serializable]
    public class CGSK_JumpSettings_Distance {
        [Header("Classic Game Starter Kit - Distance Jump Settings")]
        [Tooltip( "The horizontal distance at full run speed of the shortest jump possible (by tapping the button)" )]
        public float fullJumpDistanceMin = 0.5f;
        [Tooltip( "The horizontal distance at full run speed of the longest jump possible (by holding the button)" )]
        public float fullJumpDistanceMax = 1f;
        [Tooltip("The fraction of the jump that is going up")]
        [Range( 0.05f, 0.95f )] public float jumpApexFraction = 0.6f;
    }
    
    private void CalculateDerivedCGSKJumpValues_Distance() {
        jumpDist.up = jumpSettingsDistance.fullJumpDistanceMax *
                       jumpSettingsDistance.jumpApexFraction;
        jumpDist.down = jumpSettingsDistance.fullJumpDistanceMax - jumpDist.up;
        // Th = Xh / Vh
        jumpDuration.up = jumpDist.up     / maxSpeed;
        jumpDuration.down = jumpDist.down / maxSpeed;
        // Vy = 2hVh / Xh
        jumpVel.up = 2   * jumpHeight * maxSpeed / jumpDist.up;
        jumpVel.down = 2 * jumpHeight * maxSpeed / jumpDist.down;
        // G = -2h(Vx*Vx) / (Xh*Xh)
        jumpGrav.up = -2   * jumpHeight * ( maxSpeed * maxSpeed ) / ( jumpDist.up   * jumpDist.up );
        jumpGrav.down = -2 * jumpHeight * ( maxSpeed * maxSpeed ) / ( jumpDist.down * jumpDist.down );
    }

}

[System.Serializable]
public class CSSO_FloatUpDown {
    public float up, down;
}

#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( CSSO_FloatUpDown ) )]
public class CSSO_FloatUpDown_Drawer : PropertyDrawer {
    static public GUIStyle styleLabelGray = null, styleLabelGrayBold = null; 
    // SerializedProperty m_stat;

    // Draw the property inside the given rect
    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
        // Init the SerializedProperty fields
        //if ( m_show == null ) m_show = property.FindPropertyRelative( "show" );
        //if ( m_recNum == null ) m_recNum = property.FindPropertyRelative( "recNum" );
        //if ( m_playerName == null ) m_playerName = property.FindPropertyRelative( "playerName" );
        //if ( m_dateTime == null ) m_dateTime = property.FindPropertyRelative( "dateTime" );

        CSSO_FloatUpDown fud = fieldInfo.GetValue( property.serializedObject.targetObject ) as CSSO_FloatUpDown;

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty( position, label, property );

        // Draw label
        //position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), GUIContent.none );// label );
        if ( styleLabelGray == null) {
            styleLabelGray = new GUIStyle( EditorStyles.label );
            styleLabelGray.richText = true;
        }

        string colorString = "#606060ff";

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        EditorGUI.LabelField(position, $"<b><color={colorString}>{property.displayName}</color></b>", styleLabelGray );
        EditorGUI.indentLevel = 8;
        EditorGUI.LabelField( position, $"<color={colorString}>up: {fud.up:0.0###}</color>", styleLabelGray );
        EditorGUI.indentLevel = 14;
        EditorGUI.LabelField( position, $"<color={colorString}>down: {fud.down:0.0###}</color>", styleLabelGray );

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}

// This was a bad idea because the CSSO doesn't know who the character is. Moved to CharacterJump
// [CustomEditor( typeof(Character_Settings_SO) )]
// public class CSSO_Editor : Editor {
//     private Character_Settings_SO csso;
//
//     private void OnEnable() {
//         csso = (Character_Settings_SO) target;
//     }
//
//
//     private void OnSceneGUI() {
//         if ( csso == null || csso.jumpLinePoints == null ) return;
//         
//         // Handles.matrix = Matrix4x4.Translate(); // Not needed because jump will be shown at origin.
//         Handles.color = Color.green;
//         Handles.DrawAAPolyLine(4, csso.jumpLinePoints);
//     }
// }


#endif
