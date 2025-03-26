using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject player;
    
    private Health health;
    public TMP_Text healthText;
    void Start()
    {
        health = player.GetComponent<Health>(); 
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = ("HP:" + health.currentHealth);
    }
}
