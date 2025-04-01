using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pickup : MonoBehaviour
{
    private enum PickupType
    {
        health, ammo, armor
    }
    
    [Header("Inscribed")]
    [SerializeField] PickupType type;
    public eWeaponType ammoType;
    public bool isBonus;
    public int amount;
    
    
    
    void OnTriggerEnter(Collider other)
    {
        // Only player can grab pickups
        if(other.tag != "Player") return;
        Health health = other.GetComponent<Health>();
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        switch (type)
        { 
            case PickupType.health:
                health.ChangeHealth(amount, true, false, isBonus);
                break;
            case PickupType.armor:
                health.ChangeHealth(amount, false, true, isBonus);
                break;
            case PickupType.ammo:
                weapon.Reload(amount, ammoType);
                break;
        }
        Destroy(gameObject);
    }
}
