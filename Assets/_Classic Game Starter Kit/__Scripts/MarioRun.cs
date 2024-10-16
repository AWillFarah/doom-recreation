using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class MarioRun : MonoBehaviour {
    [Header("Inscribed")]
    public CharacterMovement     cMove;
    
    public Xnput.eButton runButton;
    
    [NaughtyAttributes.Expandable]
    public Character_Settings_SO cssoWalk, cssoRun;


    [Header("Dynamic")]
    public bool isRunning = false;
    
    // Start is called before the first frame update
    void Start() {
        isRunning = false;
        cMove.characterSettingsSO = cssoWalk;
    }

    // Update is called once per frame
    void Update() {
        bool shouldBeRunning = Xnput.GetButton( runButton );
        if ( isRunning != shouldBeRunning ) {
            isRunning = shouldBeRunning;
            // This ternary operator replaces the commented out if..else statement below
            cMove.characterSettingsSO = (isRunning) ? cssoRun : cssoWalk;
            // if ( isRunning ) {
            //     csso.maxSpeed = maxSpeedRun;
            // } else {
            //     csso.maxSpeed = maxSpeedWalk;
            // }
        }
    }
}



/*
public class MarioRun : MonoBehaviour {
    public Character_Settings_SO csso;

    public Xnput.eButton runButton;
    
    [Header("CSSO Walking Values")]
    public float maxSpeedWalk = 6;

    [Header( "CSSO Running Values" )]
    public float maxSpeedRun = 9;

    [Header("Dynamic")]
    public bool isRunning = false;
    
    // Start is called before the first frame update
    void Start() {
        isRunning = false;
    }

    // Update is called once per frame
    void Update() {
        bool shouldBeRunning = Xnput.GetButton( runButton );
        if ( isRunning != shouldBeRunning ) {
            isRunning = shouldBeRunning;
            // This ternary operator replaces the commented out if..else statement below
            csso.maxSpeed = (isRunning) ? maxSpeedRun : maxSpeedWalk;
            // if ( isRunning ) {
            //     csso.maxSpeed = maxSpeedRun;
            // } else {
            //     csso.maxSpeed = maxSpeedWalk;
            // }
        }
    }

    void OnDestroy() {
        csso.maxSpeed = maxSpeedWalk;
    }
}
*/