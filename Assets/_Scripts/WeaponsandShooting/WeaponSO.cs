using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eWeaponType
{
    none, // default or no weapon
    pistol, 
    shotgun, // multiple shots simultaneously
    fireball, // For imp
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Stats")]
public class WeaponSO : ScriptableObject
{
    public eWeaponType weaponType;
    public float damageMin = 5f;
    public float damageMax = 15f;
    [Tooltip("Shots per second")]
    public float rateOfFire = 2.5f;
    public float range = 2048f;
    public GameObject ammoType = null;
    public int weaponSlot = 2;
    public float offsetMin = 0f;
    public float offsetMax = 5.6f;
    public int numberOfShots = 1;
    
    [Header("Projectile Based Weapons")]
    public bool useProjectile = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
}
