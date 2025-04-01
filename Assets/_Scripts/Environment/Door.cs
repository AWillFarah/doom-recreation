using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpened = false;
    bool canClose = false;
    public float doorCloseDelay = 3f;
    public Animator doorAnimator;
    public AnimationClip doorOpen;
    public AnimationClip doorClose;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
       if(isOpened) return; 
       isOpened = true;
       doorAnimator.Play(doorOpen.name);
    }

    void Update()
    {
        if (isOpened && canClose)
        {
            Invoke("CloseDoor", doorCloseDelay);
        }
    }
// Checking if something is below the door
    void OnTriggerEnter(Collider other) { canClose = false; }
    void OnTriggerExit(Collider other) { canClose = true; }
    
    void CloseDoor()
    {
        if(!isOpened || !canClose) return;
        isOpened = false;
        doorAnimator.Play(doorClose.name);
    }
}
