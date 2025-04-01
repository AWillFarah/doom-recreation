using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject player;
    private Health health;
    private Weapon ammo;
    public TMP_Text healthText;
    public TMP_Text armorText;
    public TMP_Text ammoText;
    
    void Start()
    {
        health = player.GetComponent<Health>();
        ammo = player.GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = ("HP:" + health.currentHealth);
        armorText.text = ("Armor:" + health.currentArmor);
        switch (ammo.currentWeapon)
        {
            case(eWeaponType.pistol):
                ammoText.text = ("Ammo: " + Weapon.PISTOLAMMO);
                break;
            case(eWeaponType.shotgun):
                ammoText.text = ("Ammo: " + Weapon.SHOTGUNAMMO);
                break;
            default:
                ammoText.text = "";
                break;
        }
    }
}
